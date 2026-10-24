# Этап: Аудит лишнего

Автор: Claude Code, claude-opus-5-5, 2026-10-18

См. общий контекст и место этого этапа: `ai/plans/freem-architecture/general/roadmap.md` (этап 2).

## Зачем именно так

Этапы 3–7 переносят и переименовывают десятки проектов. Всё, что можно удалить или заменить стандартными средствами
dotnet/пакетами `Utilities.*` до переносов, не придётся переносить. Аудит касается вспомогательных `Freem.*` и
инфраструктуры; то, что и так переделывается следующими этапами (идентификаторы, use case'ы), здесь только отмечено,
чтобы не делать работу дважды.

## Зафиксированные факты

Проверено 2026-10-18: ссылки между `.csproj`, поиск использования типов по `*.cs` (без `obj/`, `bin/`), чтение кода
проектов, сравнение с `~/Sources/CSharpUtilities` и `~/Sources/StashIt`. В решении 54 C#-проекта (+ `Freem.Web`).

**Заменяются стандартными средствами, использований мало:**

- `Freem.DependencyInjection.Microsoft`: `Services.Resolve` — 3 теста; `BuildAndValidateServiceProvider` (обёртка
  над `ServiceProviderOptions { ValidateOnBuild = true }`) — 4 тестовых проекта; `TryAdd...WithImplementedInterfaces`/
  `...ExistedService` — по одному вызову в `Freem.Entities`, `Freem.Entities.Storage.PostgreSQL`, `Freem.Locking.Redis`.
- `Freem.EFCore.Extensions`: `ApplyConfiguration<TEntity, TConfiguration>()` (вместо стандартного
  `ApplyConfiguration(new TConfiguration())`) — в `Storage.PostgreSQL`; `DetachEntry` (`Entry(e).State = Detached`) —
  4 файла тестов хранилища.
- `Freem.Validation` (9 файлов): единственный потребитель — `Nickname` (проверка на пустую строку).
- `Freem.Enums`: один тип `InvalidEnumValueException<TEnum>` (обёртка над `Enum.IsDefined`), 6 проектов.
- `Freem.Exceptions`: один тип `UnknownConstantException`, 2 потребителя (`Storage.PostgreSQL`, `Storage.Migrations`).
- `Freem.Clones`: интерфейс `ICloneable<T>`; реализуют 6 доменных и 6 БД-сущностей, вызов `Clone()` — только в 4 тестах
  репозиториев (копия БД-сущности перед изменением).
- `Freem.Converters` (+ `.UnitTests`, 15 файлов): реестр конвертеров на рефлексии (`ConvertersCollection`,
  поиск интерфейса по имени `IConverter`2`, `MethodInfo.Invoke`). Потребитель один — `Storage.PostgreSQL`: имена
  идентификаторов из колонок БД, события ↔ БД-сущности, ошибки БД → исключения.
- `Freem.Reflection`: `ResourceLoader` (`Migrations`), `FieldsExtractor` (`Entities.UnitTests`), `TypeLoader` (тест
  хранилища), `TypeExtensions` (`Converters`, `Identifiers`, `Events.Consumer`).

**Мёртвый или только тестовый код:**

- `Freem.Entities.Events.Consumer` + `Freem.Entities.Events.Local`: реализаций `IEventConsumer` нет ни одной; `Local`
  подключён только в `Entities.UseCases.IntegrationTests` и там эквивалентен `NullEventPublisher`. Хост использует
  `AddNullEventPublishing()`.
- `Freem.Locking.Local` (пустой `EmptyDistributedLocker`) — только `Entities.UseCases.IntegrationTests`.
- `Freem.Sorting.Extensions.EnumerableExtensions` не используется и содержит ошибку (условие `if (iterator.MoveNext())
  return enumerable;` инвертировано, далее `OrderBy` вместо `ThenBy`); используется только `QueryableExtensions`.
- `Freem.Storage.Abstractions.IStorageTransactionExceptionHandler` никем не реализуется и не передаётся.
- `Freem.Entities.UseCases.Plain`: нужен только `Entities.UseCases.IntegrationTests` (sync-обёртки);
  async-`EntitiesPlainExecutors` не используется нигде.

**Мелкие проекты с одним потребителем:**

- `Freem.TextProcessing` (`LinesExtractor`) → только `Freem.Storage.Migrations`; тот → только
  `Entities.Storage.PostgreSQL.Migrations` (подстановка констант в SQL-скрипты триггеров и констрейнтов).
- `Freem.Timeouts` (повтор с задержкой) → только `Freem.Locking.*`.
- `Freem.AutoFixture` (один `ISpecimenBuilder`) → один тестовый проект; `Freem.Configurations` (чтение JSON-конфига) →
  3 тестовых проекта.

**`Freem.Storage.*` против `Utilities.Storage.Transactions`:** интерфейсы и `StorageTransactionRunner` совпадают почти
дословно; отличия Freem — неиспользуемый `IStorageTransactionExceptionHandler` и регистрация `AddStorageTransactions<T>`.
`Utilities.Storage.Transactions.EFCore` 0.0.2 ссылается на `Microsoft.EntityFrameworkCore` `9.0.*`, Freem — на EF Core
10.0.0. В StashIt своя `Stashit.Storage.Transactions` с каталогом ошибок (под `Result`), а не пакет.

**Остаются (аналоги есть в StashIt, использование широкое):** `Freem.Time` (16 проектов; `Stashit.Time`),
`Freem.Sorting` (`Stashit.Sorting`), `Freem.Locking.Abstractions`/`.Redis` (`Stashit.Locking`).

**Переделываются другими этапами, здесь не трогаются:** `Freem.Identifiers` (+ `.UnitTests`), `IEntityIdentifier` в
`Freem.Entities.Abstractions`, JSON-конвертеры `Freem.Entities.Serialization.Json`/`Freem.Time.Serialization.Json` —
этап 4; `Freem.Results`, `Freem.UseCases`, `.Abstractions`, `.Contracts.Abstractions`, `.Types`,
`Freem.Entities.UseCases.Plain` — этап 5; `Freem.Web.Api.Public.SyncClient` — этап 6.

## Один-два иллюстративных примера (не исчерпывающий список!)

- `Nickname` строит `ValidatorBuilder<string>` ради одной проверки `!string.IsNullOrWhiteSpace(value)`; замена —
  `ArgumentException.ThrowIfNullOrWhiteSpace`-подобная проверка в конструкторе.
- `TestContext` в `Entities.UseCases.IntegrationTests` вызывает `.AddLocalEventsProcessing()` без единого потребителя.

## Направление решения (не пошаговая реализация)

Предложение по каждому пункту (решает пользователь):

| # | Что | Предложение |
|---|-----|-------------|
| 1 | `Freem.DependencyInjection.Microsoft` | удалить; стандартный `ServiceCollection`, явные регистрации |
| 2 | `Freem.EFCore.Extensions` | удалить; стандартные `ApplyConfiguration(new ...)`, `Entry(e).State` |
| 3 | `Freem.Validation` | удалить; проверка в конструкторе `Nickname` |
| 4 | `Freem.Enums`, `Freem.Exceptions` | удалить; стандартные `ArgumentOutOfRangeException`/`InvalidOperationException` |
| 5 | `Freem.Clones` | удалить; в тестах копия БД-сущности создаётся явно |
| 6 | `Freem.Converters` (+ тесты) | удалить; явный маппинг (`switch`) в `Storage.PostgreSQL`; имена идентификаторов — согласовать с этапом 4 |
| 7 | `Freem.Reflection` | удалить после 5–6; оставшиеся 3–4 вызова перенести к потребителям или заменить стандартными |
| 8 | `Events.Consumer` + `Events.Local` | удалить (мёртвый код; подход с `NullEventPublisher` не меняется) |
| 9 | `Freem.Locking.Local` | перенести пустой locker в тестовый проект |
| 10 | `Sorting.EnumerableExtensions`, `IStorageTransactionExceptionHandler` | удалить |
| 11 | `TextProcessing` + `Storage.Migrations` | слить в `Entities.Storage.PostgreSQL.Migrations` |
| 12 | `Freem.Timeouts` | слить в `Freem.Locking.Abstractions` |
| 13 | `Freem.AutoFixture`, `Freem.Configurations` | `AutoFixture`: перенести класс в его единственный тестовый проект; `Configurations`: удалить, чтение конфига — стандартным `ConfigurationBuilder` в тестовых проектах |
| 14 | `Freem.Storage.*` → `Utilities.Storage.Transactions(.EFCore)` | открытый вопрос, см. ниже |

Открытые вопросы:

- **Транзакции (п. 14).** Варианты: (а) перейти на пакет, предварительно выпустив `Utilities.Storage.Transactions.EFCore`
  на EF Core 10 в `CSharpUtilities` (новая версия и тег); (б) оставить `Freem.Storage.*` до этапа 5: при переходе на
  `Result<TValue, TError>` раннер, возможно, станет результатным, как `Stashit.Storage.Transactions`, и пакет всё равно
  не подойдёт. Рекомендую (б): решить вместе с этапом 5, здесь только удалить неиспользуемый обработчик (п. 10).
- **Неиспользуемые `PackageReference` и публичные типы внутри крупных проектов** (`Freem.Entities`,
  `Storage.PostgreSQL`, `Web.Api.Public`) не проверялись: поиск по всем публичным членам дорогой, а эти проекты и так
  перерабатываются этапами 3–6. Предлагаю не включать в этап.

## Явно не входит в этот файл (пока)

Проекты и типы, переделываемые этапами 4–6 (список выше); переименования (этап 7); правка `Freem.Time`/`Freem.Sorting`/
`Freem.Locking` сверх пунктов таблицы.

## Критерий готовности

- По каждому пункту таблицы есть решение пользователя; принятые удаления и слияния выполнены, проекты убраны из
  `Freem.sln` и с диска, ссылок на них в `.csproj`, `Dockerfile` и `docker/` нет.
- `dotnet build Freem.sln` — 0 ошибок; unit-тесты зелёные.
- `docker/test` зелёный (запускает пользователь): затронуты тесты хранилища и use case'ов.
- Схема БД и поведение API не меняются.

## Решения пользователя (2026-10-18)

- Пункты 1–13 принимаются; в п. 13 `Freem.Configurations` удаляется сразу (правка по просьбе пользователя).
- П. 14: `Freem.Storage.*` пока остаётся.
- Неиспользуемые `PackageReference` и публичные типы крупных проектов не проверяются.

## План реализации

Каждый шаг: `dotnet build Freem.sln` — 0 ошибок, unit-тесты зелёные.

1. Мёртвый код: `Sorting.EnumerableExtensions`, `IStorageTransactionExceptionHandler`; `Events.Consumer` и
   `Events.Local` (тесты use case'ов — на `AddNullEventPublishing`); пустой locker из `Locking.Local` — в тестовый проект.
2. Тестовые и DI-обёртки: `DependencyInjection.Microsoft`, `EFCore.Extensions`, `Configurations`, `AutoFixture`.
3. Мелкие доменные: `Validation`, `Enums`, `Exceptions`, `Clones`.
4. `Converters` (+ тесты) → явный маппинг в `Storage.PostgreSQL`; затем `Reflection`.
5. Слияния: `TextProcessing` + `Storage.Migrations` → `Entities.Storage.PostgreSQL.Migrations`; `Timeouts` →
   `Locking.Abstractions`. Проверить `Dockerfile` миграций и API.
6. Итог: поиск ссылок на удалённые проекты по `source` и `docker/`; `docker/test` запускает пользователь.

## Статус и изменения

- 2026-10-18 (Claude Code, claude-opus-5-5): исследование согласовано пользователем (решения выше), план реализации
  записан; шаги 1–6 выполнены, пункты 1–13 применены; `.csproj` в `source` — 37 (было 54).
  `dotnet build Freem.sln` — 0 ошибок, unit-тесты зелёные (6 проектов). Схема БД и API не менялись: в миграциях
  изменены только `using` в двух `Instances/*.cs`, `Designer`-файлы и снимок модели не тронуты; в API — только ссылки
  в `.csproj`. `docker/test` зелёный у пользователя (2026-10-19); не закоммичено. Расхождения с планом:
  - п. 1: use case'ы зависят от базовых интерфейсов репозиториев (`ICreateRepository<…>`, `ISearchByIdRepository<…>`
    и др.), поэтому регистрация «реализация + все её интерфейсы» нужна; вместо явного списка она оставлена приватным
    хелпером `TryAddRepository<T>` в DI `Storage.PostgreSQL`. Переход на один интерфейс репозитория на агрегат (как в
    StashIt) — вопрос этапа 3;
  - `Freem.Converters.UnitTests` удалён на шаге 2, а не 4 (использовал `Services.Resolve`);
  - `Locking.Redis.UnitTests` получил явный `PackageReference` на `Microsoft.Extensions.DependencyInjection` (раньше
    приходил транзитивно через удалённый проект);
  - `Clone()` доменных сущностей не вызывался нигде и удалён; 4 используемых `Clone()` БД-сущностей перенесены в тесты
    хранилища как `Copy()` (`DatabaseEntityCopyExtensions`);
  - типы исключений: недопустимое значение enum — `ArgumentOutOfRangeException`, пустой `Nickname` —
    `ArgumentException`, неизвестный код триггера и неизвестная константа SQL-скрипта — `InvalidOperationException`;
    прежние исключения нигде не перехватывались;
  - в `CLAUDE.md` обновлены число проектов и строки `Events`/`Locking` в "Repository Layout".

  Независимая проверка отдельным агентом: APPROVE WITH NOTES, блокирующих замечаний нет. По решению пользователя
  исправлены: пустые `<ItemGroup>` в пяти `.csproj`, скобки у `foreach` в `TryAddRepository<T>`, отметка о проверке
  схемы БД и API, статус этапа сведён в одну запись, число проектов в фактах исправлено (было ошибочно 52). Замечание
  о слабой проверке `Assert.IsNotType<InvalidOperationException>` в тесте конвертера триггеров оставлено как
  незначительное.
- 2026-10-19 (Claude Code, claude-opus-5-5): этап закрыт пользователем; реализация закоммичена (`96eb914`).
