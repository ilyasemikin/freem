# Этап: Domain и Domain.Entities

Автор: Claude Code, claude-opus-5-5, 2026-10-19

См. общий контекст и место этого этапа: `ai/plans/freem-architecture/general/roadmap.md` (этап 3).

## Зачем именно так

Решение пользователя (2026-10-19): переходить ровно на подход StashIt. В StashIt домен разложен на `Domain`
(идентификаторы, value objects, события, каталог ошибок), `Domain.Entities` (агрегаты, сравнители) и `Domain.Storage`
(по одному интерфейсу репозитория на агрегат + фильтры); агрегаты ссылаются друг на друга только по Id. Во Freem всё
это сейчас лежит в `Freem.Entities` + `Freem.Entities.Abstractions` + `Freem.Entities.Storage.Abstractions`, а
use case'ы зависят от мелких базовых интерфейсов репозиториев. Разделение делается до этапов 4–5, чтобы
идентификаторы и use case'ы переписывались уже на своих местах.

## Зафиксированные факты

Проверено 2026-10-19 чтением кода Freem и StashIt (`~/Sources/StashIt/CLAUDE.md`, раздел "Layer Structure";
проекты `Core/Stashit.Core.Domain*`).

**StashIt:**

- Правило размещения (`CLAUDE.md` StashIt): тип, который только *называет* другой агрегат по Id или работает с
  сырыми данными, — в `Domain`; тип, чья сигнатура лезет во внутренности агрегата, — в `Domain.Entities`.
  `Domain.Entities` ссылается на `Domain`, обратное невозможно.
- `Domain`: Id и value objects по папкам агрегатов (`Tags/TagId.cs`, `Tags/TagName.cs`), события (`Events/...`),
  `DomainErrorCatalog`. `Domain.Entities`: агрегаты (`Tags/Tag.cs` с вложенным `Builder`) и `Comparers/`.
  Агрегат хранит только Id других агрегатов (`Tag.GroupId`, `Tag.UserId`).
- `Domain.Storage`: `Repositories/<Агрегат>/I<Агрегат>Repository.cs` (единственное число) и фильтры
  (`TagByUserFilter`, `TagFilter`), `RepositoryContext` (необязательная `IStorageTransaction`). Методы репозитория
  принимают `RepositoryContext` и возвращают `Result<Error>` / `Result<T, Error>` / `Result<AsyncBatch<T>, Error>`
  (`Utilities.Primitives`, `Utilities.Errors`). Регистрация — `AddTransient<ITagRepository, MongoTagRepository>()`.
- `Api.Public.Contracts` ссылается только на `Domain`; `Domain.UseCases.Contracts` — на `Domain` и `Domain.Entities`.

**Freem:**

- `Freem.Entities` (61 файл): агрегаты `Activity`, `Record`, `RunningRecord`, `Tag`, `User`, `UserSettings`;
  идентификаторы (`*/Identifiers/*Identifier.cs`), value objects (`*/Models/`: `ActivityName`, `ActivityStatus`,
  `RecordName`, `RecordDescription`, `TagName`, `Nickname`, `DayUtcOffset`), события и `*EventActions`,
  `EventsFactory`, `EntityIdentifierFactory`/`EntityIdentifierNameProvider`, сравнители, статистика
  (`TimeStatistics.Calculate` принимает `IEnumerable<Record>`), коллекции связей (`_Common/Relations`).
- `Freem.Entities.Abstractions`: `IEntity`, `IEntityIdentifier`/`IMultipleEntityIdentifier`, база событий
  (`EntityEvent`, `IEntityEvent`, `EventIdentifier`, `EventAction`), абстракции связей (`IEntityRelation`,
  `IReadOnlyRelatedEntitiesCollection` и др.).
- Агрегаты хранят связанные агрегаты целиком: `Record.Activities` — `RelatedActivitiesCollection` (Id + загруженные
  `Activity`), так же `Tags` у `Record`, `RunningRecord`, `Activity`. Хранилище загружает их через `Include`
  (`RecordsRepository`, `ActivitiesRepository`, `RunningRecordsRepository`) и маппит в `RelatedCollectionMapper`.
  Загруженные сущности (`.Entities`, `TryGet`) не читает никто вне самих коллекций: API отдаёт только Id
  (`RecordResponse.Activities` — `IReadOnlyRelatedEntitiesIdentifiersCollection<ActivityIdentifier>`), запись связей в
  БД использует только `Identifiers`.
- `Freem.Entities.Storage.Abstractions`: базовые интерфейсы (`IWriteRepository`, `ICreateRepository`,
  `ISearchByIdRepository`, `ISearchByMultipleIdsRepository`, `IMultipleSearchByFilterRepository`), из которых собраны
  `IActivitiesRepository`, `IRecordsRepository` и др.; фильтры, сортировка, исключения хранилища,
  `SearchEntitiesAsyncResult`. Из 33 файлов use case'ов/producer'а с зависимостью от репозитория 19 берут базовый
  интерфейс, а не репозиторий агрегата. Ошибки — исключениями (`NotFoundException`, `DuplicateKeyStorageException`).
- Контракты API (`Freem.Web.Api.Public.Contracts`) используют из домена идентификаторы, value objects и коллекции Id
  связей, агрегаты — нет.

## Один-два иллюстративных примера (не исчерпывающий список!)

- `TagIdentifier`, `TagName`, `TagCreatedEvent` → `Domain`; `Tag`, `TagEqualityComparer` → `Domain.Entities`;
  `ITagsRepository` (+ `TagsByUserFilter`, `TagsFilter`) → `Domain.Storage` как `ITagRepository`.
- `GetRunningRecordUseCase` зависит от `ISearchByIdRepository<RunningRecord, RunningRecordIdentifier>` → будет
  зависеть от `IRunningRecordRepository`.

## Направление решения (не пошаговая реализация)

- Новые проекты `Domain`, `Domain.Entities`, `Domain.Storage` (+ unit-тесты), разложение типов по правилу StashIt;
  `Freem.Entities`, `Freem.Entities.Abstractions`, `Freem.Entities.Storage.Abstractions` перестают существовать.
  Нужное из `Freem.Entities.Abstractions` переходит в `Domain`.
- Агрегаты ссылаются на другие агрегаты только по Id: коллекции связей становятся коллекциями Id (ограничения на
  количество сохраняются), загрузка связанных сущностей через `Include` убирается. Схема БД и API не меняются.
- Один интерфейс репозитория на агрегат в `Domain.Storage`, базовые generic-интерфейсы удаляются; use case'ы
  зависят от `I<Агрегат>Repository`; DI — явная пара «интерфейс → реализация», хелпер `TryAddRepository<T>`
  удаляется.
- Ссылки контрактов как в StashIt: `Api.Public.Contracts` — только на `Domain`.

Решения пользователя (2026-10-19):

1. **Объём подхода к репозиториям — вариант (а).** Здесь — структура: один интерфейс на агрегат, имена в
   единственном числе как в StashIt (`ITagRepository`, `TagByUserFilter`); сигнатуры остаются нынешними (исключения,
   `Task<T>`). `RepositoryContext`, `Result<…, Error>` и `AsyncBatch<T>` — на этапе 5 вместе с переходом use case'ов на
   `Result` и решением по `Freem.Storage.*`.
2. **Имена новых проектов — сразу итоговые:** `Freem.Domain`, `Freem.Domain.Entities`, `Freem.Domain.Storage`,
   `Freem.Domain.UnitTests`; переименование остальных проектов — этап 7.
3. **С этапом 4 не объединяем:** идентификаторы переносятся в `Domain` как есть.

## Явно не входит в этот файл (пока)

Переписывание идентификаторов (этап 4), `Result`/`RepositoryContext`/типизированные ошибки (этап 5), переименование
проектов вне `Domain*` и реализаций репозиториев (`ActivitiesRepository` и др.; этап 7).

## Критерий готовности

- `Freem.Entities`, `Freem.Entities.Abstractions`, `Freem.Entities.Storage.Abstractions` удалены; типы разложены по
  `Domain`/`Domain.Entities`/`Domain.Storage` по правилу StashIt; `Domain` не ссылается на `Domain.Entities`,
  `Api.Public.Contracts` ссылается только на `Domain` из доменных проектов.
- Агрегаты не содержат других агрегатов, только Id; базовых generic-интерфейсов репозиториев нет, use case'ы
  зависят от `I<Агрегат>Repository`.
- `dotnet build Freem.sln` — 0 ошибок, unit-тесты зелёные; `docker/test` зелёный (запускает пользователь).
- Схема БД (миграции, снимок модели) и контракты API не меняются.

## План реализации

Каждый шаг: `dotnet build Freem.sln` — 0 ошибок, unit-тесты зелёные.

1. Ссылки по Id (пока в старых проектах): коллекции связей агрегатов → коллекции Id; удалить entity-версии коллекций
   и абстракции связей (`IEntityRelation`, `IReadOnlyRelatedEntitiesCollection` и др.); убрать `Include` и
   `RelatedCollectionMapper` в хранилище; запись связей — по коллекции Id.
2. Репозитории (пока в старых проектах): базовые generic-интерфейсы влить в интерфейсы агрегатов и удалить;
   переименовать интерфейсы и фильтры в единственное число; use case'ы и producer — на `I<Агрегат>Repository`;
   DI — явные пары, без `TryAddRepository<T>`.
3. Разделение проектов: создать `Freem.Domain`, `Freem.Domain.Entities`, `Freem.Domain.Storage`; перенести типы по
   правилу StashIt (корневое пространство имён меняется, вложенные папки сохраняются; репозитории и их фильтры — по
   папкам агрегатов `Repositories/<Агрегат>/`); удалить `Freem.Entities`, `Freem.Entities.Abstractions`,
   `Freem.Entities.Storage.Abstractions`; `Freem.Entities.UnitTests` → `Freem.Domain.UnitTests`; ссылки контрактов —
   как в StashIt.
4. Итог: поиск ссылок на удалённые проекты по `source`, `docker/`, `CLAUDE.md`; проверка, что миграции и
   контракты API не изменились; `docker/test` запускает пользователь.

## Статус и изменения

- 2026-10-19 (Claude Code, claude-opus-5-5): исследование согласовано пользователем (решения выше); план реализации
  записан, реализация начата.
- 2026-10-19 (Claude Code, claude-opus-5-5): шаги 1–4 выполнены. `Freem.Entities`, `Freem.Entities.Abstractions`,
  `Freem.Entities.Storage.Abstractions`, `Freem.Entities.UnitTests` → `Freem.Domain`, `Freem.Domain.Entities`,
  `Freem.Domain.Storage`, `Freem.Domain.UnitTests` (каталоги перенесены через `git mv`, записи в `Freem.sln` сохранили
  GUID и папки). `Freem.Domain` не ссылается на `Domain.Entities`; `Api.Public.Contracts`, клиенты, `Serialization.Json`,
  `Events.Producer`, `Migrations` ссылаются только на `Domain`; `Domain.Entities` нужен `UseCases*`,
  `Storage.PostgreSQL(.IntegrationTests)`, `Web.Api.Public`. `dotnet build Freem.sln` — 0 предупреждений, 0 ошибок,
  unit-тесты зелёные (6 проектов); миграции не изменились, в контрактах API — только `using` и имя коллекции Id (формат
  JSON тот же). `docker/test` не запускался, ждёт пользователя. Изменения проиндексированы (`git add`, нужно для
  `git mv`), не закоммичены. Расхождения с планом и решения по ходу:
  - шаг 1: `Include` в хранилище оставлен — в модели EF связи многие-ко-многим читаются только через навигации, иначе
    не получить Id связанных агрегатов; `RelatedCollectionMapper` оставлен и маппит только Id. Агрегаты используют
    `RelatedActivitiesIdentifiersCollection`/`RelatedTagsIdentifiersCollection` (в них перенесены ограничения на
    количество и `Update`); тесты entity-коллекции заменены тестами коллекции Id;
  - константы `EntityName` агрегатов перенесены в `Freem.Domain.EntityNames`: их использовали
    `EntityIdentifierFactory`/`EntityIdentifierNameProvider`, которые относятся к `Domain`;
  - `TimeStatistics` используется в DTO API, поэтому по правилу StashIt (не переносить тип целиком ради одной
    операции) данные остались в `Domain`, а расчёт по записям вынесен в `Domain.Entities` как
    `TimeStatisticsCalculator`; неиспользуемый приватный `CalculateAsync` удалён;
  - интерфейсы коллекций связей объединены с классами в пространстве имён `Freem.Domain.Relations.Collections`;
  - в `Domain.Storage` интерфейсы репозиториев и их фильтры лежат в `Repositories/<Агрегат>/`, пары Id и поля
    сортировки остались в `Models/`; DI разделён: генераторы, имена Id и фабрика событий — в `Domain`, сравнители — в
    `Domain.Entities`;
  - лишние `using` в затронутых файлах удалены через `dotnet format` (IDE0005, временный `.editorconfig` удалён);
  - в `CLAUDE.md` обновлены пример scope коммита и строки `Domain*` в "Repository Layout";
  - находка вне этапа (не исправлялась): список записей (`RecordsRepository`, выборка по пользователю) делает
    `Include` только для `Activities`, поэтому у записей в списке, вероятно, пустые теги.
- 2026-10-19 (Claude Code, claude-opus-5-5): `docker/test` у пользователя: упал
  `EventsRepositoryTests.Events_ShouldAllProcessed` — тест загружал сборку по строковому имени `Freem.Entities`.
  Исправлено: сборка берётся как `typeof(IEntityEvent<,>).Assembly`, абстрактные типы (базовый `EntityEvent<,>`,
  теперь лежащий в той же сборке) отфильтрованы; других строковых ссылок на старые имена сборок нет. По решению
  пользователя исправлен и баг с тегами: выборки записей по пользователю и по периоду теперь делают `Include` тегов;
  добавлены регрессионные тесты `FindAsyncByUser_ShouldReturnRelatedIdentifiers` и
  `FindAsyncByPeriod_ShouldReturnRelatedIdentifiers` (проверяют Id активностей и тегов). Попутно разнесены по строкам два
  `using`, склеенные в одной строке ещё до этапа (`RecordByPeriodFilter`). Сборка — 0 предупреждений, 0 ошибок,
  unit-тесты зелёные; повторный `docker/test` ждёт пользователя.
- 2026-10-19 (Claude Code, claude-opus-5-5): повторный `docker/test` зелёный у пользователя; критерий готовности
  выполнен, ждёт закрытия пользователем, не закоммичено.
- 2026-10-19 (Claude Code, claude-opus-5-5): этап закрыт пользователем (независимая проверка не проводилась по решению
  пользователя); реализация закоммичена (`64e0cf3`).
