# Этап: API и клиенты

Автор: Claude Code, claude-opus-5-5, 2026-10-20

См. общий контекст и место этого этапа: `ai/plans/freem-architecture/general/roadmap.md` (этап 6).

## Зачем именно так

Решения пользователя (roadmap, 2026-10-18): контроллеры — только маршрутизация, простой маппинг и маппинг результата,
обработка через медиатор, как в StashIt; отдельный слой API-уровневых use case'ов (`Api.Public.UseCases`);
`SyncClient` убирается; `FunctionalTests` → `IntegrationTests`. Этап 5 сознательно оставил в контроллерах маппинг
`Error` → HTTP-код и отложил сюда формат тела ошибок (`ErrorResponse`), потому что он меняет контракт для
фронтенда и клиентов. После этапа 5 доменный слой уже на `Utilities.UseCases` и `Result<…, Error>`, поэтому
API-слой ложится поверх него без изменений домена.

## Зафиксированные факты

**Freem, API (`Freem.Web.Api.Public`):**

- 32 контроллера по одному на endpoint (`Controllers/V1/<Ресурс>/<Действие><Ресурс>Controller.cs`) + 4 auth-контроллера.
  Каждый получает `UseCaseContextProvider` и `IRequestMediator<HandlerContext>`, вызывает `TryGet` (нет
  пользователя — 401), маппит DTO в доменный запрос, вызывает медиатор и маппит `Result` в `IActionResult`
  собственным `CreateFailure` (20–40 строк на контроллер). Общая база `BaseController` (`[ApiController]`,
  `[Authorize]`, JSON).
- Сейчас по контроллерам (без auth): `EntityNotFound` → 404 (15), 401 (4: `Me`, `StatisticsPerDays`,
  `StatisticsPerPeriod`, `GetUserSettings` — не найден пользователь Freem), 403 (1: `UpdateSettings`);
  `RelatedEntitiesNotFound` → 422 (6); `ActivityInvalidStatus`, `TagNameAlreadyExists`, `PeriodIsInvalid` → 422;
  `NothingToDo` → 400 (5); `EntityAlreadyExists` → 409 (регистрация); остальное — 500. Тела ошибок нет.
- Обработчика исключений нет (необработанное исключение — 500 без тела); ошибки валидации модели — стандартный
  `ProblemDetails` ASP.NET. DTO `ErrorResponse(Code, Message, Properties)` в `Api.Public.Contracts` есть, но нигде не
  используется.
- Claim `sub` проверяется при аутентификации (`OnTokenValidated` в `Program.cs` отклоняет токен без разбираемого
  `sub`), поэтому под `[Authorize]` проверка `TryGet` → 401 по факту недостижима.
- Auth-контроллеры (`login`/`refresh`/`logout`/`register`, `[AllowAnonymous]`) работают с клиентом EgoHub напрямую
  через `EgoHubAuthSupport`, медиатор не используют.
- Фронтенд (`Freem.Web`) тела ошибок не читает, смотрит только коды: 401 на `GET user/me` — «не зарегистрирован»,
  401 на логине, 409 на регистрации, 404 в `ActivitiesDashboard`.

**Freem, клиенты и тесты:**

- `Freem.Web.Api.Public.Client` (14 файлов): клиенты по ресурсам + `CompositeClient`, `BaseClient` поверх
  `Utilities.Http`, ответ — собственный `ClientResult`/`ClientResult<T>` (код ответа + значение, тело ошибки не
  читается), токен из `ClientSession`.
- `Freem.Web.Api.Public.SyncClient` (10 файлов) — синхронные обёртки; используется только
  `Freem.Web.Api.Public.FunctionalTests`: в `TestContext` и препарерах данных (`Context/Preparers`, вызовы из
  конструкторов тестов) и в одном тесте (`UpdateRunningRecordTests`).
- `Freem.Web.Api.Public.FunctionalTests`: 65 тестов, хост поднимается в процессе (`WebApplicationFactory<Program>`),
  проверяются коды ответов. Путь проекта зашит в `docker/test/Dockerfile`
  (`configs/Freem.Web.Api.Public.FunctionalTests.configuration.json`), упоминается в `CLAUDE.md`.

**StashIt (`Core/`):**

- `Stashit.Core.Api.Public.UseCases.Contracts`: `ApiXRequest : IRequest` с фабрикой `Create(dto)` и каталог
  `RequestTypes` (`api.tags.create` и т. п.). `Stashit.Core.Api.Public.UseCases`: контексты
  `ApiAuthorizedHandlerContext(UserId, RequestId)` и `ApiAnonymousHandlerContext`; базы
  `BaseAuthorizedRequestHandler<TRequest>`/`BaseAnonymousRequestHandler<TRequest>` →
  `IRequestHandler<TContext, TRequest, IActionResult>` с доменным `IRequestMediator<HandlerContext>`; handler —
  три строки (`context.Map()`, `request.Map()`, вызов, `response.Map()`); мапперы `Mappers/{Requests,Responses,Dtos}`.
  `CommonResponseMapper.MapStatusCode(ErrorType)` — одно место выбора кода, ошибка кладётся в
  `ErrorResponse { Error }` (`Utilities.Errors.Error`). Регистрация — `AddApiUseCases()` с двумя медиаторами
  (авторизованный и анонимный) и аудитом (`Utilities.UseCases.Audit`).
- Контроллеры — по одному на ресурс (`TagsController` со всеми действиями), метод — две-три строки:
  `HttpContext.ToAuthorizedHandlerContext()` (бросает, если claim нет), `ApiXRequest.Create(body)`,
  `Mediator.ExecuteAsync<ApiXRequest, IActionResult>`. Auth (login/refresh/logout/register) — тоже API handlers на
  анонимном контексте.
- `ErrorHandling`: `GlobalExceptionHandler` (`FormatException` → 400, остальное → 500, тело `ErrorResponse`) и
  `InvalidModelStateResponseFactory` (400 с `ErrorResponse`).
- Клиент возвращает `Result<T, IClientError>` (`Utilities.Http.Errors`); разбор тела ошибки — собственный
  `Stashit.HttpClient` (191 строка: `BaseHttpClient`, `ClientErrorFactory`, `ClientError`), его в
  `Utilities.*` нет. `SyncClient` в StashIt нет.
- `Stashit.Core.Api.Public.IntegrationTests` ходят в запущенный хост по HTTP (адрес из `configuration.json`), а не
  через `WebApplicationFactory`.

**Пакеты:** `Utilities.Errors.Serialization.Json` 0.0.1 и `Utilities.UseCases.Audit` 0.0.1 есть в
`CSharpUtilities/docker/out/packages`, в `source/packages/` их нет; `Utilities.Http.Errors` 0.0.2 там уже лежит.

## Один-два иллюстративных примера (не исчерпывающий список!)

- `CreateTagController` (70 строк): `TryGet`, `Map`, вызов медиатора, `CreateSuccess` с `CreatedResult`,
  `CreateFailure` со сравнением `ErrorType`. В StashIt тот же endpoint — 4 строки в `TagsController.CreateAsync`
  и `ApiCreateTagRequestHandler` + `TagResponseMapper.Map(Result<CreateTagResponse, Error>)`.
- `MeController`: `EntityNotFound` (пользователь Freem не создан) → 401; на этом коде фронтенд определяет статус
  «unregistered». Общий маппер «не найдено → 404» этот код сломал бы.

## Направление решения (не пошаговая реализация)

Решения пользователя (2026-10-20) по открытым вопросам ниже: 1 — вариант (а); 2 — переходим на `ErrorResponse
{ Error }` вместе с `GlobalExceptionHandler` и `InvalidModelStateResponseFactory`; 3 — контроллеры объединяются по
ресурсам; 4 — клиент переходит на `Result<T, IClientError>`, разбор ошибок переносится из `Stashit.HttpClient`;
5 — аудит не подключаем.

Направление:

- Новые проекты `Freem.Web.Api.Public.UseCases` и `.UseCases.Contracts` (имена по нынешней схеме, переименование —
  этап 7): `ApiXRequest` + `RequestTypes`, контексты `ApiAuthorizedHandlerContext`/`ApiAnonymousHandlerContext`,
  базовые handlers, мапперы, `CommonResponseMapper`, `AddApiUseCases()`. Доменный слой не меняется.
- Контроллеры — маршрутизация и вызов API-медиатора, контекст из `HttpContext` (как в StashIt, без `TryGet`: `sub`
  уже гарантирован аутентификацией). `UseCaseContextProvider` удаляется.
- Auth-endpoints переходят на API handlers с анонимным контекстом; логика `EgoHubAuthSupport` переезжает в них.
- `SyncClient` удаляется; препареры тестов становятся async (`IAsyncLifetime`, как в тестах use case'ов на этапе 5).
- `FunctionalTests` → `IntegrationTests` (`git mv`, namespace, `InternalsVisibleTo`, `docker/test`, `CLAUDE.md`);
  хост остаётся в процессе (`WebApplicationFactory`).

Открытые вопросы (решены пользователем 2026-10-20, см. выше):

1. **Коды ответов.** Общий маппер по `ErrorType`, как в StashIt, при котором часть кодов меняется, или сохранить
   нынешние коды поэндпоинтно? Расхождения: «пользователь Freem не найден» сейчас 401 в 4 endpoint'ах и 403 в
   `UpdateSettings` (фронтенду нужен 401 на `user/me`). Варианты: (а) общий маппер + отдельный тип ошибки каталога
   «пользователь не зарегистрирован» → 401 везде (правка `DomainErrorCatalog` и use case'ов, 403 → 401);
   (б) общий маппер, а 401 для user-endpoint'ов задаётся в их API handlers (403 → 401 или оставить); (в) коды
   полностью как сейчас, маппинг в каждом handler. Предлагаю (а).
2. **Тело ошибок.** Перейти на `ErrorResponse { Error }` с `Utilities.Errors.Error` (как StashIt, нужен
   `Utilities.Errors.Serialization.Json` в `packages/`)? Фронтенд тела не читает, поэтому ломается только контракт
   C#-клиента. Вместе с этим — `GlobalExceptionHandler` и `InvalidModelStateResponseFactory` (400/500 с тем же
   телом). Предлагаю да, всё вместе.
3. **Контроллеры по ресурсам.** Сливать 32 контроллера в ~9 по ресурсам (`TagsController` и т. д.), как в StashIt,
   или оставить по одному на endpoint, только упростив? Маршруты в обоих случаях не меняются. Предлагаю сливать.
4. **Клиент.** Перевести async-клиент на `Result<T, IClientError>` с разбором `ErrorResponse` (портировать
   `ClientErrorFactory`/`ClientError` из `Stashit.HttpClient` в клиент Freem) или оставить `ClientResult` и только
   удалить `SyncClient`? Предлагаю `Result<T, IClientError>`, если по п. 2 тело ошибок вводится.
5. **Аудит use case'ов** (`Utilities.UseCases.Audit`, `WithAuditLogging`) — подключать на этом этапе? Предлагаю нет,
   вне рамок этапа.

## Явно не входит в этот файл (пока)

Переименование проектов и namespace'ов (`Freem.Web.Api.*` → `Freem.Api.Public.*`) и имён DTO — этап 7. Перевод
API-тестов на внешний хост по HTTP, как в StashIt. Unit-тесты API handlers (в StashIt есть для auth). Доменный
слой, хранилище и фронтенд не меняются (кроме, возможно, `DomainErrorCatalog` по п. 1).

## Критерий готовности

- В решении нет `Freem.Web.Api.Public.SyncClient` и `.FunctionalTests`; есть `.IntegrationTests` и
  `Freem.Web.Api.Public.UseCases(.Contracts)`; контроллеры не содержат маппинга ошибок и логики, только вызов
  API-медиатора.
- `dotnet build Freem.sln` — 0 предупреждений, 0 ошибок; unit-тесты зелёные.
- `docker/test` зелёный у пользователя (интеграционные тесты хранилища, use case'ов и API).
- Маршруты не меняются; коды ответов — как сейчас, кроме согласованных по п. 1; тела успешных ответов те же, тело
  ошибок — по решению п. 2 (тесты API на тело ошибки для ключевых случаев).
- Фронтенд: `npm run build` проходит, коды, на которые он опирается (401 на `user/me`, 401 логина, 409 регистрации,
  404), сохранены.

## План реализации

После каждого шага решение собирается (`dotnet build Freem.sln`, 0 предупреждений) и проходят unit-тесты; тесты,
затронутые шагом, правятся в нём же.

1. **Каталог ошибок и пакеты.** Скопировать `Utilities.Errors.Serialization.Json` 0.0.1 в `packages/`, дописать в
   список пакетов в `CLAUDE.md`. В `DomainErrorCatalog` добавить `UserNotRegistered` и `InvalidRequest` (для
   обработчика исключений и валидации модели). «Пользователь не найден» сейчас приходит из репозитория как общий
   `EntityNotFound` с `UserId`, поэтому use case'ы, читающие текущего пользователя (`GetUser`, `GetUserSettings`,
   `UpdateUserSettings`; статистика получает ошибку через вложенный `GetUserSettings`), переводят его в
   `UserNotRegistered`. Тесты use case'ов на эти случаи — на новый тип.
2. **Контракты ошибок и обработка исключений.** `ErrorResponse { Error }` (`Utilities.Errors.Error`) вместо
   нынешнего неиспользуемого DTO; конвертеры `Error` в JSON-настройках API; `ErrorHandling` по образцу StashIt:
   `GlobalExceptionHandler` (`FormatException` → 400, остальное → 500, тело `ErrorResponse`) и
   `InvalidModelStateResponseFactory` (400 с `ErrorResponse`).
3. **`Freem.Web.Api.Public.UseCases(.Contracts)`.** Новые проекты: `RequestTypes` (`api.<ресурс>.<действие>`),
   `ApiXRequest` с `Create(...)` из DTO, контексты `ApiAuthorizedHandlerContext(UserId, RequestId)` и
   `ApiAnonymousHandlerContext`, базы `BaseAuthorizedRequestHandler`/`BaseAnonymousRequestHandler`, handlers на
   каждый из 36 endpoint'ов, мапперы `Mappers/{Requests,Responses}`, `CommonResponseMapper` (`EntityNotFound` → 404,
   `UserNotRegistered` → 401, `RelatedEntitiesNotFound`/`ActivityInvalidStatus`/`TagNameAlreadyExists`/
   `PeriodIsInvalid` → 422, `NothingToDo`/`InvalidRequest` → 400, `EntityAlreadyExists` → 409, остальное → 500),
   `AddApiUseCases()` с двумя медиаторами, без аудита. Auth-handlers на анонимном контексте, `EgoHubAuthSupport` и
   работа с cookie — через `IHttpContextAccessor`, как в StashIt; ошибки EgoHub маппятся как сейчас (401/502).
   `RequestId` доменного контекста берётся из API-контекста.
4. **Контроллеры.** 36 контроллеров сливаются в контроллеры по ресурсам (`Activities`, `Auth`, `Events`, `Records`,
   `RunningRecords`, `Statistics`, `Tags`, `User` (вместе с настройками) — состав уточню по маршрутам), метод —
   контекст из `HttpContext`, `ApiXRequest.Create(...)`, вызов API-медиатора. `[ProducesResponseType<ErrorResponse>]`
   для кодов ошибок. `UseCaseContextProvider` и маппинг в контроллерах удаляются. Маршруты сверяются с HEAD по
   OpenAPI-документу (до и после).
5. **Клиент.** В `Freem.Web.Api.Public.Client`: `ClientError : IClientError` (код ответа + разобранный `Error`, если
   тело есть), `ClientErrorFactory` по образцу `Stashit.HttpClient`; методы возвращают `Result<T, IClientError>`/
   `Result<IClientError>`; `ClientResult` удаляется. `Freem.Web.Api.Public.SyncClient` удаляется.
6. **Тесты API.** `git mv` `FunctionalTests` → `IntegrationTests` (namespace, `InternalsVisibleTo`, `docker/test`
   Dockerfile и config-файл, `CLAUDE.md`). `TestContext` без `SyncClient`, препареры — async, подготовка данных — в
   `IAsyncLifetime.InitializeAsync`. Проверки — на `Result`/код ответа; новые тесты на тело ошибки: 404
   `entity_not_found`, 401 `user_not_registered` на `user/me` и настройках, 400 на невалидную модель.
7. **Чистка и сверка.** Лишние `using` и сортировка, BOM относительно HEAD, grep строковых ссылок на удалённые
   сборки и типы, бит skip-worktree у `appsettings.Development.json`, число проектов в `CLAUDE.md` (28 → 29) и строки
   "Repository Layout". JSON успешных ответов сверяется с HEAD, если затронуты DTO. Фронтенд не меняется,
   проверка `npm run build`.
8. **Проверка пользователем:** `docker/test`.

Изменения внешнего поведения (согласованы): 403 → 401 в `PUT user/settings` для незарегистрированного
пользователя; у всех ошибок появляется тело `ErrorResponse`; 400 валидации модели — `ErrorResponse` вместо
`ProblemDetails`; необработанное исключение — 500 с телом.

## Статус и изменения

- 2026-10-20 (Claude Code, claude-opus-5-5): план согласован пользователем; шаги 1–7 выполнены. Новые проекты
  `Freem.Web.Api.Public.UseCases(.Contracts)` (36 API handlers, `CommonResponseMapper`, два медиатора без аудита),
  `SyncClient` удалён, `FunctionalTests` → `IntegrationTests` (`.csproj` — 29, было 28). `dotnet build Freem.sln` —
  0 предупреждений, 0 ошибок; unit-тесты зелёные (5 проектов, 316); `npm run build` фронтенда проходит; хост
  запускается локально (`ValidateOnBuild`), список маршрутов и параметров OpenAPI совпадает с эталоном до изменений.
  `docker/test` не запускался, ждёт пользователя; не закоммичено, изменения проиндексированы (нужно для `git mv`).
  Расхождения с планом и решения по ходу:
  - каталог: кроме `UserNotRegistered`/`InvalidRequest` добавлены `AuthenticationRejected` (`auth:rejected`, ошибки
    EgoHub с проходным кодом 400/401/409/422 и `refresh` без cookie) и `FailedDependency` (502), а также фабрика
    `CreateTagNotFound(TagName)` для `GET tags/by-name` с пустым результатом (раньше 404 без тела); auth-handlers
    задают код сами, как раньше в `EgoHubAuthSupport`;
  - API-запросы держат DTO целиком (`Body`/`Query`) и Id из маршрута, без копирования полей по образцу StashIt;
    маппинг DTO → доменный запрос перенесён из контроллеров в `Mappers/Requests` без изменений логики;
  - Id в маршрутах привязываются типами (`ActivityId`, `TagName` и т. д. через `IParsable`) вместо `string` +
    `Parse`: неразбираемый Id раньше давал 500 (`FormatException` без обработчика), теперь 400 `invalid_request`;
  - списки отдаются через `ItemsListResult<T>` (заголовки `X-ITEMS-COUNT`/`X-TOTAL-ITEMS-COUNT` при выполнении
    результата), тело — тот же JSON-массив; `ProducesHeader` читается с метода, а не с класса контроллера;
  - документация OpenAPI исправлена там, где расходилась с поведением: 422 у `unarchive`, 401 у `events`, 400 у
    `tags/by-name`, тег `Statistics` у `per-period`, схемы ответов `GET records/running` и `GET tags/{tagId}` (были
    схемы запроса и доменного ответа);
  - клиент: `RecordsClient.ListAsync`/`ListByPeriodAsync` возвращали `RunningRecordResponse` вместо `RecordResponse`
    — исправлено вместе с переходом на `Result<IReadOnlyList<T>, IClientError>`;
  - тесты API: проверки кодов → `ShouldSuccess`/`ShouldHaveStatusCode`, подготовка данных — в `InitializeAsync`;
    новые тесты: 404 `entity_not_found` и 400 `invalid_request` (`GetTagTests`), 401 `user:not_registered` на
    `user/me`, `GET`/`PUT user/settings` (`UserNotRegisteredTests`); тесты use case'ов на `UserNotRegistered` для
    настроек;
  - находки вне этапа (не исправлялись): `ListRecordRequestModelBinder` путает `offset`/`limit`, а `GET records`
    объявляет параметры `[FromRoute]`, поэтому query-параметры, вероятно, игнорируются; схемы `Error` в OpenAPI
    показывают внутреннюю структуру `ErrorType`/`ErrorMessage` вместо строк (в StashIt для этого есть transformers).
- 2026-10-20 (Claude Code, claude-opus-5-5): по решению пользователя исправлены обе находки вне этапа.
  `GET records`: параметры объявлены `[FromQuery]` (было `[FromRoute]`, из-за чего query игнорировался и в OpenAPI
  у операции не было параметров), в `ListRecordRequestModelBinder` больше не перепутаны `offset` и `limit`; это
  меняет поведение для фронтенда, который передаёт query в `GET records`: параметры начнут учитываться. Регрессионные
  тесты `ListRecordTests.Request_ShouldApplyLimit`/`Request_ShouldApplyOffset`. OpenAPI: `ErrorsSchemaTransformer`
  (по образцу `Stashit.OpenApi`) описывает `ErrorType`/`ErrorMessage` строками и `properties` — объектом «имя →
  строка», как их пишут конвертеры `Utilities.Errors.Serialization.Json`. Сборка 0/0, unit-тесты зелёные, схемы
  сверены по запущенному хосту; `docker/test` ждёт пользователя.
- 2026-10-20 (Claude Code, claude-opus-5-5): `docker/test` у пользователя: упал
  `GetTagTests.Request_ShouldReturnInvalidRequestError_WhenTagIdIsMalformed` (404 вместо 400). Ошибка в тесте:
  `TryParse` идентификаторов отклоняет только пустую строку и строку из пробелов (этап 4), поэтому `not-an-id` —
  валидный `TagId` несуществующего тега. Тест переведён на `api/v1/tags/%20`; повторный `docker/test` ждёт
  пользователя.
- 2026-10-20 (Claude Code, claude-opus-5-5): строгий формат Id не вводим (решение пользователя: Id — любая непустая
  строка). Повторный `docker/test`: тест на `%20` получил 400 в формате `ProblemDetails`, а не `ErrorResponse`.
  Причина: `Configure<ApiBehaviorOptions>` был зарегистрирован до `AddControllers()`, и стандартная настройка MVC
  перезаписывала `InvalidModelStateResponseFactory`; настройка перенесена в
  `AddControllers().ConfigureApiBehaviorOptions(...)`, локально проверено (`POST auth/login` с битым JSON → 400
  `freem:invalid_request`). Найдено вне этапа, не исправлялось: тело с отсутствующими обязательными полями
  (`POST auth/login` с `{}`) даёт 500, потому что конструктор DTO бросает `ArgumentNullException` при
  десериализации — так было и до этапа. Повторный `docker/test` ждёт пользователя.
- 2026-10-20 (Claude Code, claude-opus-5-5): `docker/test` зелёный у пользователя. По его решению исправлена находка
  с 500 на невалидные данные запроса: проверки в конструкторах DTO (`ArgumentNullException`/`ArgumentException`/
  `ArgumentOutOfRangeException`) при разборе тела и в query-биндерах давали необработанное исключение. DTO не
  менялись: `ArgumentValidationJsonInputFormatter` (декоратор над `SystemTextJsonInputFormatter`, подключается
  через `IPostConfigureOptions<MvcOptions>`) и `ArgumentValidationModelBinder` (обёртка над биндерами
  `QueryListModelBinderProvider`) превращают такие исключения в ошибку модели → 400 `invalid_request`; исключения из
  handlers по-прежнему 500. `InvalidModelStateResponseFactory` отдаёт текст ошибки и при пустом ключе модели (тело
  запроса). Проверено локально (`POST auth/login` с `{}` и с пробельным логином → 400 с сообщением). Новые тесты:
  `LoginTests.Request_ShouldReturnInvalidRequestError_WhenBodyMissesFields`,
  `ListActivityTests.Request_ShouldReturnInvalidRequestError_WhenLimitIsNegative`; разбор JSON-ошибки вынесен в
  `HttpResponseMessageAssertExtensions`. Попутно удалены лишние `using` (IDE0005), пропущенные проверкой на шаге 7:
  та проверка по решению целиком ничего не находила, повторная — по каждому затронутому проекту. Сборка 0/0,
  unit-тесты зелёные; повторный `docker/test` ждёт пользователя.
- 2026-10-20 (Claude Code, claude-opus-5-5): повторный `docker/test` зелёный у пользователя; критерий готовности
  выполнен, ждёт закрытия пользователем, не закоммичено.
- 2026-10-20 (Claude Code, claude-opus-5-5): этап закрыт пользователем (независимая проверка не проводилась); реализация закоммичена (`604677f`).
