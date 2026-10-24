# Этап: UseCases

Автор: Claude Code, claude-opus-5-5, 2026-10-19

См. общий контекст и место этого этапа: `ai/plans/freem-architecture/general/roadmap.md` (этап 5).

## Зачем именно так

Решение пользователя (roadmap, 2026-10-18): собственные `Freem.Results`/`Freem.UseCases*` заменяются пакетами
`Utilities.UseCases` и `Utilities.Primitives` (`Result<TValue, TError>`), как в StashIt и Trandoma. Сейчас во Freem
четыре проекта инфраструктуры use case'ов (свой исполнитель, реестр типов, резолвер), ответ каждого use case'а —
отдельный класс с `Success`/`Error<enum>`, а ошибки хранилища приходят исключениями, которые use case'ы ловят и
разбирают по типам Id. Пакетный медиатор и `Result` убирают эту инфраструктуру и делают ошибки частью сигнатуры.
На этап 5 отложены также решения этапов 2–3: `RepositoryContext`, `Result<…, Error>` и `AsyncBatch<T>` в
репозиториях, `Freem.Storage.*` против `Utilities.Storage.Transactions`.

## Зафиксированные факты

**Freem, инфраструктура use case'ов:**

- `Freem.UseCases.Abstractions` (`IUseCase<TContext, TRequest, TResponse, TErrorCode>`, `IUseCaseExecutor<TContext>`,
  `IUseCaseResolver`, `UseCaseException<TContext>`), `Freem.UseCases.Contracts.Abstractions` (`IResponse<TErrorCode>`,
  `Error<TCode>` с `IReadOnlyDictionary<string, string>? Properties`), `Freem.UseCases.Types` (реестр типов и
  `AddUseCases<TContext>`), `Freem.UseCases` (`UseCaseExecutor`, `MicrosoftAdapterUseCaseResolver`).
- `Freem.Entities.UseCases`: 33 use case'а (`IEntitiesUseCase<…>`) по папкам агрегатов, регистрация через
  `AddEntitiesUseCases`. Контекст — `UseCaseExecutionContext` с `UserId?`; каждый use case начинает с
  `context.ThrowsIfUnauthorized()` (`UnauthorizedException`). Обработчика исключений в `Program.cs` нет.
- `Freem.Entities.UseCases.Contracts`: на каждый use case — `Request`, `Response : IResponse<ErrorCode>` с фабриками
  `CreateSuccess`/`CreateFailure` и `enum <UseCase>ErrorCode`; ответы отдают доменные сущности (`CreateTagResponse.Tag`).
- Вложенные вызовы через исполнитель: `StartRunningRecord` → `StopRunningRecord`, `StopRunningRecord` →
  `CreateRecord`, `StatisticsPerDays`/`StatisticsPerPeriod` → `GetUserSettings`. В первых двух ответ вложенного вызова
  не проверяется: если `CreateRecord` вернёт ошибку, `StopRunningRecord` всё равно удалит запущенную запись и
  закоммитит транзакцию.
- `Freem.Results` (`Result<T>` без ошибки) используется только в `Freem.Entities.UseCases.Plain`; `Plain`
  (async- и sync-обёртки, бросающие `PlainRequestFailedException`) используется только
  `Freem.Entities.UseCases.IntegrationTests` (`UseCasePlainExecutor`). В тестах use case'ов ~107 `[Fact]`, из них
  27 проверяют `UnauthorizedException`. Unit-тестов use case'ов нет.
- API: 32 контроллера вызывают `IUseCaseExecutor<UseCaseExecutionContext>`, контекст строит
  `UseCaseContextProvider` из claim `sub`. Ошибки маппятся `switch` по `ErrorCode` в коды без тела (404, 422, 400,
  500 и т. д.); DTO `ErrorResponse` в `Api.Public.Contracts` есть, но нигде не используется. Текущие коды: сущность
  запроса не найдена — 404 (`StopRunningRecord` без запущенной записи — тоже 404); связанные активности/теги не
  найдены — 422; `ActivityInvalidStatus`, дубликат имени тега, `EndAtToEarly` — 422; `NothingToUpdate`/`NothingToDo` —
  400; пользователь не найден — 401, но в `UpdateSettingsController` — 403 (`ForbidResult`); повторная регистрация —
  409; остальное — 500. Auth-контроллеры
  (`login`/`refresh`/`logout`/`register`) use case'ы не вызывают, работают с клиентом EgoHub напрямую.

**Freem, хранилище:**

- Репозитории (`Freem.Domain.Storage`, один интерфейс на агрегат) возвращают `Task`, `SearchEntityResult<T>`
  (`Founded`/`Entity`) и `SearchEntitiesAsyncResult<T>` (`IAsyncEnumerable<T>` + `TotalCount`); ошибки —
  исключения `NotFoundException(Id)`, `NotFoundRelatedException(Id, params Id[])`, `DuplicateKeyStorageException(enum)`,
  `InternalStorageException`.
- Транзакции: `Freem.Storage.Abstractions` (`IStorageTransaction`, фабрика, `StorageTransactionRunner`) и
  `Freem.Storage.EFCore` (транзакция общего scoped `DbContext`, поэтому репозиториям её передавать не нужно).
  `StorageTransactionRunner` откатывает только по исключению: при переходе на `Result` ошибка внутри лямбды
  закоммитится. Код `Utilities.Storage.Transactions` 0.0.1 совпадает с `Freem.Storage.Abstractions` (тот же
  исключенческий раннер), `.EFCore` 0.0.2 ссылается на EF Core `9.0.*`.

**Пакеты** (исходники в `~/Sources/CSharpUtilities`, теги версий есть):

- `Utilities.Primitives` 0.0.3: `Result<TError>`, `Result<TValue, TError>` (неявные преобразования из значения и
  ошибки), `AsyncBatch<T>` (`IAsyncEnumerable<T>` + `Count`), `Option`.
- `Utilities.UseCases` 0.1.0: `IRequest` (обязательное свойство `RequestType Type`), `IHandlerContext`
  (`RequestId`), `IRequestHandler<TContext, TRequest, TResponse>`, `IRequestMediator<TContext>`,
  `AddRequestMediator<TContext>()` (scoped, декораторы). Handlers регистрируются вручную как
  `IRequestHandler<…>`; реестра типов нет.
- `Utilities.Errors` 0.0.1: `Error(ErrorType, ErrorMessage, ErrorProperties?, ErrorDetails?)` с `Builder`,
  `ErrorType` вида `scope:code`. В `source/packages/` его нет (есть в `docker/out/packages/`); Primitives,
  UseCases и `Utilities.Http.Errors` 0.0.2 там уже лежат, но ни один `.csproj` на них не ссылается.

**StashIt** (`Core/`):

- Цепочка: `Domain.Storage` (`RepositoryContext` с необязательной `IStorageTransaction`; методы принимают его и
  возвращают `Result<Error>`/`Result<T, Error>`/`Result<AsyncBatch<T>, Error>`) → `Domain.UseCases(.Contracts)`
  (`IRequestHandler<HandlerContext, XRequest, Result<XResponse, Error>>`, `HandlerContext(UserId, RequestId,
  RepositoryContext)` с ненулевым `UserId`, каталог `RequestTypes`, ответы — простые классы, удаление возвращает
  `Result<Error>`) → `Api.Public.UseCases(.Contracts)` (`ApiXRequest`, handler вызывает доменный медиатор и маппит
  `Result` в `IActionResult`; `CommonResponseMapper` выбирает HTTP-код по `ErrorType` и кладёт ошибку в тело
  `ErrorResponse`) → контроллер (одна строка: `Mediator.ExecuteAsync<ApiXRequest, IActionResult>`).
- Ошибки — `Utilities.Errors.Error` из `DomainErrorCatalog` (`Types`, `Properties`, `Factories`:
  `CreateTagNotFound(id)` и т. п.). Транзакции — собственный `Stashit.Storage.Transactions` с результатным раннером
  (ошибка `Result` откатывает транзакцию) и `StorageErrorCatalog`.
- Trandoma (PostgreSQL/EF, как Freem): та же схема `Api.Public.UseCases` → `Domain.UseCases`, но без
  `RepositoryContext`; репозитории возвращают `Result<T, RepositoryError>`.

## Один-два иллюстративных примера (не исчерпывающий список!)

- `CreateRecordUseCase` ловит `NotFoundRelatedException` и через `ex.RelatedIds.HasActivityIds()`/`HasTagIds()`
  выбирает `CreateRecordErrorCode`; контроллер превращает любой из кодов в 422 без тела. После перехода репозиторий
  возвращает ошибку из каталога (с Id в свойствах), handler возвращает её дальше, маппер выбирает код.
- `CreateTagController`: 30 строк маппинга ответа и `switch` по `CreateTagErrorCode`; в StashIt аналог — одна строка
  в контроллере и `Map()` в API handler.

## Направление решения (не пошаговая реализация)

Решения пользователя (2026-10-19):

1. **Граница этапов — вариант (а).** На этапе 5 — доменный слой (use case'ы, репозитории, ошибки, транзакции);
   контроллеры адаптируются минимально: маппинг `Error` → HTTP-код остаётся в контроллере. `Api.Public.UseCases` и
   тонкие контроллеры — этап 6.
2. **Ошибки — `Utilities.Errors.Error` и `DomainErrorCatalog`**, как в StashIt.
3. **Без `RepositoryContext`:** репозитории возвращают `Result`/`AsyncBatch`, как в Trandoma.
4. **Транзакции:** `Freem.Storage.*` остаётся, раннер становится результатным по образцу `Stashit.Storage.Transactions`.
5. **Вложенные вызовы исправляем:** ошибка вложенного use case'а пробрасывается и откатывает транзакцию.
6. **`Plain` удаляется,** нужные async-хелперы переносятся в `Freem.Entities.UseCases.IntegrationTests`, sync-обёртки
   удаляются.
7. **Имена проектов не меняются** до этапа 7.

Итоговое направление:

- `Freem.UseCases`, `.Abstractions`, `.Contracts.Abstractions`, `.Types`, `Freem.Results`,
  `Freem.Entities.UseCases.Plain` удаляются; use case'ы становятся `IRequestHandler<HandlerContext, XRequest,
  Result<XResponse, Error>>` (или `Result<Error>`), запросы — `IRequest` с каталогом `RequestTypes`, вызов через
  `IRequestMediator<HandlerContext>`.
- `HandlerContext` с ненулевым `UserId` и `RequestId`; проверка авторизации уходит из use case'ов в API (нет
  пользователя — 401 до вызова медиатора); тесты на `UnauthorizedException` удаляются.
- Ошибки — `Utilities.Errors.Error` и `DomainErrorCatalog` в `Freem.Domain`; enum'ы `*ErrorCode` удаляются.
  Чтобы сохранить нынешние коды, в каталоге различаются «сущность не найдена» и «связанная сущность не найдена»
  (Id — в свойствах ошибки).
- Репозитории возвращают `Result<…, Error>`/`AsyncBatch<T>` вместо исключений и `SearchEntity*Result`.
- Раннер транзакций становится результатным (ошибка в `Result` откатывает транзакцию).
- Ошибки вложенных вызовов (`Start` → `Stop`, `Stop` → `CreateRecord`) пробрасываются и откатывают транзакцию.
- Внешнее поведение API (маршруты, коды ответов, тела успешных ответов) не меняется. Исключение — запрос без
  разбираемого claim `sub`: сейчас `UnauthorizedException` без обработчика (500), после — 401 до вызова медиатора.

## Явно не входит в этот файл (пока)

`Api.Public.UseCases`, тонкие контроллеры и клиенты (этап 6); формат тела ошибок API (`ErrorResponse`) — меняет
контракт для фронтенда и клиентов, вместе с этапом 6; переименование проектов (этап 7). Известные долги не
исправляются: `DayUtcOffset` без сравнения по значению, 500 на поиск тега по имени из пробелов, чтение пустых имён
тегов, `MonthOnly` без `IParsable`.

## Критерий готовности

- В решении нет `Freem.Results`, `Freem.UseCases*`, `Freem.Entities.UseCases.Plain`; use case'ы работают через
  `Utilities.UseCases`, ошибки — через `Result<…>`; репозитории не бросают `NotFound*`/`DuplicateKey*` исключений.
- `dotnet build Freem.sln` — 0 предупреждений, 0 ошибок; unit-тесты зелёные.
- `docker/test` зелёный у пользователя (тесты хранилища, use case'ов и функциональные тесты API).
- Внешнее поведение API не изменилось: маршруты, коды ответов и тела успешных ответов те же (функциональные тесты
  API и сверка JSON, если затронуты DTO).

## План реализации

После каждого шага решение собирается (`dotnet build Freem.sln`, 0 предупреждений) и проходят unit-тесты; тесты,
затронутые шагом, правятся в нём же, чтобы решение не оставалось несобранным.

1. **Пакеты и каталог ошибок.** Скопировать `Utilities.Errors` 0.0.1 в `packages/` и дописать в список пакетов в
   `CLAUDE.md` `Primitives`, `UseCases`, `Errors`. Добавить `DomainErrorCatalog` в `Freem.Domain` (`Types`,
   `Properties`, `Factories`) с типами, которых хватает для нынешних кодов: сущность не найдена, связанная сущность
   не найдена, нечего делать, недопустимый статус, уже существует, недопустимый период, внутренняя ошибка.
2. **Раннер транзакций.** `StorageTransactionRunner` в `Freem.Storage.Abstractions` получает перегрузки с
   `Result<Error>`/`Result<T, Error>`: ошибка в результате откатывает транзакцию, как в `Stashit.Storage.Transactions`.
   Исключения по-прежнему откатывают и пробрасываются.
3. **Репозитории.** Интерфейсы `Freem.Domain.Storage` возвращают `Result<Error>`, `Result<T, Error>`,
   `Result<AsyncBatch<T>, Error>`; `NotFound*`/`DuplicateKeyStorageException` и `SearchEntity*Result` удаляются
   (`InternalStorageException` — по итогам чтения реализации). Реализации в `Storage.PostgreSQL` переводят нарушения
   ключей в ошибки каталога. В тех же коммитах шага: тесты хранилища на `Result`; в нынешних use case'ах
   `try/catch` заменяется проверкой результата с прежними `*ErrorCode`. Это механическая промежуточная правка,
   чтобы решение собиралось до шага 4.
4. **Use case'ы на `Utilities.UseCases`.** `HandlerContext(UserId, RequestId)`, каталог `RequestTypes`, запросы —
   `IRequest`, use case'ы — `IRequestHandler<HandlerContext, XRequest, Result<XResponse, Error>>` (`Result<Error>`,
   если ответу нечего вернуть); ответы — простые классы без `Success`/`Error`, enum'ы `*ErrorCode` удаляются.
   Регистрация — `AddRequestMediator<HandlerContext>()` и явные `IRequestHandler<…>`. Вложенные вызовы — через
   `IRequestMediator<HandlerContext>`, ошибка пробрасывается и откатывает транзакцию. `UnauthorizedException` и
   `ThrowsIfUnauthorized` удаляются. В том же шаге: удаление `Freem.UseCases*` (4 проекта), `Freem.Results`,
   `Freem.Entities.UseCases.Plain`; async-хелперы — в `Freem.Entities.UseCases.IntegrationTests`; тесты use case'ов —
   на медиатор и `Result`, 27 тестов на `UnauthorizedException` удаляются; регрессионные тесты на вложенные вызовы
   (если сценарий ошибки воспроизводится через публичные use case'ы; иначе сообщу). Контроллеры адаптируются тут же
   (без этого решение не собирается): `IRequestMediator<HandlerContext>`, контекст из `UseCaseContextProvider`, при
   отсутствии пользователя — 401, маппинг `ErrorType` → HTTP-код с теми же кодами, что сейчас, по каждому
   контроллеру отдельно.
5. **Чистка и сверка.** Лишние `using` (IDE0005) и их сортировка, BOM относительно HEAD, grep строковых ссылок на
   удалённые сборки и типы, явные `PackageReference` вместо транзитивных из удалённых проектов, бит
   skip-worktree у `appsettings.Development.json`. `CLAUDE.md`: число проектов (34 → 28) и строки "Repository Layout"
   про `Results`/`UseCases`/`Plain`. Если менялись DTO API — сверка JSON круговой сериализацией с HEAD.
6. **Проверка пользователем:** `docker/test`.

## Статус и изменения

- 2026-10-20 (Claude Code, claude-opus-5-5): план и критерий готовности согласованы пользователем (в том числе 401
  вместо 500 без claim `sub`); шаги 1–5 выполнены. `Freem.Results`, `Freem.UseCases`, `.Abstractions`,
  `.Contracts.Abstractions`, `.Types`, `Freem.Entities.UseCases.Plain` удалены (`.csproj` — 28, было 34); use case'ы —
  `IRequestHandler<HandlerContext, …, Result<…, Error>>` (`Utilities.UseCases` 0.1.0), ошибки — `DomainErrorCatalog`
  (`Utilities.Errors` 0.0.1 скопирован в `packages/`), репозитории возвращают `Result`/`AsyncBatch`. `dotnet build
  Freem.sln` — 0 предупреждений, 0 ошибок; unit-тесты зелёные (5 проектов, 316). Коды ответов API сверены по всем 32
  контроллерам с прежним `switch`; контракты API, клиенты, миграции и фронтенд не менялись, поэтому сверка JSON не
  нужна. `docker/test` не запускался, ждёт пользователя; не закоммичено, изменения проиндексированы (нужно для
  `git mv`). Расхождения с планом и решения по ходу:
  - шаг 2: раннер не сбрасывал `_transaction` после завершения, и второй `RunAsync` в том же scope (например, `Start`
    после вложенного `Stop`) шёл без транзакции на освобождённом объекте; исправлено сбросом в `finally`;
  - каталог: в `Properties`/`Factories` добавлен `EventId` (его нет в `EntityIdNameProvider`); связанные сущности —
    одна ошибка `related_entities_not_found` с деталями `entity_not_found` на каждый Id, поэтому тесты use case'ов
    больше не различают «не найдены активности» и «не найдены теги» (Id проверяют тесты хранилища);
  - хранилище: `InternalStorageException` (нераспознанная ошибка PostgreSQL) и `InvalidRelatedCountException`
    (нарушение числа связей) остаются исключениями; конвертеры ошибок БД переименованы `*ToExceptionConverter` →
    `*ToErrorConverter` (`git mv`, вместе с тестом); `UsersRepository.DeleteAsync` по-прежнему всегда успешен;
    `EventProducer.PublishAsync` возвращает `Result<Error>`;
  - удалены ставшие неиспользуемыми `HasTagIds`/`HasActivityIds` (`Freem.Domain`) и мок `SampleId` (unit-тесты
    хранилища);
  - Contracts: каталог `RequestTypes`, у запросов `IRequest`; 12 ответов без данных удалены (handler возвращает
    `Result<Error>`), остальные — простые классы с теми же именами свойств; все 32 enum'а `*ErrorCode` удалены;
    лишняя ссылка Contracts на `Freem.Domain.Storage` убрана. Имена классов use case'ов (`*UseCase`) не менялись;
  - `UseCaseContextProvider.Get()` → `TryGet(out HandlerContext?)`, контроллер без пользователя отвечает 401;
  - тестов на `UnauthorizedException` оказалось 27, а не 7 (в исследовании посчитано по обрезанному выводу; текст
    исправлен), все удалены. Подготовка данных в тестах use case'ов перенесена из конструкторов в
    `IAsyncLifetime.InitializeAsync` (`UseCaseTestBase`), хелперы — `Fixtures/Executors` поверх медиатора;
  - вложенные вызовы: регрессионный тест `RunningRecordsStartUseCaseTests.ShouldFailure_WhenAnotherRunningRecordStartedLater`
    (старт раньше текущей записи: ошибка `period_is_invalid` вложенного `Stop`, текущая запись не меняется). Для
    `Stop` → `CreateRecord` сценария ошибки через публичные use case'ы не нашлось: связи удаляются каскадно, а запись
    без активностей падает исключением ещё при чтении; тест не добавлен. Следствие исправления: ошибка `CreateRecord`
    внутри `Stop` теперь даёт 500 и откат вместо 200 с потерей записи;
  - тесты хранилища: `*_ShouldThrowException_*` → `*_ShouldReturnError_*`, проверки ошибок — через `ErrorAssert`,
    успешные записи проверяют `Success`;
  - в `CLAUDE.md` — список пакетов, число проектов и строки "Repository Layout";
  - находка вне этапа: `RunningRecordsRepositoryTests.CreateAsync_..._WhenTagNotExists` на деле проверял
    отсутствующую активность, как соседний тест; по решению пользователя исправлен (активность есть в БД, тега нет,
    ожидается ошибка с Id тега) и переименован в `CreateAsync_ShouldReturnError_WhenRelatedTagNotExists`.

  `docker/test` зелёный у пользователя (до исправления теста); повторный прогон после исправления ждёт пользователя.
- 2026-10-20 (Claude Code, claude-opus-5-5): этап закрыт пользователем (независимая проверка не проводилась); реализация закоммичена (`8a72f7a`).
