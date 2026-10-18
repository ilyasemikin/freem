# Этап: Удаление Kafka

Автор: Claude Code, claude-sonnet-5-5, 2026-10-18

См. общий контекст и место этого этапа: `plans/freem-revival/general/roadmap.md` (этап 3).

## Зачем именно так

Kafka в проекте нужна только как транспорт для публикации событий; потребителей событий в коде нет. При этом
хост без доступного брокера блокирует запрос внутри use case'а. Брокер для личного трекера — лишняя
инфраструктура (в compose-окружениях этапа 5 его не будет). Журнал событий в БД (`IEventsRepository`) остаётся,
меняется только «публикация вовне».

## Зафиксированные факты

Проверено 2026-10-18 чтением кода (сборка и тесты в рамках исследования не запускались).

- Kafka-специфичный код: проекты `Freem.Entities.Events.Producer.Kafka` и `Freem.Entities.Events.Consumer.Kafka`
  (оба в решении, единственные потребители пакета `Confluent.Kafka` 2.16.0); в хосте — `Program.cs:68`
  (`AddKafkaEventProduction`), `Configuration/Instances/KafkaConfiguration.cs`, поле `Kafka` в
  `CompositeConfiguration`, `GetKafkaProducerConfiguration`, секция `Kafka` в `appsettings.json`,
  `appsettings.Development.json`, `FunctionalTests/configuration.json` и `configuration.example.json`;
  ссылка на `Producer.Kafka` в `Freem.Web.Api.Public.csproj`. Упоминание в `CLAUDE.md` (раздел Repository Layout).
- Абстракция: `IEventPublisher` (`Events.Producer/Abstractions`), его использует только `EventProducer`, который
  вызывают use case'ы (`RegisterUserUseCase`, `CreateTagUseCase` и др.): сначала `IEventsRepository.CreateAsync`
  (запись в БД), затем `_publisher.PublishAsync`. `EventProducer` регистрируется через `AddEventProduction<T>`,
  которому нужен `IEventPublisher`; без него хост не стартует.
- Реализации `IEventPublisher`: `KafkaEventPublisher` (хост) и `LocalEventPublisher` (в
  `Freem.Entities.Events.Local`; fire-and-forget вызывает `EventConsumersExecutor`; подключается
  `AddLocalEventsProcessing()`, используется только `UseCases.IntegrationTests/Fixtures/TestContext.cs:103`).
- Потребителей нет: ни одной реализации `IEventConsumer<T>` в репозитории. Инфраструктура потребления
  (`Events.Consumer`: `EventConsumersExecutor`, `EventConsumerRunner`, `EventConsumersBuilder`, дескрипторы)
  нигде в хосте не регистрируется; ссылаются на неё `Events.Local`, `Events.Consumer.Kafka` и
  `UseCases.IntegrationTests`.
- Redis (блокировки) от Kafka не зависит: `AddSimpleRedisDistributedLocks` остаётся.

## Один-два иллюстративных примера (не исчерпывающий список!)

- `Program.cs:68`: цепочка `.AddSimpleRedisDistributedLocks(...).AddKafkaEventProduction(...)` — вторая вставка уходит.
- `EventProducer.PublishAsync`: две строки (запись в БД + публикация) — вторая зависит от решения ниже.

## Направление решения (не пошаговая реализация)

Решение пользователя (2026-10-18): публикация и потребление событий как механизм остаются; Kafka заменяется пустой
реализацией.

- Удаляются оба Kafka-проекта, пакет `Confluent.Kafka`, конфигурация Kafka (хост, `appsettings*`,
  `FunctionalTests`), упоминание в `CLAUDE.md`.
- `Events.Producer`, `Events.Consumer`, `Events.Local` сохраняются без изменений.
- В `Events.Producer` добавляется `NullEventPublisher : IEventPublisher` (ничего не делает) и расширение
  регистрации; хост использует его вместо `AddKafkaEventProduction`. Журнал событий в БД продолжает писаться через
  `EventProducer`.
- Потребление остаётся в виде существующей инфраструктуры `Events.Consumer` (хост её не подключает, потребителей
  нет); «пустая реализация» для потребления не добавляется, так как подключение не требуется (подтверждено пользователем).

Что не меняется: схема БД, Redis, use case'ы. Если по ходу всплывёт необходимость менять БД — остановиться и
согласовать.

## Явно не входит в этот файл (пока)

Форма Events в целом — этап 2 плана `plans/freem-architecture`.

## Критерий готовности

- `dotnet build Freem.sln` без ошибок; `dotnet test Freem.sln` — unit-тесты зелёные.
- В репозитории нет упоминаний Kafka (`grep -ri kafka` в коде, конфигурации и `CLAUDE.md`), пакета `Confluent.Kafka`
  и секции `Kafka` в конфигурациях.
- Хост стартует без брокера, события пишутся в журнал (если окружение доступно — проверка запуском по согласованию; иначе фиксируется как
  непроверенное).
- Запись событий в журнал БД продолжает работать (use case'ы по-прежнему вызывают `EventProducer`).

## План реализации

1. В `Events.Producer` добавить `NullEventPublisher` и `AddNullEventPublishing()` (через `AddEventProduction<T>`).
   Проверка: `dotnet build` проекта.
2. В хосте заменить `AddKafkaEventProduction(...)` на `AddNullEventPublishing()`; удалить `KafkaConfiguration`,
   поле `Kafka` в `CompositeConfiguration`, `GetKafkaProducerConfiguration`, ссылку на `Producer.Kafka` в csproj.
   Проверка: `dotnet build Freem.Web.Api.Public`.
3. Удалить проекты `Events.Producer.Kafka` и `Events.Consumer.Kafka` из решения и с диска. Проверка:
   `dotnet build Freem.sln`.
4. Убрать секцию `Kafka` из `appsettings.json`, `appsettings.Development.json`, `FunctionalTests/configuration.json`
   и `configuration.example.json`; поправить `CLAUDE.md`. Проверка: `grep -ri kafka` по репозиторию пуст.
5. Итог: `dotnet build Freem.sln`, `dotnet test Freem.sln`.

## Статус и изменения

- 2026-10-18 (Claude Code, claude-sonnet-5-5): исследование записано.
- 2026-10-18 (Claude Code, claude-sonnet-5-5): пользователь выбрал сохранить публикацию/потребление и заменить Kafka пустой реализацией; направление и план реализации записаны, ожидают одобрения.
- 2026-10-18 (Claude Code, claude-sonnet-5-5): план одобрен пользователем; шаги 1–5 выполнены. Добавлены `NullEventPublisher` и `AddNullEventPublishing()` в `Events.Producer`, хост использует их; Kafka-проекты, пакет, конфигурация и упоминание в `CLAUDE.md` удалены (`grep -ri kafka` вне `plans/` пуст). Отклонение: хосту понадобилась прямая `ProjectReference` на `Events.Producer` (транзитивные ссылки отключены). `dotnet build Freem.sln`: 0 предупреждений, 0 ошибок; unit-тесты зелёные, интеграционные/функциональные падают без БД и Redis (не критерий до этапа 5). Запуск хоста не проверялся. Ожидает проверки и закрытия пользователем, не закоммичено.
- 2026-10-18 (Claude Code, claude-sonnet-5-5): независимая проверка отдельным агентом: APPROVE WITH NOTES (блокирующих нет). Пользователь решил: пустая реализация потребления не нужна (вопрос закрыт); запуск хоста не проверять (критерий «хост стартует без брокера» остаётся непроверенным по решению пользователя); BOM в `ConfigurationManagerExtensions.cs` не возвращать.
- 2026-10-18 (Claude Code, claude-sonnet-5-5): этап 3 закрыт пользователем; реализация закоммичена (00f0875).
