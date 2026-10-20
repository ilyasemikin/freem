# Events: приведение к схеме StashIt

Автор: Claude Code, claude-sonnet-5-5, 2026-10-20

Статус: **закрыт (2026-10-20, Claude Code, claude-sonnet-5-5): реализация завершена, интеграционные тесты в `docker/test` прошли**

## Зачем

Подсистема событий Freem и StashIt решает одну задачу (история действий пользователя), но устроена по-разному.
Во Freem событие — «голый» факт: `EntityEvent<TEntityId, TUserId>` с `EntityId`, `UserEntityId` и строкой `Action`;
данных об изменении нет, `AdditionalData` в БД не заполняется, времени события (`OccurredAt`) нет, наружу в API уходит
доменный `IEntityEvent<Id, UserId>` без DTO. В StashIt событие типизировано и несёт снимок состояния
(`TagCreatedEvent { TagId, Name, Color, GroupId }`), тип задаётся `EventType` (`"tags.created"`), ссылка на сущность —
`EntityRef(EntityType, Value)`, атрибуция — `EventEnvelope(UserId, OccurredAt, Event)`, хранится отдельная модель
`Event` с `EventPayload`, в API отдаётся `EventDto`, есть `Get`, список фильтруется по типу сущности, ссылке и периоду.

Цель — привести Freem к той же схеме, чтобы два проекта развивались одинаково, а общие абстракции жили в
`CSharpUtilities`. Совместимость со старой схемой не нужна.

## Принятые решения

- **Гарантия записи остаётся жёсткой** (решение пользователя): событие пишется в той же транзакции, что и операция,
  ошибка записи откатывает операцию. Схема StashIt (запись после коммита, вне транзакции, ошибки в лог) не
  переносится. Всё остальное (модель, хранение, API) приводится к StashIt.
- **В `CSharpUtilities` выносится только `INamingPolicy`** (пакет `Utilities.Naming`). Требование для агента
  CSharpUtilities: `csharputilities-events-requirement.md` (передаётся пользователем). Freem берёт пакет из
  `_Src/Freem/packages/`, как остальные `Utilities.*`.
- **`Stashit.Events` не переносится в пакет, абстракции событий живут во Freem** (решение пользователя): подход
  StashIt в таком виде нельзя считать переносимым, проекты могут разойтись по концепциям. `EventType` и `IEvent`
  добавляются в `Freem.Domain/Events/` рядом с уже существующим `EventId` (который остаётся Freem-овским, наследником
  `Id`). Отдельного проекта под них нет.
- **Данные не мигрируем**: таблицы `events` пусты, старые миграции остаются в истории, новая миграция пересоздаёт таблицу.
- `EventPayload`, `EntityEvent`, `EntityRef`, `EntityType`, `EventEnvelope`, `EventTypes` остаются доменным кодом Freem,
  как в StashIt.

## Целевая схема

Домен (`Freem.Domain/Events/`):

- `EntityType` (enum: `Activity`, `Record`, `RunningRecord`, `Tag`, `User`), `EntityRef(EntityType, string Value)`,
  `EventTypes` (`"activities.created"`, `"activities.updated"`, `"activities.removed"`, `"activities.archived"`,
  `"activities.unarchived"`, `"records.created|updated|removed"`, `"runningRecords.started|stopped|updated|removed"`,
  `"tags.created|updated|removed"`, `"users.registered"`, `"users.settingsChanged"`; точные строки уточняются на
  этапе 2), `EventEnvelope`, `EventPayload`.
- `EntityEvent : IEvent` (`Id`, `Type`, `EntityRef`) и типизированные события по сущностям: поля — снимок после
  изменения (для `Removed` — только идентификатор). Состав полей каждого события определяется по агрегатам и
  фиксируется на этапе 2.
- Удаляются: `EntityEvent<,>`, `IEntityEvent`, `EventAction`, `EventsFactory`, `*EventActions`, `EntityIdNameProvider` в части
  событий (если больше нигде не используется).

Хранение:

- Модель `Freem.Domain.Entities.Events.Event` (`Id`, `Type`, `UserId`, `OccurredAt`, `EntityRef`, `Payload`).
- `IEventRepository`: `CreateAsync(EventEnvelope)`, `FindByIdAsync(EventId)`, `FindAsync(EventByUserFilter)`;
  фильтр `EventByUserFilter` (Offset/Limit по принятой во Freem схеме лимитов, `EntityType`, `EntityRef`, `From`, `To`).
  Сортировка: `OccurredAt` по убыванию, затем `Id` по убыванию.
- `EventPayloadSerializer` (событие ⇄ `EventPayload`) в `Freem.Domain.Storage`, `EventPayloadJsonConverter` в
  `Freem.Domain.Serialization.Json`.
- PostgreSQL: `EventEntity` с колонками `EventType`, `EntityType`, `EntityRef`, `OccurredAt`, `Payload` (`jsonb`);
  индексы под фильтры (`UserId, OccurredAt`, `UserId, EntityType, EntityRef`). Старые триггеры и constraints
  (`events_triggers.psql` и миграции `Events*`) заменяются новой миграцией; проверки соответствия `EntityType` и
  `EntityRef` формату Id — по необходимости.

Публикация:

- Публикация берёт `UserId` из контекста, время — из `IClock`, пишет через репозиторий **в ambient-транзакции** (без
  `RepositoryContext.Default` и без `CancellationToken.None`).
- **Форма публикации — `EventProducer` с `Result<Error>`** (решение пользователя, вариант «б»): `StorageTransactionRunner`
  откатывает транзакцию по `Result` с ошибкой, и это совпадает со стилем ошибок остального кода. Вариант с
  публикатором, бросающим исключение, отвергнут. `IEventPublisher` и `NullEventPublisher` удаляются.
- Проект `Freem.Domain.Events.Producer` упраздняется (события всегда пишутся в БД, второй шаг публикации не нужен);
  `EventProducer` переезжает в слой use cases и принимает фабрику `EntityEvent` (по смыслу текущего `EventFactory`),
  сам ставит `EventId`, время из `IClock` и `UserId` из `HandlerContext`.

API:

- `EventDto { Id, EventType, EntityRef, OccurredAt, Payload }` в `Freem.Api.Public.Contracts`, маппер `Event → EventDto`,
  `GET api/v1/events/{id}` (`GetEvent`), `GET api/v1/events` с фильтрами `entityType`, `entityRef`, `from`, `to`,
  `offset`, `limit`. Ответы и ошибки — через существующий `Result<…, Error>` и `ErrorResponse`.
  Ошибка «событие не найдено» — `DomainErrorCatalog`.
- `Freem.Api.Public.Client` (`EventsClient`): `GetAsync`, `ListAsync` с новыми параметрами.
- `Freem.Web`: проверить, использует ли фронтенд события (на момент составления плана — не проверялось).

## Этапы

1. **Зависимость.** Получить от агента CSharpUtilities пакет `Utilities.Naming`, положить `.nupkg` в
   `_Src/Freem/packages/`, подключить `PackageReference`. Проверить использование событий в `Freem.Web`.
2. **Домен.** `EventType`, `IEvent`, `EntityType`, `EntityRef`, `EventTypes`, `EventEnvelope`, `EventPayload`, типизированные события для
   activity, record, running record, tag, user; удалить старую модель (`EntityEvent<,>`, `EventAction`, `EventsFactory`,
   `*EventActions`). Переписать `Build*Event` методы агрегатов под новые события. Юнит-тесты на события и `EventPayload`.
3. **Хранилище.** Модель `Event`, `IEventRepository`, `EventByUserFilter`, `EventPayloadSerializer`,
   PostgreSQL-сущность и конфигурация, новая миграция (пересоздание `events`, индексы, замена триггеров), репозиторий и
   конвертеры. Переписать `EventsRepositoryTests` и `EventsConstraintTriggerTests`; сериализатор — юнит-тестами
   (все типы событий, round-trip).
4. **Use cases.** Публикация через `EventProducer` в 17 use case'ах (`Activities`, `Records`, `RunningRecords`, `Tags`,
   `Users`), `GetEvent`/`ListEvents` по схеме StashIt, упразднение `Freem.Domain.Events.Producer`, DI.
   Интеграционные тесты use cases: события пишутся в той же транзакции, ошибка записи откатывает операцию.
5. **API, клиент, фронтенд.** `EventDto`, маппер, `GetEvent`, новые фильтры, `EventsClient`, интеграционные тесты API;
   при необходимости правки `Freem.Web`. Обновить документацию (`CLAUDE.md`, если меняется структура проектов).

Каждый этап — отдельный коммит или серия коммитов после ревью пользователя (правила коммитов — `CLAUDE.md`).
Порядок строгий: 1 → 2 → 3 → 4 → 5, промежуточные состояния между 2 и 4 не обязаны собираться целиком, если
ветка не вливается до конца этапа 4.

## Критерии готовности

- Все события Freem типизированы и несут снимок состояния; `OccurredAt` ставится из `IClock`.
- Запись события и операция — одна транзакция; тест на откат есть.
- `dotnet build Freem.sln` и юнит-тесты зелёные; интеграционные тесты в `docker/test` проходят.
- Контракт API событий совпадает по форме с StashIt (`EventDto`, `GetEvent`, фильтры).
- В решении нет `EventAction`, `EventsFactory`, `IEntityEvent`, `Freem.Domain.Events.Producer`.
