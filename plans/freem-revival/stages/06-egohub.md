# Этап: Интеграция с EgoHub

Автор: Claude Code, claude-sonnet-5-5, 2026-10-18

См. общий контекст и место этого этапа: `plans/freem-revival/general/roadmap.md` (этап 6).

## Зачем именно так

Временная схема (`userId` из query в `UseCaseContextProvider`) не должна жить дальше: любой клиент может
выдать себя за любого пользователя. Эталон интеграции — Trandoma (тот же стек, net10, EgoHub, Vite): API
валидирует JWT, выданный EgoHub, а идентичность берёт из claim `sub`. Это даёт минимум собственного кода
авторизации во Freem (ровно то, что было удалено на этапе 2) и единый вход для всех проектов.

## Зафиксированные факты

Проверено чтением кода Freem, Trandoma и EgoHub.

- Freem: `UseCaseContextProvider` берёт `userId` из query (`QueryParameterNames.UserId`) и строит
  `UseCaseExecutionContext`; все контроллеры получают контекст только через него. `Program.cs` не содержит
  `AddAuthentication`/`UseAuthentication`. CORS уже с `AllowCredentials()`.
- Идентификатор пользователя Freem — `UserIdentifier : StringIdentifier`, генерируется
  `GuidStringIdentifierGenerator` в `RegisterUserUseCase`; в БД `UserEntity.Id` — строка. `UserEntity`
  содержит `Nickname`, `Settings`, `TelegramIntegration`; поля внешней идентичности нет.
- `RegisterUserController` (`POST api/v1/user/register`, тело — `Nickname`) создаёт пользователя с новым
  сгенерированным id и возвращает его. `MeController` (`GET api/v1/user/me`) возвращает `userId` и
  `nickname`, при отсутствии пользователя отдаёт 401.
- EgoHub: `UserId` — строковый идентификатор (`BaseEntityId<UserId>`, `Generate()`), реализует `IParsable`.
  Публичный API: `identities/password/{register,login}`, `identities/telegram`, `sessions/tokens/{refresh,revoke}`,
  `users/{userId}`, `jwks`.
- Пакет `EgoHub.Authentication.AspNetCore` 0.0.1: схема `EgoHubBearerAuthenticationDefaults.SchemeName`,
  `AddEgoHubBearerAuthentication(options => options.OnTokenValidated = ...)`; подпись проверяется по JWKS
  EgoHub (кэш 15 минут), issuer/audience не проверяются. Нужен `IClock` из `Utilities.Clock`.
- Trandoma подключает его так: `OnTokenValidated` достаёт `sub`, парсит в свой `UserId` и добавляет claim;
  политика авторизации требует аутентифицированного пользователя; `EgoHub.Api.Public.Client` (0.0.10) с
  `ITokenCredentialLoader` и сервисным токеном нужен только там, где API вызывает EgoHub сам (регистрация,
  логин, refresh через `CookieAuthController`: refresh-токен в HttpOnly-cookie, access-токен в теле ответа).
  Конфигурация: секция `EgoHubIntegration` с `Url` и `Token`.
- Пакеты есть в `~/Sources/EgoHub/docker/pack/out/packages/`: `EgoHub.Authentication.AspNetCore.0.0.1`,
  `EgoHub.Api.Public.Client.0.0.10`, `EgoHub.Api.Admin.Client.0.0.10`. Во Freem в `packages/` лежат
  `Utilities.{Clock 0.1.0, Collections 0.1.0, Http 0.0.8, Http.SystemNet 0.0.8}`; Trandoma использует
  `Utilities.Clock 0.0.2`, `Primitives`, `Errors` — совместимость с Freem (Clock 0.1.0) и транзитивные
  зависимости пакетов EgoHub не проверялись.
- Фронтенд Freem: код авторизации удалён на этапе 2; `UserClient.me()` вызывает `api/v1/user/me`;
  HTTP-клиент (`clients/http`) токен не подставляет. У Trandoma.Web есть образец: `AuthorizationProvider`,
  `AccessTokenStore`, `AuthClient`.

## Один-два иллюстративных примера (не исчерпывающий список!)

- Серверная сторона: `Trandoma.Api.Public/Program.cs` (блок `AddEgoHubBearerAuthentication`) и
  `Authentication/EgoHub/EgoHubClaimsMapper.cs`.
- Клиентская сторона: `Trandoma.Web/src/authorization/AuthorizationProvider.tsx` и `clients/AccessTokenStore.ts`.

## Направление решения (не пошаговая реализация)

Предварительное, до ответов пользователя:

1. **Идентичность.** Id пользователя Freem = `sub` из токена EgoHub (строка, тип колонки не меняется, DB не
   трогаем). `UseCaseContextProvider` читает claim из `HttpContext.User` вместо query; query-схема удаляется
   вместе с `QueryParameterNames.UserId` и подстановкой в `BaseClient`.
2. **Регистрация.** Эндпоинт остаётся (решение пользователя), но создаёт пользователя Freem с id из токена и
   переданным nickname, а не с сгенерированным id. Нужна идемпотентность на повторный вызов.
3. **Вход.** Либо Freem проксирует логин/регистрацию/refresh в EgoHub (как `CookieAuthController` у
   Trandoma), либо фронтенд говорит с EgoHub напрямую. Склоняюсь к варианту Trandoma (прокси с cookie), т. к.
   сервисный токен EgoHub не должен попадать в браузер.
4. **Клиенты и тесты.** `Freem.Web.Api.Public.Client`/`SyncClient` вместо `CurrentUser`+query передают
   bearer-токен; функциональные тесты получают токен у EgoHub (или тестовой подмены схемы — решить).
5. **Фронтенд.** Логин/регистрация, хранение access-токена, `Authorization`-заголовок, обновление по cookie.
6. **Окружения.** Секция `EgoHubIntegration` в конфигах API, `docker/service`, `docker/test` (в `test`
   EgoHub, скорее всего, недоступен — нужен способ тестировать без него).

## Решения пользователя (2026-10-18)

1. Id пользователя Freem = `sub` EgoHub, как в Trandoma (там `sub` парсится прямо в доменный `UserId`); БД не меняется.
2. Вход — через Freem API по образцу Trandoma (`CookieAuthController`: refresh-токен в HttpOnly-cookie, access-токен
   в теле); EgoHub снаружи на проде недоступен.
3. Пользователь Freem создаётся явным вызовом эндпоинта регистрации (не лениво).
4. EgoHub в конфигурации — внешняя интеграция (`EgoHubIntegration:Url/Token`), как в `service` у StashIt и
   Trandoma. В `docker/test` у Trandoma адрес — `host.docker.internal:<порт>` запущенного вручную EgoHub, а
   тестовых пользователей тест регистрирует в нём сам (`TestUserFactory`: register + login через API проекта).
5. Фронтенд на этом этапе — только то, что нужно для новой авторизации (вход, регистрация, токен, выход);
   остальное — этап 7.

## Явно не входит в этот файл (пока)

Список затронутых файлов, точные версии пакетов (после проверки совместимости с `Utilities.*` Freem).

## Критерий готовности

- `dotnet build Freem.sln` и unit-тесты зелёные; `docker/test` проходит (формат проверки аутентификации — по
  ответу на вопрос 4).
- Запрос без валидного токена EgoHub получает 401; с валидным — выполняется от имени пользователя из `sub`;
  `userId` в query больше нигде не используется.
- Фронтенд: вход через EgoHub, работа с данными после входа, выход.
- Изменений схемы БД нет (иначе — остановка и согласование).

## План реализации

Ждёт одобрения. Критерий каждого шага — сборка (`dotnet build Freem.sln` / `npm run build`) и unit-тесты;
интеграционные и функциональные — на шаге 6 (нужен запущенный EgoHub; запускает пользователь).

1. **Пакеты.** Скопировать в `_Src/Freem/packages/` `EgoHub.Authentication.AspNetCore` 0.0.1 и
   `EgoHub.Api.Public.Client` 0.0.10 из `~/Sources/EgoHub/docker/pack/out/packages/` (+ недостающие
   `Utilities.*`: Primitives, Errors и т. п. из `~/Sources/Trandoma/packages/`); добавить ссылки в
   `Freem.Web.Api.Public`; проверить restore/build и конфликты версий (в первую очередь `Utilities.Clock`
   0.1.0). Если нужна пересборка пакетов — остановиться и спросить.
2. **Аутентификация на сервере.** `AddEgoHubBearerAuthentication` с `OnTokenValidated` (claim `sub` → claim
   пользователя Freem, как `EgoHubClaimsMapper`), политика «аутентифицированный пользователь», `[Authorize]` на
   контроллерах через `BaseController`, `UseAuthentication`/`UseAuthorization` в `Program.cs`.
   `UseCaseContextProvider` читает id из claim; удалить схему с query (`QueryParameterNames.UserId`).
3. **Регистрация.** `RegisterUserUseCase` создаёт пользователя с id из контекста (не из генератора); повторный
   вызов — осмысленная ошибка/идемпотентность; убрать неиспользуемый генератор `UserIdentifier`, если он
   больше никому не нужен. Обновить контракт/контроллер/unit-тесты.
4. **Вход через Freem API.** Секция `EgoHubIntegration` (`Url`, `Token`) в конфигурации, регистрация
   `AddPublicClient`/`ITokenCredentialLoader`; контроллер `api/v1/auth/cookie/{register,login,refresh,logout}`
   по образцу Trandoma (прокси к `IPasswordIdentityClient`/`ISessionTokensClient`, cookie, CORS с credentials);
   контракты в `Freem.Web.Api.Public.Contracts`.
5. **Клиенты.** `Freem.Web.Api.Public.Client`/`SyncClient`: вместо `CurrentUser` + query — bearer-токен
   (провайдер токена по образцу `SessionAuthorizationTokenProvider`), клиент авторизации; убрать `CurrentUser`.
6. **Тесты и окружения.** `FunctionalTests`: фабрика тестового пользователя (register + login через API,
   затем регистрация в Freem), `UserPreparer`/`TestContext` на новую схему; конфиги `docker/test` и
   `docker/service` (`EgoHubIntegration`, `.env.example`, README окружений) — как у Trandoma. Прогон
   `docker/test` делает пользователь при запущенном EgoHub; результат фиксирую.
7. **Фронтенд.** Хранилище access-токена в памяти, `AuthClient` (register/login/refresh/logout с
   `credentials: "include"`), `AuthorizationProvider`, страницы входа/регистрации (после регистрации — вызов
   регистрации пользователя с nickname), заголовок `Authorization`, молчаливый refresh при 401 и при загрузке;
   удалить остатки `userId`. Проверка: `npm run build`, `npm run lint` без новых предупреждений.
8. **Документация.** `CLAUDE.md` (убрать оговорку про временную схему и `userId` из query), запись в статус.

## Статус и изменения

- 2026-10-18 (Claude Code, claude-sonnet-5-5): исследование записано, ожидает ответов на открытые вопросы и согласования направления; реализация не начата.
- 2026-10-18 (Claude Code, claude-sonnet-5-5): получены ответы пользователя на вопросы, записаны решения и план реализации, ожидает одобрения; реализация не начата.
- 2026-10-18 (Claude Code, claude-sonnet-5-5): план одобрен пользователем; шаги 1–8 выполнены, этап ожидает проверки и закрытия. `dotnet build Freem.sln` — 0 ошибок и предупреждений, unit-тесты зелёные, `npm run build` проходит, `npm run lint` — 3 прежних предупреждения. Не проверено: `docker/test` и функциональные тесты (нужен запущенный EgoHub и валидный токен в локальном конфиге), поведение живого хоста и фронтенда в браузере. Отклонения от плана: (1) вместо удаления `CurrentUser` он переименован в `ClientSession` и хранит access-токен; (2) `refresh`/`logout` — отдельные контроллеры без `BaseController` (у него `Consumes: json`, а у запросов нет тела), `logout` анонимный, т. к. учётные данные — cookie; (3) добавлен код ошибки `UserAlreadyRegistered` (409) для повторной регистрации; (4) удалён генератор `UserIdentifier`; (5) в `docker/test/configs/...FunctionalTests.configuration.json` порт EgoHub `10080` — заглушка, а `Token` пустой (подставляется локально, `skip-worktree`); (6) `.env` окружения `service` не менялся — нужно добавить `EGOHUB_INTEGRATION_URL/TOKEN` самому.
- 2026-10-18 (Claude Code, claude-sonnet-5-5): независимая проверка отдельным агентом: APPROVE WITH NOTES (блокирующих нет). Решения пользователя по замечаниям: исправлено — проброс статуса EgoHub (теперь клиенту отдаются только 400/401/409/422, остальное — 502), удаление refresh-cookie только при 4xx от EgoHub, лишний refresh при 401 от `me` у незарегистрированного пользователя (`skipRefresh`), хэши коммитов этапа 5 в записях о закрытии (`4947861..f77a3f2`); добавлено — примечание про `Secure`-cookie в `docker/service/README.md`; остаются как есть — падение хоста при пустом `EgoHubIntegration:Url/Token`, отсутствие проверки issuer/audience токена (ограничение пакета, как в Trandoma), отсутствие rate limit на `register`/`login`, непроверенное поведение остальных эндпоинтов для пользователя EgoHub без пользователя Freem; три записи статуса по этапу оставлены. После правок: `dotnet build` 0/0, `npm run build` проходит, lint — 3 прежних предупреждения; `docker/test` по-прежнему не запускался.
- 2026-10-18 (Claude Code, claude-sonnet-5-5): этап 6 закрыт пользователем; реализация закоммичена (`5ae9c23..919bf51`).
