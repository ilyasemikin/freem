# Этап: Инфраструктура

Автор: Claude Code, claude-sonnet-5-5, 2026-10-18

См. общий контекст и место этого этапа: `plans/freem-revival/general/roadmap.md` (этап 5).

## Зачем именно так

Сейчас Freem запускается только вручную: адреса БД и Redis (`192.168.1.2:24000/24001`) и пароль БД закоммичены,
схема применяется командой `dotnet ef database update` руками, интеграционные тесты требуют чужого
`configuration.json`, порт API (30000) и CORS-origin зашиты в конфиг и во фронтенд. Соседние проекты (StashIt,
Trandoma) решают это одинаково: три Docker Compose окружения (`develop`, `service`, `test`) с `.env`, шаблонами
конфигов и отдельным контейнером миграций. Цель этапа — привести Freem к тому же виду, чтобы дальше (EgoHub,
фронтенд) работать на воспроизводимом окружении.

## Зафиксированные факты

Проверено 2026-10-18 чтением кода и конфигов соседних проектов (Docker не запускался).

**Freem сейчас.**

- `docker/` нет; есть только `Freem.Web.Api.Public/Dockerfile` (SDK/aspnet 10.0, публикует API, копирует
  `appsettings.json`).
- В git закоммичены: пароль и адрес БД в `Freem.Web.Api.Public/appsettings.Development.json` и
  `Freem.Web.Api.Public.FunctionalTests/configuration.json` (там же адрес Redis); `skip-worktree` ни на одном
  файле не стоит. `appsettings.json` содержит `Api.Ports: [30000]`, `CorsOrigins: [http://localhost:5173]`.
- Фронтенд: адрес API зашит — `new HttpClient("http://localhost:30000")` в
  `Freem.Web/src/contexts/clients/BackendClientsProvider.tsx`; `.env`/`import.meta.env` не используются,
  `vite.config.ts` пустой.
- Миграции: проект `Freem.Entities.Storage.PostgreSQL.Migrations` — библиотека с `IDesignTimeDbContextFactory`
  (`DesignTimeDatabaseContextFactory`, строка подключения — `args[0]`), применяется вручную
  (`dotnet ef database update -- "<conn>"`, см. README проекта); исполняемого раннера и Dockerfile нет.
- Очистка таблиц: `DatabaseContextExtensions.TruncateTables` (`Freem.Entities.Storage.PostgreSQL`, internal) делает
  `TRUNCATE ... CASCADE` по всем таблицам без каких-либо проверок; вызывается из тестов трёх проектов
  (`PostgreSQL.IntegrationTests`, `UseCases.IntegrationTests`, `Web.Api.Public.FunctionalTests`). Если в
  `configuration.json` указана боевая база, тесты её очистят.
- Конфигурация интеграционных тестов: `configuration.json` в `PostgreSQL.IntegrationTests` (`ConnectionString`),
  `Locking.Redis.IntegrationTests`, `UseCases.IntegrationTests` (закоммичены с пустыми значениями), и
  `FunctionalTests/configuration.json` (с реальными значениями; пример — `configuration.example.json`).
- Redis нужен хосту (блокировки) и `Locking.Redis.IntegrationTests`.

**Образец соседей** (StashIt, Trandoma).

- `docker/develop` — зависимости для локальной разработки: `docker-compose.yaml`, `.env.example` (`.env` игнорируется),
  `templates/` + `configs-render/` (сервис `configs-render` рендерит конфиги из `.env`), порты наружу из `.env`,
  `COMPOSE_PROJECT_NAME=<проект>_develop`.
- `docker/service` — «боевое» окружение: те же зависимости плюс контейнеры приложений, бэкап/восстановление
  PostgreSQL (`profile manual`), `README.md`.
- `docker/test` — Trandoma: PostgreSQL с `test/test` + контейнер миграций + приложения + контейнер `test`, который
  гоняет `dotnet test` внутри Docker; StashIt: только зависимость (Mongo) + контейнер `test`.
- Применение миграций в Trandoma — отдельный сервис `*-postgres-migration` (`restart: no`, `depends_on: postgres: healthy`),
  собираемый из `Dockerfile` проекта миграций (`dotnet tool install dotnet-ef`, `entrypoint.sh`); проверяет наличие
  `packages/*.nupkg`. Контекст сборки — `_Src/<Проект>`.
- Образ PostgreSQL в соседях — `postgres:18`.

**Ограничения из `CLAUDE.md` и решений плана.** Пул портов Freem 10150–10199: `docker/develop` 10150–10169,
`docker/service` 10170–10189, локальный `dotnet run`/прочее 10190–10199; стандартные порты (5432, 6379…) не
занимать; интеграционные тесты запускаются локальным `dotnet test` против локально запущенных сервисов, а не
Docker-пайплайном; Docker Compose сам не запускаю (сокет может быть недоступен — запускает пользователь);
локальные секреты в отслеживаемых конфигах скрываются через `git update-index --skip-worktree`.

## Один-два иллюстративных примера (не исчерпывающий список!)

- `TruncateTables` сегодня безусловен; защита — например, проверка `current_database()` (имя заканчивается на
  `_test`) перед `TRUNCATE` с исключением иначе.
- `BackendClientsProvider.tsx`: `"http://localhost:30000"` → `import.meta.env.VITE_API_URL`.

## Направление решения (не пошаговая реализация)

1. **Три окружения по образцу Trandoma** (там тот же стек PostgreSQL + EF Core):
   - `docker/develop`: PostgreSQL 18, Redis, контейнер миграций; порты наружу из 10150–10169; `.env.example`.
   - `docker/service`: всё, что нужно для работы API и web (PostgreSQL, Redis, миграции, API, фронтенд-статика),
     плюс бэкап/восстановление PostgreSQL (`profile manual`) и `README.md`; порты из 10170–10189.
   - `docker/test`: изолированное окружение целиком в Docker: отдельный PostgreSQL (`freem_test`), Redis,
     контейнер миграций и контейнер `test`, гоняющий `dotnet test`; портов наружу нет, после прогона окружение
     гасится (автоматизировать остановку не обязательно).
2. **Раннер миграций** — Dockerfile + `entrypoint.sh` в каталоге проекта миграций (`dotnet ef database update --
   "<conn>"` с параметрами из env), как в Trandoma; код миграций не меняется.
3. **Защита `TruncateTables`**: перед очисткой проверяется, что имя БД оканчивается на `_test`, иначе исключение.
4. **Конфигурация**: хост берёт строки подключения и порт из конфигурации/env, фронтенд — `VITE_API_URL` (на этапе
   сборки); закоммиченные секреты заменить пустыми значениями, реальные — в `skip-worktree`-файлах и `.env`;
   `FunctionalTests/configuration.json` — пустой закоммиченный шаблон + `skip-worktree`.
5. Пароль из истории git: по словам пользователя, стенд `192.168.1.2` уже очищен, смена пароля не требуется;
   историю не переписываем.

Решения пользователя (2026-10-18): `service` — всё для API и web + бэкапы; контейнер миграций делаем; `test` —
как в Trandoma (тесты в контейнере, наружу порты не публикуются); правило защиты — имя БД на `_test`;
`VITE_API_URL` достаточно; `FunctionalTests/configuration.json` переводим на шаблон. Из этого следует, что пункт
`CLAUDE.md` «`.IntegrationTests` идут локальным `dotnet test` против локальных сервисов» устаревает — его нужно
переписать (тесты запускает `docker/test`); локально против `develop` остаётся возможным, но не основной путь.

## Явно не входит в этот файл (пока)

План реализации по шагам; точный список портов и переменных `.env`; содержимое шаблонов конфигов и `Dockerfile`
фронтенда; обновление `CLAUDE.md` (дописать при реализации).

## Критерий готовности

- `docker compose config` для `develop`/`service`/`test` проходит без ошибок (запуск окружений — на стороне
  пользователя, с его подтверждением; образы собирает он же, если сокет недоступен мне).
- Контейнер миграций применяет всю цепочку миграций на пустую БД (проверка пользователем после запуска окружения;
  я проверяю только сборку образа/`compose config`, если доступен Docker).
- `dotnet build Freem.sln` и unit-тесты проходят; интеграционные тесты `PostgreSQL`, `Redis`, `UseCases`,
  `FunctionalTests` проходят в окружении `docker/test` (прогон делает пользователь; теперь это критерий готовности,
  см. решения roadmap).
- `TruncateTables` отказывается работать на базе, не прошедшей проверку (покрыто тестом).
- В отслеживаемых файлах нет паролей и боевых адресов; `git ls-files -v` показывает `S` у файлов с локальными
  значениями; `npm run build` проходит, адрес API берётся из `VITE_API_URL`.
- `CLAUDE.md` описывает окружения и порты.

## План реализации

Каждый шаг заканчивается `dotnet build Freem.sln` (0 предупреждений) и unit-тестами, где применимо.

1. **Защита очистки.** В `TruncateTables` проверка `current_database()` оканчивается на `_test`, иначе исключение;
   unit-тест на правило, если проверку можно вынести в чистую функцию.
2. **Раннер миграций.** `Dockerfile` + `entrypoint.sh` в `Freem.Entities.Storage.PostgreSQL.Migrations` (по образцу
   Trandoma, с проверкой `packages/*.nupkg`); README проекта дополнить. Проверка: `docker build`, если доступен
   сокет, иначе — чтение и `docker compose config`.
3. **Конфигурация и секреты.** Убрать пароль/адреса из `appsettings.Development.json` и
   `FunctionalTests/configuration.json` (шаблон с пустыми значениями), поставить `skip-worktree` на файлы с
   локальными значениями; проверить `git ls-files -v`. Порт API и CORS — из конфигурации/env.
4. **`docker/develop`.** PostgreSQL 18, Redis, миграции; `.env.example`, `.gitignore`; порты из 10150–10169.
   Проверка: `docker compose config`.
5. **`docker/test`.** PostgreSQL `freem_test` и Redis без публикации портов, миграции, контейнер `test` с
   конфигами тестов, копируемыми в `Dockerfile`. Проверка: `docker compose config`; прогон — пользователь.
6. **Фронтенд.** `VITE_API_URL` вместо зашитого адреса, `.env.example`, `Dockerfile` фронтенда (сборка + статика).
   Проверка: `npm run build`.
7. **`docker/service`.** PostgreSQL, Redis, миграции, API, web, шаблоны конфигов и `configs-render`, бэкап и
   восстановление PostgreSQL, `README.md`; порты из 10170–10189. Проверка: `docker compose config`.
8. **Документация.** `CLAUDE.md` (окружения, порты, путь запуска тестов), итоговая сверка критерия готовности.

## Статус и изменения

(До аппрува плана: одна переписываемая запись.)

- 2026-10-18 (Claude Code, claude-sonnet-5-5): пользователь ответил на вопросы исследования, направление и критерий обновлены, план реализации записан; ожидает одобрения плана, реализация не начата.
- 2026-10-18 (Claude Code, claude-sonnet-5-5): план одобрен пользователем; шаги 1–8 выполнены, не закоммичено. `dotnet build Freem.sln`: 0 предупреждений, 0 ошибок; unit-тесты зелёные (добавлен тест правила `_test`); `npm run build` проходит; `docker compose config -q` проходит для `develop`/`test`/`service`. Сокет Docker для меня закрыт, поэтому образы не собирались, миграции и `docker/test` не запускались — проверка за пользователем. Отклонения: (1) `FunctionalTests/configuration.json` был устаревшим (ключ `WebApiConfiguration` вместо `WebApiApplication`, без `Api`/`OpenApi`) — приведён к реальной форме, а `configuration.example.json` (другая устаревшая форма) удалён; (2) `skip-worktree` не ставил: он скрыл бы от git сами закоммиченные плейсхолдеры — пользователь ставит его, когда внесёт локальные значения (`appsettings.Development.json` теперь с портами/адресами пула, пустыми `UserID`/`Password`); (3) порты локального запуска: API 10190, Vite 10191 (`strictPort`); (4) добавлены `.dockerignore` в корне и в `_Src/Freem` (иначе `COPY` тащит `bin/obj/node_modules`); (5) конфигурация `service` передаётся переменными окружения (`Postgres__ConnectionString` и т. д.), шаблоны `configs-render` не понадобились; (6) в `entrypoint.sh` миграций нет `--no-build` (как в Trandoma). `CLAUDE.md` обновлён. Ожидает проверки и закрытия пользователем.
- 2026-10-18 (Claude Code, claude-sonnet-5-5): первый прогон `docker/test` у пользователя показал падения. Исправлено: (1) регрессия этапа 2 — `CurrentUser` регистрировался в DI с лишним аргументом `JsonSerializerOptions` (теперь `TryAddSingleton<CurrentUser>()`); (2) старое расхождение — сервер использует camelCase (`Program.cs`, коммит фронтенда aaaefa7), клиенты API — `snake_case`, из-за чего `RegisterUserResponse.UserId` приходил `null`; оба клиента (`Client`, `SyncClient`) теперь явно ставят camelCase; (3) три тестовые сборки гоняются параллельно в одной БД и обнуляют таблицы друг у друга — у каждой теперь своя БД (`freem_storage_test`, `freem_usecases_test`, `freem_api_test`), `entrypoint.sh` миграций принимает список БД через пробел; (4) хелпер `ShouldThrowExceptionAsync<T>` в `PostgreSQL.IntegrationTests` ищет `PostgresException` и во внутренних исключениях (EF оборачивает в `DbUpdateException`). Остаются невыясненными падения `UsersSettings*UseCaseTests` и тестов с `RunningRecordIdentifier` в имени; нужен повторный прогон.
- 2026-10-18 (Claude Code, claude-sonnet-5-5): после разделения БД оставшиеся падения (активности, триггеры, репозитории) имели одну причину: `column "status" is of type core_entities.activity_status but expression is of type integer` — enum статуса активности отображался EF-провайдером как integer (последствие перехода на EF Core/Npgsql 10 на этапе 1: одного `MapEnum` на `NpgsqlDataSource` недостаточно). Добавлен `options.MapEnum<ActivityStatus>(name, schema)` в `UseNpgsql` и схема в имя типа при `MapEnum` на источнике данных. Проверено только сборкой; ожидает повторного прогона `docker/test` пользователем.
- 2026-10-18 (Claude Code, claude-sonnet-5-5): после правки enum в прогоне `docker/test` остались 3 падения, не связанных с инфраструктурой. По решению пользователя `GET api/v1/tags/by-name/{name}` теперь возвращает один тег или 404 (было: список и 200 с пустым списком; `FindTagByNameController`). `StatisticsPerDaysUseCaseTests.ShouldSuccess`: use case пропускает период из одного дня (`while (current < end)` при `StartAt == EndAt`) — ждёт решения пользователя, конец периода включительный или нет.
- 2026-10-18 (Claude Code, claude-sonnet-5-5): по решению пользователя конец периода в статистике по дням включительный (`StatisticsPerDaysUseCase`: `end = EndAt.AddDays(1)`); `DatePeriod(day, day)` теперь даёт один день. Проверено сборкой; ждёт повторного прогона `docker/test`.
- 2026-10-18 (Claude Code, claude-sonnet-5-5): полный прогон `docker/test` у пользователя зелёный: все 11 тестовых сборок (unit, `PostgreSQL`/`UseCases`/`Redis` IntegrationTests, `FunctionalTests`) без падений. Критерий готовности по тестам выполнен; окружения `develop` и `service` пользователем ещё не запускались. Ожидает проверки и закрытия пользователем.
- 2026-10-18 (Claude Code, claude-sonnet-5-5): пользователь запустил `develop` и `service` с заполненными `.env` (созданы локально, в git не входят): окружения поднимаются и работают, часть по запуску Docker подтверждена. Веб-интерфейс в `service` выдаёт ошибки — ожидаемо (код авторизации фронтенда удалён на этапе 2, актуализация фронтенда — этап 7). Ожидает проверки и закрытия пользователем.
- 2026-10-18 (Claude Code, claude-sonnet-5-5): этап 5 закрыт пользователем; реализация закоммичена отдельными коммитами (см. `git log`).
