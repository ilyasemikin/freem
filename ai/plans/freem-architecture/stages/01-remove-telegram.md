# Этап: Удаление Telegram

Автор: Claude Code, claude-sonnet-5-5, 2026-10-18

См. общий контекст и место этого этапа: `ai/plans/freem-architecture/general/roadmap.md` (этап 1).

## Зачем именно так

Интеграция с Telegram — заготовка без использования: решение пользователя убрать её в рамках плана. Выполняется
первым, пока слои не разошлись по этапам 3–7: меньше типов переносить и переименовывать.

## Зафиксированные факты

Проверено 2026-10-18 поиском `telegram` (без учёта регистра) по `source` и `ai/plans/`.

- Домен (`Freem.Entities`): `UserTelegramIntegration`, свойство `User.TelegramIntegration`, метод
  `User.BuildTelegramIntegrationChangedEvent`, событие `UserTelegramIntegrationChangedEvent` (папка
  `Users/Events/TelegramIntegrationChanged`), константа `UserEventActions.TelegramIntegrationChanged` (входит в
  `UserEventActions.All`), ветка в `EventsFactory`.
- Хранилище (`Freem.Entities.Storage.PostgreSQL`): `UserTelegramIntegrationEntity`, его конфигурация,
  `DbSet UserTelegramIntegrations`, регистрация в `ModelBuilderExtensions`, навигация
  `UserEntity.TelegramIntegration` и связь в `UserEntityTypeConfiguration`, блок `EntitiesNames.UserTelegramIntegration`.
  Check-констрейнт `events_entity_name_check` строится из `UserEventActions.All`, поэтому значение
  `telegram_integration_changed` попадает в него.
- Миграции: таблица `user_telegram_integrations` создана миграцией `20250101164038`; в последней миграции
  (`20261017235202_RemovePasswordCredentialsAndSignInEvents`) констрейнт событий ещё содержит
  `telegram_integration_changed`. Значит, нужна новая миграция: `DROP TABLE` + пересоздание констрейнта + удаление
  уже записанных событий с этим действием (иначе новый констрейнт не создастся на существующих данных).
- Тесты: `DatabaseEntitiesFactory` (`.Without(e => e.TelegramIntegration)`) и `EventsRepositoryTests`
  (строка с `UserTelegramIntegrationChangedEvent`) в `Freem.Entities.Storage.PostgreSQL.IntegrationTests`.
- Фронтенд, API, use case'ы: упоминаний нет. В `tasks.md` и в планах первого плана упоминания есть (история).

## Один-два иллюстративных примера (не исчерпывающий список!)

- `UserEventActions.All` сейчас: `registered`, `settings_changed`, `telegram_integration_changed` → останутся первые два.
- Образец миграции с таблицей, констрейнтом и чисткой событий — `20261017235202_RemovePasswordCredentialsAndSignInEvents`.

## Направление решения (не пошаговая реализация)

Удалить все типы и ссылки выше; новая миграция по образцу последней (удаление таблицы, чистка событий, пересоздание
констрейнта; `Down` возвращает таблицу и значение). Старые миграции не трогаем. SQL миграции показать пользователю до
применения (решение roadmap). Открытых вопросов нет.

## Явно не входит в этот файл (пока)

Правка исторических упоминаний в `tasks.md` и закрытых планах первого плана.

## Критерий готовности

- `rg -i telegram source` вне старых миграций, их `Designer`-файлов и снимка модели ничего не находит.
- `dotnet build Freem.sln` — 0 ошибок; unit-тесты зелёные.
- Миграция сгенерирована, SQL показан и одобрен пользователем; `docker/test` зелёный (интеграционные тесты
  хранилища применяют миграции к чистой БД).

## План реализации

1. Домен: удалить `UserTelegramIntegration`, `UserTelegramIntegrationChangedEvent`, константу, свойство и метод в `User`,
   ветку `EventsFactory`. Проверка: `dotnet build Freem.Entities`.
2. Хранилище и тесты: удалить сущность, конфигурацию, `DbSet`, связь, блок `EntitiesNames`, правки в тестах.
   Проверка: `dotnet build Freem.sln` — 0 ошибок.
3. Миграция: `dotnet ef migrations add RemoveTelegramIntegration`, показать SQL пользователю и дождаться одобрения.
4. Проверка: `rg -i telegram` по критерию, unit-тесты; `docker/test` запускает пользователь.

## Статус и изменения

- 2026-10-18 (Claude Code, claude-sonnet-5-5): исследование записано, ждёт согласования направления и критерия готовности; реализация не начата.
- 2026-10-18 (Claude Code, claude-sonnet-5-5): направление и критерий согласованы пользователем; план реализации записан. Шаги 1–2 выполнены (код, хранилище, тесты очищены от Telegram), шаг 3: сгенерирована миграция `RemoveTelegramIntegration` (в `Up` вручную добавлен `DELETE` событий `telegram_integration_changed` перед пересозданием констрейнта). `dotnet build Freem.sln` — 0/0, unit-тесты зелёные, `telegram` остался только в старых миграциях и в имени новой. SQL ждёт одобрения пользователя; `docker/test` запускает пользователь.
- 2026-10-18 (Claude Code, claude-sonnet-5-5): SQL миграции одобрен, `docker/test` зелёный у пользователя; этап закрыт пользователем; реализация закоммичена (`8994cf8`).
