# Data transfer: симметрия импорта и экспорта

Автор: Claude Code, claude-sonnet-5-5, 2026-10-24

Статус: **закрыт (2026-10-24, Claude Code, claude-sonnet-5-5): реализовано, проверено в браузере и независимым ревью; прогон `docker/test` остаётся за пользователем**

## Зачем

Импорт и экспорт (плюс форматы) — две стороны одной области, но в коде они разложены по-разному. В одних слоях
папки `Import`, в других `Imports`; `Export` и `Formats` лежат отдельными папками рядом; в API часть уже
под `DataTransfer` (экспорт, форматы, `DataTransferClient`), а импорты отдельно. Публичные маршруты тоже не
сходятся: `api/v1/imports`, `api/v1/export/{format}`, `api/v1/formats`.

Цель — привести всё к одной схеме. Менять нужно размещение, namespace, имена и маршруты, но не сами сущности:
агрегаты разные (импорт многошаговый и хранит состояние, экспорт без состояния), общей абстракции не вводим.
Выделение в отдельный домен (по образцу `Analytics` в StashIt) пока не делаем, но схема должна позволять
вынести папку `DataTransfer` позже механическим переносом.

## Принятые решения

- **Единый уровень `DataTransfer/{Import,Export,Formats}`** на каждом слое, в том числе в namespace
  (`...DataTransfer.Import`, `...DataTransfer.Export`, `...DataTransfer.Formats`).
- **Единственное число везде**: `Imports` → `Import`, в папках, namespace и типах, где оно встречается.
- **Публичные маршруты под общим префиксом, старые не сохраняем** (приложение не выкатывалось):
  - `api/v1/data-transfer/imports/...` (подмаршруты не меняются: `{importId}/activities`, `/tags`, `/rows`,
    `/apply` и т.д.);
  - `api/v1/data-transfer/exports/{format}`;
  - `api/v1/data-transfer/formats`.
- **Три контроллера** (`ImportController`, `ExportController`, `FormatsController`; имена окончательно
  выравниваются в фазе 4) в одной папке `Freem.Api.Public/Controllers/V1/DataTransfer/`.
- **БД не меняется.** Таблицы `import_documents`, `import_document_*`, `import_row_*` остаются как есть
  (суффикс `document` оставлен по решению пользователя; он согласован с сущностью `ImportDocument`). Миграция
  не нужна. Идентификаторы `import_tag_id` / `import_activity_id` не трогаем: они общие для нескольких
  таблиц и совпадают с доменными `ImportTagId` / `ImportActivityId`.
- **Сущность `ImportDocument` и её Id не переименовываем** — это не часть приборки.
- **Публичные DTO и контракты не меняем по содержанию.** Меняется только расположение и namespace.
- **Web**: меняются URL в клиентах; маршруты страниц браузера и структура `pages/` — в рамках фазы 5,
  если не окажется, что менять их незачем.

## Фазы

Каждая фаза заканчивается зелёной сборкой и тестами; коммиты — только по просьбе пользователя.

1. **Домен и сущности**: `Freem.Domain` (`Import/`, `Formats/`), `Freem.Domain.Entities` (`Import/`, `Export/`),
   `Freem.Domain.Serialization.Json` при необходимости. Юнит-тесты (`Freem.Domain.UnitTests`) повторяют структуру.
2. **Хранилище**: `Freem.Domain.Storage` (`Repositories/Imports` → `DataTransfer/Import`),
   `Freem.Domain.Storage.PostgreSQL` (`Repositories/Imports`), интеграционные тесты репозитория. Схема БД и
   миграции не меняются; проверить, что модель-снимок EF не разошлась (новая миграция не генерируется).
3. **Use cases**: `Freem.Domain.UseCases(.Contracts)` (`Import`, `Export`, `Formats`),
   `Freem.Domain.UseCases.UnitTests` и `.IntegrationTests`, DI-регистрация.
4. **API**: `Freem.Api.Public.UseCases(.Contracts)` (handlers, mappers, request-контракты),
   `Freem.Api.Public.Contracts/V1/{DataTransfer,Imports}` в один `DataTransfer`, контроллеры, model binders и
   формы, **новые маршруты**, C#-клиент `Freem.Api.Public.Client` (`ImportsClient`, `DataTransferClient`),
   `Freem.Api.Public.IntegrationTests`.
5. **Web**: `ImportsClient.ts` и `DataTransferClient.ts` на новые URL; при необходимости выравнивание папок
   `clients/models/*` и `pages/*`; `npm run build` и `npm run lint`.
6. **Документация**: упоминания старых маршрутов в `ai/plans/data-transfer/**`, `CLAUDE.md` (если нужно) и
   проверка тестов в `docker/test`.

## Проверка

- `dotnet build Freem.slnx` и `dotnet test Freem.slnx` (юнит-тесты) после каждой фазы.
- Интеграционные тесты (storage, use cases, API) — в `docker/test`, запускает пользователь.
- Ручной прогон в браузере — только по согласованию (`ai/skills/web-manual-test/SKILL.md`).

## Статус и изменения

- 2026-10-24 (Claude Code, claude-sonnet-5-5): план одобрен пользователем, фазы 1-6 выполнены, не закоммичено.
  Проверка: `dotnet build Freem.slnx` без ошибок и предупреждений; юнит-тесты, интеграционные тесты хранилища,
  use cases и API проходят (API: 125/125 после обновления прямых URL в тестах); `npm run build` и `npm run lint`
  Freem.Web чистые. Миграции и снимок EF не менялись.
  Что вышло за буквальный текст плана:
  - `RequestTypes` (домен и API) перестроены в `RequestTypes.DataTransfer.{Export,Formats,Import}`, строковые
    значения теперь `data-transfer.imports.*`, `data-transfer.exports.data`, `data-transfer.formats.list`
    (`api.`-префикс в API-слое сохранён);
  - C#-клиент разделён по контроллерам: `ImportClient`, `ExportClient`, `FormatsClient` (папка
    `Implementations/DataTransfer`), в `CompositeClient` свойства `Import`, `Export`, `Formats`;
    `ListFormatsAsync` стал `FormatsClient.ListAsync`;
  - контроллеры: `ImportController` (было `ImportsController`), `ExportController`, `FormatsController`;
  - мапперы API лежат в `DataTransfer/` подпапках; `DataTransferRequestMapper` и `DataTransferResponseMapper`
    разделены на `ExportRequestMapper`/`FormatsRequestMapper` и `ExportResponseMapper`/`FormatsResponseMapper`;
  - Web (по просьбе пользователя, после первой сдачи): `clients/dataTransfer/{ImportClient,ExportClient,FormatsClient}.ts`,
    `clients/models/dataTransfer/{import,formats}`, `pages/dataTransfer/{import,export}`; в контексте клиентов
    свойства `imports`, `exports`, `formats`, `FormatsClient.list()`; маршруты страниц браузера (`/imports`,
    `/export`) оставлены как есть;
  - старые `ai/plans/data-transfer/**` не правились: это записи состояния на момент работы.
- 2026-10-24 (Claude Code, claude-sonnet-5-5): независимая проверка отдельным агентом: APPROVE WITH NOTES. По замечаниям и
  решению пользователя: удалены пустые каталоги Web, убран лишний `exports` из зависимостей эффекта `ExportPage`, новые
  файлы получили BOM; строки `RequestType` не менялись обратно (решение пользователя). По просьбе пользователя убраны
  остатки старого имени: маршруты страниц Web теперь `/data-transfer/imports`, `/data-transfer/imports/:id`,
  `/data-transfer/exports`, `ImportsPage` стал `ImportListPage`, префикс блокировки `data-transfer/imports/`.
  Свойство `ImportListResponse.Imports` оставлено: коллекция в списочных ответах называется во множественном числе
  (`ActivityListResponse.Activities`).
- 2026-10-24 (Claude Code, claude-sonnet-5-5): план закрыт по решению пользователя. Повторный ручной прогон в браузере по новым
  адресам страниц прошёл без ошибок. Старые `ai/plans/data-transfer/**` по решению пользователя не правятся (история).
  Прогон `docker/test` агентом не выполнялся.
