# Этап: Платформа и зависимости

Автор: Claude Code, claude-sonnet-5-5, 2026-10-18

См. общий контекст и место этого этапа: `ai/plans/freem-revival/general/roadmap.md` (этап 1).

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

Каждый шаг заканчивается проверкой и показом результата; коммиты только по просьбе пользователя. Пакеты,
которые уйдут на этапах 2 и 3 (JWT, Kafka), здесь только поднимаются до совместимых версий, но не
удаляются.

1. **Конфигурация интеграционных проектов.** Сделать копирование `configuration.json` условным
   (`Condition="Exists(...)"`) в трёх `.IntegrationTests`. Унифицировать имена файлов-примеров: единое имя
   `configuration.example.json` (как уже у `FunctionalTests`; если в StashIt/Trandoma принято другое — сверить
   и взять его); обновить упоминания в `CLAUDE.md`. Проверка: `dotnet build Freem.sln` на чистом клоне
   (`git clean -xdn` для просмотра, затем сборка) без MSB3030.
2. **Версия платформы.** Перевести `TargetFramework` всех проектов на `net10.0`; `Dockerfile` — на образы
   `sdk:10.0` / `aspnet:10.0`. Проверка: `dotnet build Freem.sln` (предупреждения сравнить с аудитом),
   `docker build` не запускается без согласования (сокет может быть недоступен) — только прочитать файл.
3. **Пакеты Microsoft.** `Microsoft.EntityFrameworkCore*`, `Npgsql.EntityFrameworkCore.PostgreSQL`,
   `Microsoft.Extensions.*`, `Microsoft.AspNetCore.*`, `Scalar.AspNetCore` — до версий для net10.0
   (ориентир: Trandoma/EgoHub, EF Core/Npgsql 10.0.0). Проверка: сборка; затем
   `dotnet ef migrations has-pending-model-changes` (или эквивалентная проверка snapshot). Если snapshot
   или модель расходятся и нужна новая миграция либо правка схемы — **остановиться и согласовать с
   пользователем**, самостоятельно миграции не генерировать.
4. **Прочие пакеты.** `StackExchange.Redis`, `Confluent.Kafka`, `Microsoft.IdentityModel.*`, `System.Linq.Async`,
   `AutoFixture*` — до актуальных стабильных версий, совместимых с net10.0. Проверка: сборка, unit-тесты.
5. **Единый тестовый стек.** Все тестовые проекты на xunit (как в StashIt): перевести `Freem.Time.UnitTests`
   с NUnit на xunit; выровнять версии `xunit`, `xunit.runner.visualstudio`, `Microsoft.NET.Test.Sdk`,
   `coverlet.collector` по StashIt. Проверка: `dotnet test` по unit-проектам без `DOTNET_ROLL_FORWARD`,
   число тестов не уменьшилось (Time: 109).
6. **Предупреждения компилятора.** Разобрать список из аудита (CS8618 в фильтрах, CA2264, CS8774, NU1603) и
   новые предупреждения net10.0: исправить причину, без подавления; если исправление меняет публичный
   контракт — спросить пользователя. Проверка: чистая пересборка (`--no-incremental`), список оставшихся
   предупреждений показать.
7. **Фронтенд.** Исправить ошибку `no-explicit-any` и 5 предупреждений lint; проверить, что `npm ci`
   проходит без предупреждений о заблокированных install-скриптах (решить `allowScripts` для esbuild).
   Обновление самих npm-зависимостей — этап 7, здесь не делается. Проверка: `npm ci`, `npm run build`,
   `npm run lint` без ошибок.
8. **Итог.** Прогнать критерий готовности целиком и показать результат; дописать статусы и остановиться.

## Статус и изменения

- 2026-10-18 (Claude Code, claude-sonnet-5-5): этап «Базовая линия» слит в этот этап; аудит записан, проекты вне решения удалены; направление согласовано пользователем (критерий готовности отдельно не подтверждался); план реализации записан, ожидает согласования, реализация не начата.
- 2026-10-18 (Claude Code, claude-sonnet-5-5): пользователь подтвердил критерий готовности и одобрил план реализации; начат шаг 1.
- 2026-10-18 (Claude Code, claude-sonnet-5-5): шаг 1 выполнен с отклонением от плана по решению пользователя: вместо условного копирования и переименования примеров взята конвенция соседей — `configuration.json` закоммичен с пустыми значениями в трёх `.IntegrationTests` (бывшие `configuration_example.json`), записи убраны из `source/.gitignore`, README и `CLAUDE.md` обновлены. `FunctionalTests/configuration.json` и `configuration.example.json` не тронуты (секреты — этап 5). Проверка: `dotnet build Freem.sln --no-incremental` — 0 ошибок, 11 предупреждений (как в аудите).
- 2026-10-18 (Claude Code, claude-sonnet-5-5): шаги 2–8 выполнены, этап ожидает проверки пользователем. Итог: все проекты на net10.0, `Dockerfile` на образах 10.0 (`docker build` не запускался); `dotnet build Freem.sln --no-incremental` — 0 ошибок, 0 предупреждений (в т.ч. NuGet-уязвимости: EF Core/Npgsql/Microsoft.* → 10.0.0, `Microsoft.OpenApi` 2.12.2, `System.Security.Cryptography.Pkcs/Xml` 10.0.13 закреплены явно в Migrations); `dotnet ef migrations has-pending-model-changes` — модель не менялась, миграции не создавались; unit-тесты — 330 из 330 без `DOTNET_ROLL_FORWARD`; `Freem.Time.UnitTests` переведён с NUnit на xunit (109 тестов, как было); версии xunit/Test.Sdk/runner/coverlet выровнены по StashIt; фронтенд: `npm run build` проходит, `npm run lint` — 0 ошибок.
- 2026-10-18 (Claude Code, claude-sonnet-5-5): отклонения от плана. (1) Удалён проект `Freem.Linq` (единственный метод `ToArrayAsync` конфликтует с `System.Linq.AsyncEnumerable` из BCL .NET 10) вместе с пакетом `System.Linq.Async`. (2) Шаг 3: код OpenApi-трансформеров портирован на Microsoft.OpenApi 2.x (JWT-трансформеры будут удалены на этапе 2). (3) Шаг 6: `required` у `Limit`/`Offset` в 4 фильтрах (все места вызова уже задавали их); из `ListEventRequest` убран бессмысленный `ThrowIfNull`; `ClientResult<T>.EnsureSuccess` теперь бросает `InvalidOperationException`, если успешный ответ без значения (раньше атрибут обещал не-null, но не проверял); xUnit2013 в интеграционных тестах → `Assert.Single`; `TheoryData` в `EventsRepositoryTests` под xunit 2.9.3. (4) Шаг 7: исправлена только ошибка lint (`QueryParams` = `object`, `String(...)` при сборке query); 5 предупреждений `react-hooks/exhaustive-deps`/`only-export-components` оставлены — правка меняет поведение эффектов, два из них в файлах, которые меняются на этапах 2 и 6; вопрос `allowScripts` для esbuild не решался (сборка работает). Не проверялось: запуск хоста и OpenAPI-документа, интеграционные и функциональные тесты (нужна инфраструктура).
- 2026-10-18 (Claude Code, claude-sonnet-5-5): этап 1 закрыт пользователем; 5 предупреждений lint перенесены на этап 7 (фронтенд).
