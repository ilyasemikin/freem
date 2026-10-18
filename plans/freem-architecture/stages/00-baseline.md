# Этап: Базовая проверка

Автор: Claude Code, claude-sonnet-5-5, 2026-10-18

См. общий контекст и место этого этапа: `plans/freem-architecture/general/roadmap.md` (этап 0).

## Зачем именно так

Перед структурными изменениями нужна зелёная база: хост стартует, `docker/test` проходит целиком. Часть кода
после этапов 3–7 первого плана не запускалась (хост без брокера, функциональные тесты с EgoHub, тесты
`UpdateRecord*` из этапа 7). Если проверять позже, нельзя будет отличить регрессию архитектурных правок от старого
долга.

## Зафиксированные факты

Проверено 2026-10-18 на этой машине.

- EgoHub запущен локально из IDE (не в Docker): Public API — `127.0.0.1:25001`, Admin API — `127.0.0.1:25000`.
  Admin API без авторизации.
- Оба порта слушают только loopback. Контейнер `test` ходит в EgoHub через `host.docker.internal`
  (`172.17.0.1`, `docker0`), до сервиса на `127.0.0.1` он не дотянется.
- В EgoHub заведён сервис `freem` (раньше были `stashit-core`, `trandoma`) и выпущен сервисный токен
  `api-public-test-2`. Первый выпущенный токен (`api-public-test`) отозван: его значение не сохранилось.
- Адрес и токен EgoHub для `docker/test` не хранятся в `configs/` (там пустые значения): они берутся из
  `docker/test/.env` (шаблон `.env.example`, `.env` в `.gitignore`), `docker-compose.yaml` передаёт их контейнеру
  `test` переменными `WebApiApplication__EgoHubIntegration__{Url,Token}`, а `TestContext` читает конфигурацию с
  `AddEnvironmentVariables()`. Локальный `.env` заполнен: `http://host.docker.internal:25001` и токен.
- Окружение `develop` сейчас не запущено (порты 10150, 10151 не слушаются). Docker-сокет из сессии агента
  недоступен.

## Один-два иллюстративных примера (не исчерпывающий список!)

- Функциональные тесты регистрируют пользователя через EgoHub, поэтому без достижимого EgoHub и валидного токена
  падают целиком, а не по одному тесту.
- `Freem.Web.Api.Public/appsettings.Development.json` содержит пустые `EgoHubIntegration:Url/Token`; хост при этом
  по решению пользователя падает на старте (исправлять не нужно).

## Направление решения (не пошаговая реализация)

1. Сделать EgoHub достижимым из контейнеров: запустить его Public API не только на loopback (например,
   `http://0.0.0.0:25001` или адрес `docker0`). Перезапуск делает пользователь: процесс его.
2. Пользователь запускает `docker compose up --build --abort-on-container-exit test` и затем `down -v` в
   `docker/test`; результат фиксируется здесь.
3. Проверка хоста: поднять `develop` (пользователь), заполнить локальный `appsettings.Development.json`
   (`EgoHubIntegration` — адрес `http://127.0.0.1:25001` и токен), запустить хост `dotnet run` (порт 10190)
   и проверить запросы (`api/v1/auth/*`, `user/register`, `user/me`). Хост запускает агент, процесс закрывается по PID.
4. Найденные проблемы записываются сюда; что чинить в этом этапе, а что откладывать, решает пользователь.

## Явно не входит в этот файл (пока)

Проверка фронтенда в браузере (по решению пользователя не выполняется без отдельной просьбы), падение хоста при пустом
`EgoHubIntegration:Url/Token`.

## Критерий готовности

- `docker/test`: все тестовые сборки зелёные (включая `FunctionalTests` с новыми тестами `UpdateRecord*`).
- Хост стартует на `develop`, `register`/`login`/`user/me` отвечают ожидаемо.
- Все найденные проблемы записаны; зелёная база зафиксирована.

## План реализации

Не расписан: этап состоит из проверок (см. направление). Расписывается после согласования.

## Статус и изменения

- 2026-10-18 (Claude Code, claude-sonnet-5-5): исследование и подготовка записаны: сервис и токен EgoHub созданы, конфиг `docker/test` заполнен (скрыт `skip-worktree`); ждём, пока EgoHub станет достижим из контейнеров и пользователь запустит `docker/test`; реализация не начата.
- 2026-10-18 (Claude Code, claude-sonnet-5-5): первый прогон `docker/test` дал `UnauthorizedException` в `Freem.Entities.UseCases.IntegrationTests` (не связано с EgoHub: токен и `register`/`login` через `docker0` проверены вручную, отвечают 200). Причина — регрессия этапа 6: `RegisterUserUseCase` берёт идентификатор из контекста, а тестовый хелпер `UsersPlainExecutor.RegisterAsync` передавал `UseCaseExecutionContext.Empty`. Исправлено: хелпер создаёт контекст со случайным `UserIdentifier`; `dotnet build` проекта — 0 ошибок; повторный прогон `docker/test` ждёт пользователя.
- 2026-10-18 (Claude Code, claude-sonnet-5-5): по замечанию пользователя значения, специфичные для машины, убраны из закоммиченных файлов: EgoHub для `docker/test` задаётся через `docker/test/.env` (шаблон `.env.example`) и переменные окружения контейнера; `skip-worktree` с конфига `docker/test/configs` снят, файл возвращён к пустым значениям.
- 2026-10-18 (Claude Code, claude-sonnet-5-5): второй прогон `docker/test`: unit-сборки и `Locking.Redis.IntegrationTests` зелёные; в `PostgreSQL.IntegrationTests` падали тесты `FindAsync_ShouldFailure_WhenPartOfIdsNotExists` (Tags, Records, а также Activities и Events по тому же вызову): `Can't generate "TIdentifier"` — тесты брали `UserIdentifier` из `IdentifiersGenerator`, а генератор `UserIdentifier` удалён на этапе 6. Исправлено: `BaseRepositoryTests.GenerateUserIdentifier()` (случайное значение), 4 вызова заменены; `dotnet build Freem.sln` — 0/0. Вывод остальных сборок (`UseCases`, `FunctionalTests`) в присланном фрагменте не поместился; повторный прогон ждёт пользователя.
- 2026-10-18 (Claude Code, claude-sonnet-5-5): третий прогон `docker/test`: остался один падающий тест — `FunctionalTests` `RegisterUserTests.Request_WhenAlreadyRegistered_ShouldConflict` (ожидался 409, пришёл 500). Причина — регрессия этапа 2: при удалении логина из `DuplicateKeyErrorToExceptionConverter` убрали единственное соответствие, по которому повторная регистрация превращалась в `DuplicateKeyStorageException`; нарушение первичного ключа `users` нигде не отображается и уходит как необработанное исключение. Исправлено: добавлен `DuplicateKeyStorageException.ErrorCode.DuplicateUser` и соответствие `EntitiesNames.Users.Constraints.PrimaryKey` → `DuplicateUser`; схема БД не менялась; `dotnet build Freem.sln` — 0/0, unit-тесты `PostgreSQL.UnitTests` зелёные. Проверка новым прогоном `docker/test` ждёт пользователя.
- 2026-10-18 (Claude Code, claude-sonnet-5-5): `docker/test` зелёный у пользователя (все сборки). Проверка хоста на `develop` (запуск `dotnet run`, порт 10190, EgoHub `127.0.0.1:25001`; процессы закрыты по PID): хост стартует; `auth/register` 200, `auth/login` 200 (access-токен в теле, refresh-cookie), `user/me` до регистрации в Freem 401, `user/register` 200, повторный `user/register` 409, `user/me` 200, `activities` 200 (пустой список), `auth/refresh` 200, запрос без токена 401; в логе хоста две записи `fail` — ожидаемое нарушение первичного ключа `users` при повторной регистрации (EF логирует, use case превращает в 409), других ошибок нет. Для хоста выпущен токен EgoHub `api-public-dev` (сервис `freem`), значения записаны в локальный `appsettings.Development.json` (скрыт `skip-worktree`); часть правок кода и `docker/test` — см. записи выше. Этап ожидает закрытия пользователем.
- 2026-10-18 (Claude Code, claude-sonnet-5-5): этап закрыт пользователем; реализация закоммичена (`a54ffe6..49323aa`).
