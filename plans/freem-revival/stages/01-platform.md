# Этап: Платформа и зависимости

Автор: Claude Code, claude-sonnet-5-5, 2026-10-18

См. общий контекст и место этого этапа: `plans/freem-revival/general/roadmap.md` (этап 1).

## Зачем именно так

Платформа идёт первой: без net10.0, актуальных EF Core/Npgsql и единого тестового стека остальные этапы
(удаление авторизации и Kafka, общие библиотеки) пришлось бы делать на устаревшей основе и затем переделывать.
Отдельный этап «базовой линии» упразднён по решению пользователя; его аудит (ниже) служит точкой отсчёта
для этого этапа, а полезные находки включены в его объём.

## Зафиксированные факты (аудит перед этапом)

Проверено 2026-10-18 на SDK 10.0.400 (`dotnet --list-runtimes`: только Microsoft.NETCore.App 10.0.11 и
Microsoft.AspNetCore.App 10.0.11).

Сборка (`dotnet build Freem.sln --no-incremental`):

- Падает с 3 ошибками MSB3030 (нет `configuration.json`) в `Freem.Entities.UseCases.IntegrationTests`,
  `Freem.Entities.Storage.PostgreSQL.IntegrationTests`, `Freem.Locking.Redis.IntegrationTests`. Рядом с каждым
  лежит закоммиченный `configuration_example.json`. У `FunctionalTests` пример назван иначе
  (`configuration.example.json`), а сам `configuration.json` закоммичен.
- Остальные проекты собираются. 11 предупреждений компилятора: CS8618 в фильтрах
  `Freem.Entities.Storage.Abstractions/Models/Filters/*` (`Limit`/`Offset`, 7 шт.), CA2264 в
  `ListEventRequest.cs:18`, CS8774 в `ClientResult.cs:44`, NU1603 (Castle.Core 4.0.0) в
  `Storage.PostgreSQL.IntegrationTests`.
- `Freem.Http` — net8.0, остальные проекты net9.0. `Freem.Entities.Events.InPlace` содержит только `.csproj`.

Тесты:

- Хост-рантайма net9.0 нет, поэтому `dotnet test` на net9.0-проектах падает с «You must install or update
  .NET». С `DOTNET_ROLL_FORWARD=Major` unit-проекты проходят: Collections 10, Converters 29,
  Storage.PostgreSQL.Migrations 3, Storage.PostgreSQL 77, Entities 76, Identifiers 25, Locking.Redis 1,
  Time 109 — без падений.
- `Freem.Web.Api.Public.FunctionalTests` без поднятого API не работают (тесты падают за ~1 мс, прогон
  набора не завершился за 10 минут и был прерван). Интеграционные проекты не запускались (нет конфигурации
  и инфраструктуры; до этапа 6 это не критерий).

Фронтенд (`Freem.Web`):

- `npm ci` проходит (предупреждение: скрипты esbuild заблокированы `allowScripts`), `npm run build` проходит
  (предупреждение о чанке 592 kB).
- `npm run lint`: 1 ошибка (`no-explicit-any` в `src/clients/http/IHttpMethodProps.ts:2`) и 5 предупреждений
  (`react-hooks/exhaustive-deps` ×4, `react-refresh/only-export-components` ×1).

Рабочее дерево после проверок чистое (`dist` и `node_modules` игнорируются).

## Один-два иллюстративных примера (не исчерпывающий список!)

- `Freem.Locking.Redis.IntegrationTests.csproj:34` копирует `configuration.json`, которого нет в репозитории.
- Предупреждения CS8618 в `ActivitiesByUserFilter.cs:33` (`Limit`, `Offset`).

## Направление решения (не пошаговая реализация)

- Основная часть этапа: перевод на net10.0 (снимает проблему запуска тестов без `DOTNET_ROLL_FORWARD`), EF Core/Npgsql, `Dockerfile`, пакеты, единый тестовый стек.
- Чтобы `dotnet build Freem.sln` собирался на чистом клоне: либо условное копирование `configuration.json`
  (`Condition="Exists(...)"`), либо иное решение; вариант выбрать с пользователем.
- Имена файлов-примеров унифицировать (`configuration_example.json` и `configuration.example.json`): какое имя
  берём — по образцу StashIt, уточнить при реализации.
- Предупреждения компилятора: исправить или явно принять; ошибку линтера исправить.
- Проекты вне решения (`Freem.Http`, `Freem.Entities.Events.InPlace`): удалены по решению пользователя (2026-10-18).
- Замечание для этапа 6: закоммиченный `FunctionalTests/configuration.json` (в том числе секреты) — не
  трогаем здесь, это зона этапа 6.

## Явно не входит в этот файл (пока)

План реализации; решение по каждому предупреждению; поведение FunctionalTests с живым API.

## Критерий готовности

- `dotnet build Freem.sln` проходит на чистом клоне без ручного создания файлов, без предупреждений
  (или с явно принятым списком).
- Unit-тесты проходят одной командой, которая описана в `CLAUDE.md`.
- `npm ci`, `npm run build`, `npm run lint` проходят без ошибок.
- Все проекты на net10.0; unit-тесты запускаются без `DOTNET_ROLL_FORWARD`.

## План реализации

Пока не расписан. Дописывается после согласования исследования.

## Статус и изменения

- 2026-10-18 (Claude Code, claude-sonnet-5-5): этап «Базовая линия» слит в этот этап; аудит записан, проекты вне решения удалены; направление и критерий готовности ожидают согласования, реализация не начата.
