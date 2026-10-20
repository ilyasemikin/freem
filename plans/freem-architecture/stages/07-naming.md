# Этап: Именование проектов и слоёв

Автор: Claude Code, claude-sonnet-5-5, 2026-10-20

См. общий контекст и место этого этапа: `plans/freem-architecture/general/roadmap.md` (этап 7).

## Зачем именно так

Решения пользователя (roadmap, 2026-10-18): `Freem.Web.Api.*` → `Freem.Api.Public.*`; остальные проекты приводятся
к схеме StashIt — и имена, и состав. Этапы 2–6 уже привели состав и границы слоёв в порядок (29 `.csproj`), поэтому
здесь остаётся чистое переименование: сначала имена согласуются с пользователем, потом механически применяются.
Этап последний, потому что затрагивает почти каждый файл (имена проектов и пространств имён) и не должен
пересекаться со структурными правками. Роадмап требует согласовывать с пользователем каждое решение — объём,
порядок, имена; поэтому ниже только предложения и открытые вопросы, а не готовый план.

## Зафиксированные факты

**Текущие проекты (29 `.csproj`, `_Src/Freem/`)** и их аналоги в StashIt (`Core/`, префикс `Stashit.Core.` нужен
StashIt только из-за нескольких продуктов — `Core`, `Markets`, `Agent`; у Freem продукт один, поэтому `Core` не
нужен):

| Сейчас | Аналог в StashIt |
|---|---|
| `Freem.Web.Api.Public` (+ `.Client`, `.Contracts`, `.IntegrationTests`, `.UseCases`, `.UseCases.Contracts`) | `Stashit.Core.Api.Public` (+ те же суффиксы) |
| `Freem.Domain`, `Freem.Domain.Entities`, `Freem.Domain.Storage`, `Freem.Domain.UnitTests` | `Stashit.Core.Domain`, `.Domain.Entities`, `.Domain.Storage`, `.Domain.UnitTests` — совпадают |
| `Freem.Entities.UseCases` (+ `.Contracts`, `.IntegrationTests`) | `Stashit.Core.Domain.UseCases` (+ `.Contracts`, `.IntegrationTests`) |
| `Freem.Entities.Storage.PostgreSQL` (+ `.Migrations`, `.UnitTests`, `.IntegrationTests`, `.Migrations.UnitTests`) | `Stashit.Core.Domain.Storage.Mongo` (+ `.Migrations`) |
| `Freem.Entities.Serialization.Json` | `Stashit.Core.Domain.Serialization.Json` |
| `Freem.Entities.Events.Producer` | в StashIt события — отдельный `Stashit.Events` (+ `.Serialization.Json`) на верхнем уровне |
| `Freem.Storage.Abstractions`, `Freem.Storage.EFCore` | `Stashit.Storage.Transactions`, `Stashit.Storage.Transactions.Mongo` |
| `Freem.Locking.Abstractions`, `Freem.Locking.Redis` (+ тесты) | `Stashit.Locking`, `Stashit.Locking.InMemory` (+ тесты) |
| `Freem.Sorting`, `Freem.Time` (+ `Time.UnitTests`) | `Stashit.Sorting`, `Stashit.Time` — совпадают |

**Что уже совпадает:** `Freem.Domain*`, `Freem.Sorting`, `Freem.Time`.

**Что привязано к именам проектов (проверено):**

- Имена проектов и пространств имён в `.cs`/`.csproj`/`.sln`: «Freem.Web.Api» встречается в 263 файлах,
  «Freem.Entities» — в 388, «Freem.Storage» — в 29, «Freem.Locking» — в 40.
- `docker/test/Dockerfile` (пути `COPY ./docker/test/configs/<Проект>.configuration.json ./<Проект>/…` — имена файлов
  конфигов в `docker/test/configs/` тоже содержат имена проектов), три `docker-compose.yaml`
  (`dockerfile: ./Freem.Entities.Storage.PostgreSQL.Migrations/Dockerfile`, `./Freem.Web.Api.Public/Dockerfile`),
  `Dockerfile` обоих проектов, `README.md` проекта API, `_Src/.gitignore`, `CLAUDE.md` (слои, команды, scope коммитов).
- `InternalsVisibleTo` в `Freem.Entities.Storage.PostgreSQL` перечисляет тестовые сборки по именам; в нём остались
  устаревшие имена (`…Filler`, `…FunctionalTests` — видны в `obj/`, исходный список нужно сверить при реализации).
- Миграции EF (`Freem.Entities.Storage.PostgreSQL.Migrations/Instances/*.cs`) идентифицируются в БД по
  `[Migration("<id>")]`, а не по пространству имён; переименование пространства имён схему и таблицу истории
  миграций не затрагивает (подтвердить на `docker/test` — миграции применяются при его запуске).
- Фронтенд `Freem.Web` — не .NET-проект, переименованию не подлежит.

## Один-два иллюстративных примера (не исчерпывающий список!)

- `Freem.Web.Api.Public.UseCases.Contracts` → `Freem.Api.Public.UseCases.Contracts`: меняются папка, `.csproj`,
  запись в `.sln`, `ProjectReference` в зависимых проектах и пространство имён во всех файлах.
- `Freem.Entities.Storage.PostgreSQL.Migrations` → `Freem.Domain.Storage.PostgreSQL.Migrations`: дополнительно
  меняются пути в `Dockerfile` проекта и в трёх `docker-compose.yaml`.

## Направление решения (не пошаговая реализация)

Механика: `git mv` папки и `.csproj` (чтобы сохранилась история), замена имени в `.sln`, `ProjectReference`,
пространствах имён, `using`, `InternalsVisibleTo`, Docker-файлах и документации; сборка и тесты после каждой группы.
Группы идут от листьев к корню либо по префиксу — порядок решает пользователь (вопрос 6).

Предлагаемые имена (всё — предложения, ни одно не принято):

- `Freem.Web.Api.Public*` → `Freem.Api.Public*` (решение roadmap, не обсуждается).
- `Freem.Entities.UseCases*` → `Freem.Domain.UseCases*`.
- `Freem.Entities.Storage.PostgreSQL*` → `Freem.Domain.Storage.PostgreSQL*`.
- `Freem.Entities.Serialization.Json` → `Freem.Domain.Serialization.Json`.
- остальное — по вопросам ниже.

## Решения пользователя (2026-10-20)

1. `Freem.Entities.*` → `Freem.Domain.*` (use case'ы, хранилище PostgreSQL, сериализация).
2. `Freem.Entities.Events.Producer` → `Freem.Domain.Events.Producer`.
3. `Freem.Storage.Abstractions` → `Freem.Storage.Transactions`; `Freem.Storage.EFCore` → `Freem.Storage.Transactions.EFCore`.
4. `Freem.Locking.Abstractions` → `Freem.Locking`.
5. Тестовые проекты и файлы конфигов в `docker/test/configs/` следуют за покрываемым проектом.
6. Один коммит на весь этап.
7. Имена сборок и пространства имён меняются вместе с папками.

Итоговые имена: `Freem.Web.Api.Public*` → `Freem.Api.Public*`; `Freem.Entities.UseCases*` → `Freem.Domain.UseCases*`;
`Freem.Entities.Storage.PostgreSQL*` → `Freem.Domain.Storage.PostgreSQL*`; `Freem.Entities.Serialization.Json` →
`Freem.Domain.Serialization.Json`; `Freem.Entities.Events.Producer` → `Freem.Domain.Events.Producer`.

## Явно не входит в этот файл (пока)

- Пошаговый план реализации (после согласования вопросов выше).
- Точный перечень затрагиваемых файлов — определяется при реализации.
- Переименование фронтенда `Freem.Web`, публичных маршрутов API и схемы БД — не затрагивается.

## Критерий готовности

- `dotnet build Freem.sln` — 0 ошибок и 0 предупреждений (как на этапах 5–6), `dotnet test Freem.sln` (unit) зелёный.
- Нет упоминаний старых имён (`Freem.Web.Api`, `Freem.Entities` и др. — по итогам согласованного списка) в
  `.cs`, `.csproj`, `.sln`, Docker-файлах и `CLAUDE.md`; проверяется `grep`.
- `docker/test` (`docker compose up --build --abort-on-container-exit test`) зелёный у пользователя; образы
  `develop`/`service` собираются с новыми путями (проверяет пользователь).
- `CLAUDE.md` (раздел «Repository Layout», команды, scope коммитов) обновлён под новые имена.

## План реализации

Каждый шаг: `git mv` папки и `.csproj`, замена имени в `.sln`, `ProjectReference`, пространствах имён, `using`,
`InternalsVisibleTo`, Docker-файлах; затем `dotnet build Freem.sln` (0/0) и `dotnet test Freem.sln` (unit).

1. `Freem.Web.Api.Public*` → `Freem.Api.Public*` (6 проектов); `Dockerfile`, `README.md` API, `docker/service`,
   конфиг `Freem.Web.Api.Public.IntegrationTests.configuration.json` и его `COPY` в `docker/test/Dockerfile`.
2. `Freem.Entities.*` → `Freem.Domain.*` (UseCases, Storage.PostgreSQL и тесты, Serialization.Json, Events.Producer);
   пути `Dockerfile` миграций в трёх `docker-compose.yaml`, конфиги `docker/test/configs/`.
3. `Freem.Storage.*` → `Freem.Storage.Transactions*`, `Freem.Locking.Abstractions` → `Freem.Locking`.
4. Сверка `InternalsVisibleTo` (устаревшие имена), `_Src/.gitignore`, `CLAUDE.md` (слои, команды, scope коммитов).
5. `grep` на старые имена по репозиторию (без `plans/`, `bin`, `obj`, `node_modules`); итоговая сборка и unit-тесты;
   остановка для прогона `docker/test` пользователем. Коммит — только по его просьбе, один на этап.

## Статус и изменения

- 2026-10-20 (Claude Code, claude-sonnet-5-5): план подтверждён пользователем.
- 2026-10-20 (Claude Code, claude-sonnet-5-5): этап 7: план согласован, реализация выполнена (шаги 1–5: `Freem.Web.Api.*` → `Freem.Api.*`, `Freem.Entities.*` → `Freem.Domain.*`, `Freem.Storage.Transactions*`, `Freem.Locking`; убраны устаревшие `InternalsVisibleTo` (`…Filler`, `…UseCases.DependencyInjection`), папка решения `Web.Api` → `Api`, `CLAUDE.md` обновлён), сборка 0/0 и unit-тесты зелёные, старых имён в репозитории нет; ждёт прогона `docker/test` и ревью пользователем, не закоммичено.
- 2026-10-20 (Claude Code, claude-sonnet-5-5): этап 7: `docker/test` зелёный, образы `develop` и `service` собираются и работают у пользователя; в `docker/service/.env` добавлены переменные EgoHub; критерий готовности выполнен, ждёт закрытия пользователем, не закоммичено.
- 2026-10-20 (Claude Code, claude-sonnet-5-5): этап 7 закрыт пользователем; реализация закоммичена (`624eaee`). Все этапы плана (0–7) закрыты.
