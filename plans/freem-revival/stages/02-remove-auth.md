# Этап: Удаление собственной авторизации

Автор: Claude Code, claude-sonnet-5-5, 2026-10-18

См. общий контекст и место этого этапа: `plans/freem-revival/general/roadmap.md` (этап 2).

## Зачем именно так

Идентификация в итоге переезжает на EgoHub (этап 6). Пока EgoHub не подключён, собственные пароли, JWT и
блэклист токенов — мёртвый груз: их нужно поддерживать при каждом обновлении зависимостей и они тянут за
собой Redis-блэклист, Crypto и конфигурацию секретов. Удаляем их сразу и целиком, а `userId` временно берём из
query в единственной точке создания контекста use case'ов, чтобы не трогать 30+ контроллеров.

## Зафиксированные факты

Проверено 2026-10-18 чтением кода (сборка и тесты в рамках исследования не запускались).

Серверная часть:

- Контекст use case'ов создаётся только в `Freem.Web.Api.Public/Services/Implementations/UseCaseContextProvider.cs`:
  читает claim `JwtAuthenticationClaimTypes.UserId` из `HttpContext.User`, иначе `UseCaseExecutionContext.Empty`.
  Его вызывают все контроллеры V1; на них висит `[Authorize(JwtAuthorizationPolicy.Name)]` (кроме регистрации,
  логина и refresh).
- Проекты авторизации: `Freem.Credentials.Password` (хеширование паролей), `Freem.Entities.Tokens.JWT`,
  `Freem.Tokens`, `Freem.Tokens.Blacklist.Redis`; `Freem.Crypto.Hashes` используется ими и ссылается на него
  `Freem.Entities`, `UseCases`, `Storage.PostgreSQL`, `Web.Api.Public`, `UseCases.IntegrationTests`
  (нужно проверить, остаётся ли что-то, кроме паролей).
- В хосте (`Program.cs`): `AddCryptoHashes`, `AddPasswordRawHahser`, `AddGuidSaltGenerator`,
  `AddStaticCurrentPasswordHashAlgorithmGetter`, `AddRedisTokensBlacklist`, `AddStaticSecurityKeyGetter`,
  `AddAccessTokens`, `AddRefreshTokens`, `AddAuthentication`/`AddJwtBearerAuthentication`/
  `AddJwtCookieAuthentication`, `AddAuthorization(AddJwtPolicy)`, OpenApi-трансформеры JWT,
  `UseAuthentication`/`UseAuthorization`. Каталоги `Authentication/` и `Autherization/` в
  `Freem.Web.Api.Public`. Секции `appsettings.json`: `Redis.TokensBlackListKey`, `PasswordCredentials`, `Tokens`.
- Use case'ы пользователей: `Users/Password/{LoginUserPassword,RegisterUserPassword,UpdateLoginCredentials}`,
  `Users/Tokens/RefreshUserAccessToken`, контракты в `UseCases.Contracts/Users/{Password,Tokens}`,
  исполнители в `UseCases.Plain` (`UsersPasswordPlainExecutor`, `UsersTokensPlainExecutor` + Sync). Остаются
  `GetUser` (`/api/v1/user/me`) и `Users/Settings/*`.
- Регистрация сейчас — `RegisterUserPasswordUseCase`: принимает `Nickname`, `Login`, `Password`, создаёт `User` с
  `PasswordCredentials` и публикует два события (`Registered`, `PasswordCredentialsChanged`).
- Поле логина хранится **только** в таблице паролей (`UserPasswordCredentialsEntity.Login`, уникальный
  constraint `LoginUnique`; `IUsersRepository.FindByLoginAsync`, `DuplicateKeyStorageException.DuplicateUserLogin`).
  У `User` есть только `Nickname`.
- События пользователя: `UserEventActions` содержит `SignedIn`, `LoginCredentialsChanged` (значение
  `password_credentials_changed`), `TelegramIntegrationChanged`; список `All` используется в check constraint
  таблицы событий (`EventEntityTypeConfiguration.cs:28`) и в `EventsFactory`. Удаление значений из `All` меняет
  модель EF → нужна миграция сверх «удаления сущности паролей».
- `User.TelegramIntegration`/`UserTelegramIntegrationEntity` — отдельная сущность, паролей не касается.
- Клиенты и тесты: `Freem.Web.Api.Public.Client` (`UsersClient`, `TokenLoader`, `BaseClient`),
  `SyncClient` (`UsersSyncClient`, `BaseSyncClient`), `Contracts` (`DTO/Users/{LoginPassword,Tokens}`,
  `CookieNames`); `FunctionalTests` (`UserPreparer`, `Tests/V1/Users/{PasswordCredentials,Tokens}`, `TestContext`);
  `UseCases.IntegrationTests` (`UsersPassword*`, `UserTokensRefresh*`, `TestContext`);
  `Storage.PostgreSQL.IntegrationTests` (`DatabaseEntitiesFactory`).

Фронтенд (`Freem.Web/src`):

- Авторизация: `contexts/authorization/*` (`AuthorizationProvider` вызывает `/user/me` и `logout`),
  `clients/AuthorizationClient.ts`, `clients/http/AuthorizedHttpClient.ts`, `clients/models/users/{LoginPassword,Tokens}`,
  `UserTokens.ts`, страницы `LoginModel`, `RegisterModel`, `RemindPasswordModel`, компоненты `LoginEditor`,
  `RegisterEditor`, `AuthorizationPanel`, `Header`, роутинг `PrivateRoute`/`PublicRoute`.
- `userId` фронтенду сейчас приходит из `/user/me` после логина. По решению пользователя (2026-10-18) источник
  `userId` без логина на этом этапе не проектируется: фронтенд может быть нерабочим до конца работ по roadmap.

## Один-два иллюстративных примера (не исчерпывающий список!)

- `UseCaseContextProvider.Get()` — единственное место, которое надо переписать на чтение `userId` из query.
- `UserEventActions.All` → check constraint событий: пример того, как «чисто кодовое» удаление задевает схему БД.

## Направление решения (не пошаговая реализация)

- Удалить проекты `Freem.Credentials.Password`, `Freem.Entities.Tokens.JWT`, `Freem.Tokens`,
  `Freem.Tokens.Blacklist.Redis` (и `Freem.Crypto.Hashes`, если он больше никому не нужен), use case'ы/контракты/
  исполнители/контроллеры/DTO/клиенты/тесты логина, обновления пароля и refresh, а также соответствующий код хоста,
  конфигурацию и фронтенд.
- `UseCaseContextProvider` читает `userId` из query (имя параметра выбрать при реализации); на контроллерах
  убрать `[Authorize]`. Временная схема не публикуется наружу (не в OpenApi-документ как постоянный контракт и не
  в боевой деплой).
- Регистрация остаётся, но без пароля и без логина (принимает `Nickname`); `MeController` и настройки остаются.
- Изменения БД, согласованные пользователем (2026-10-18): (1) удаление таблицы учётных данных пароля;
  (2) удаление событий `SignedIn` и `LoginCredentialsChanged` вместе с их значениями в `UserEventActions.All` —
  это меняет check constraint таблицы событий. Обе правки делаются миграциями (одной или двумя — решить при
  реализации); любое иное изменение схемы согласуется отдельно. Данные одноразовые, поэтому строки событий с
  удаляемыми действиями можно не сохранять, но миграция не должна падать на них — проверить при реализации.

### Принятые решения (пользователь, 2026-10-18)

1. События `SignedIn` и `LoginCredentialsChanged` удаляются целиком (классы, константы, `EventsFactory`, тесты),
   с миграцией check constraint.
2. Логин убирается целиком (`FindByLoginAsync`, `DuplicateUserLogin`, тип `Login`, `LoginJsonConverter`);
   регистрация принимает только `Nickname`.
3. Код авторизации во фронтенде (`Freem.Web`) удаляется в этом этапе; работоспособность фронтенда не требуется,
   `npm run build` проходит.
4. `Freem.Crypto.Hashes` удаляется, если после удаления паролей им никто не пользуется.
5. Redis остаётся только для блокировок; секция `Redis.TokensBlackListKey` и Redis-блэклист уходят.

## Явно не входит в этот файл (пока)

План реализации; точное имя query-параметра; перечень затронутых файлов; поведение FunctionalTests без логина
(как они получают пользователя — решается при реализации).

## Критерий готовности

- `dotnet build Freem.sln --no-incremental` — 0 ошибок и 0 предупреждений; unit-тесты проходят
  (`dotnet test Freem.sln` без `DOTNET_ROLL_FORWARD`).
- В коде нет ссылок на пароли, JWT, токены и их блэклист (проверка поиском по репозиторию); удалённые проекты
  убраны из `Freem.sln`, `Dockerfile` и `CLAUDE.md` обновлены, если упоминали их.
- `dotnet ef migrations has-pending-model-changes` чистый после новых миграций, которые удаляют таблицу паролей
  и обновляют check constraint событий; никаких других изменений схемы.
- Запросы к API с `?userId=...` работают без заголовков/cookie авторизации; регистрация создаёт пользователя по
  `Nickname`; `/user/me` и настройки возвращают данные для переданного `userId` (проверяется вручную при
  согласии пользователя или функциональными тестами, если поднят API).
- Фронтенд: работоспособность не требуется (решение пользователя); код авторизации удалён, `npm run build`
  проходит.

## План реализации

Каждый шаг заканчивается проверкой и показом результата; коммиты только по просьбе пользователя. Интеграционные
и функциональные тесты запускать не нужно (критерий — сборка и unit-тесты), но они должны компилироваться.

1. **Контекст и хост.** `UseCaseContextProvider` читает `userId` из query; убрать `[Authorize]` с контроллеров,
   аутентификацию/авторизацию и JWT-трансформеры OpenApi из `Program.cs`, каталоги `Authentication/` и
   `Autherization/`; удалить контроллеры логина, обновления пароля и refresh (и их DTO в `Contracts`).
   Проверка: `dotnet build Freem.Web.Api.Public`.
2. **Use case'ы и домен пользователей.** Удалить use case'ы, контракты и исполнители (Async/Sync) логина,
   обновления учётных данных и refresh; регистрация — только по `Nickname` (публикует только `Registered`);
   убрать `FindByLoginAsync`, `DuplicateUserLogin`, типы `Login`/`Password`/`PasswordHash`, их JSON-конвертеры,
   `UserPasswordCredentials`, `UserCredentials.Password`, `UserTokens`, `User.PasswordCredentials` и событие
   смены учётных данных. Проверка: сборка решения (ошибки в клиентах и тестах допустимы до шагов 5–6).
3. **События.** Удалить `SignedIn` и `LoginCredentialsChanged`: классы, константы, `UserEventActions.All`,
   `EventsFactory`, затронутые unit- и интеграционные тесты. Проверка: unit-тесты `Freem.Entities.UnitTests`.
4. **Хранилище и миграции.** Удалить `UserPasswordCredentialsEntity`, конфигурацию, имена, маппинг; сгенерировать
   миграцию(и) на удаление таблицы и обновление check constraint событий. Проверка:
   `dotnet ef migrations has-pending-model-changes` чистый, unit-тесты `Storage.PostgreSQL*` проходят; в
   миграции показать пользователю итоговый SQL до продолжения.
5. **Проекты и конфигурация.** Удалить из решения и ссылок `Freem.Credentials.Password`,
   `Freem.Entities.Tokens.JWT`, `Freem.Tokens`, `Freem.Tokens.Blacklist.Redis`, `Freem.Crypto.Hashes` (после
   проверки, что его больше никто не использует); убрать секции `PasswordCredentials`, `Tokens`,
   `Redis.TokensBlackListKey` из `appsettings*.json` и конфигурационного кода; обновить `Dockerfile` и
   `CLAUDE.md`, если они их упоминали. Проверка: `dotnet build Freem.sln --no-incremental` без ошибок и
   предупреждений.
6. **Клиенты и тесты.** `Client`/`SyncClient`: убрать логин и `TokenLoader`, передавать `userId` в query;
   `FunctionalTests`, `UseCases.IntegrationTests`, `Storage.PostgreSQL.IntegrationTests`: удалить тесты
   паролей/токенов, перевести подготовку пользователя на регистрацию по `Nickname` и `userId` в query.
   Проверка: сборка решения, `dotnet test` по unit-проектам.
7. **Фронтенд.** Удалить код авторизации: страницы логина/регистрации/напоминания пароля, редакторы, клиенты и
   модели, `AuthorizationProvider`, `PrivateRoute`/`PublicRoute`, роутинг и вызовы. Приложение может не
   работать. Проверка: `npm run build` без ошибок, `npm run lint` без новых ошибок.
8. **Итог.** Прогнать критерий готовности целиком, поиском по репозиторию убедиться в отсутствии паролей/JWT/
   токенов/блэклиста, показать результат, дописать статусы и остановиться.

## Статус и изменения

- 2026-10-18 (Claude Code, claude-sonnet-5-5): исследование записано (по чтению кода), ожидает согласования направления, критерия готовности и ответов на 5 вопросов (вопрос про источник `userId` во фронтенде снят пользователем, добавлен вопрос про удаление фронтенд-кода авторизации); реализация не начата.
- 2026-10-18 (Claude Code, claude-sonnet-5-5): пользователь ответил на вопросы (события удаляются целиком с миграцией check constraint; логин, Crypto.Hashes, фронтенд-код авторизации удаляются; Redis только для блокировок); решения записаны, план реализации написан и ожидает одобрения, реализация не начата.
- 2026-10-18 (Claude Code, claude-sonnet-5-5): пользователь одобрил план реализации; начата реализация.
- 2026-10-18 (Claude Code, claude-sonnet-5-5): шаги 1–3 и код шага 4 выполнены, шаг 5 выполнен досрочно в части проектов. Отклонения от порядка плана: (1) удаление проектов `Freem.Credentials.Password`, `Freem.Crypto.Hashes`, `Freem.Tokens`, `Freem.Tokens.Blacklist.Redis`, `Freem.Entities.Tokens.JWT` и секций `PasswordCredentials`/`Tokens`/`Redis.TokensBlackListKey` сделано до генерации миграции — `dotnet ef` не запускался со ссылкой на удалённый код; (2) регистрация переименована: `RegisterUser*` (контракты, use case, `UsersPlainExecutor`/`UsersPlainSyncExecutor`, контроллер `POST api/v1/user/register` вместо `.../password-credentials/register`); (3) удалены также неиспользуемый `UserCredentials` и тег `ControllerTags.PasswordCredentials/AuthTokens`; (4) в `UserTelegramIntegrationEntity.Equals(object)` исправлена ошибочная проверка типа (`UserPasswordCredentialsEntity` → `UserTelegramIntegrationEntity`), иначе не собиралось. Миграция `20261017235202_RemovePasswordCredentialsAndSignInEvents` (удаление таблицы `user_password_credentials`, удаление строк событий `signed_in`/`password_credentials_changed`, обновление check constraint) ждёт просмотра SQL пользователем; `has-pending-model-changes` чистый, снимок модели заодно обновил `ProductVersion` 9.0.2 → 10.0.0. Не сделаны: шаг 6 (клиенты и тесты — решение не собирается из-за них), шаг 7 (фронтенд), шаг 8.
- 2026-10-18 (Claude Code, claude-sonnet-5-5): пользователь одобрил SQL миграции; шаги 6–8 выполнены, этап ожидает проверки и закрытия пользователем. Итог: `dotnet build Freem.sln --no-incremental` — 0 ошибок, 0 предупреждений; unit-тесты 328 из 328 (было 330: ушли два параметра `UserEventActionsTests` для удалённых событий); `dotnet ef migrations has-pending-model-changes` чистый; `npm run build` проходит, `npm run lint` — 0 ошибок (3 старых предупреждения этапа 7). Отклонения: (1) ответ регистрации теперь содержит `userId` (`RegisterUserResponse`) — иначе клиенты и функциональные тесты не узнают идентификатор нового пользователя (раньше его давал логин); (2) в клиентах `TokenLoader` заменён на `CurrentUser`, методы `SendAuthorized*` → `SendForUser*`, `userId` передаётся в query (имя параметра — `QueryParameterNames.UserId` в `Contracts`); (3) в `UseCases.IntegrationTests` убран Redis целиком (использовался только блэклистом токенов), из `configuration.json` убрана `RedisConnectionString`; (4) из фронтенда удалён также `Header` (показывал ник из `/user/me`); `CLAUDE.md` обновлён (убраны устаревшие строки про JWT/InPlace/Linq, добавлена заметка про временный `userId`). Не проверялось: запуск хоста и OpenAPI-документа, применение миграции к реальной БД (`database update` не запускался), интеграционные и функциональные тесты (нужна инфраструктура), работа фронтенда в браузере (по решению пользователя не требуется).
- 2026-10-18 (Claude Code, claude-sonnet-5-5): независимая проверка отдельным агентом: APPROVE WITH NOTES (блокирующих нет). Неблокирующие замечания: шум в диффе (форматирование `appsettings.Development.json`, перевод строки в двух файлах `FunctionalTests`), остатки `credentials: "include"`/`.AllowCredentials()` под cookie-авторизацию, секреты в `appsettings.Development.json` и `FunctionalTests/configuration.json` не скрыты `skip-worktree` (этап 5), не проверено поведение живого хоста, расхождение 5 и 3 предупреждений lint. Пользователь счёл замечания незначительными, правок не потребовал, реализацию одобрил и попросил закоммитить; записи о закрытии этапа пользователь не давал.
