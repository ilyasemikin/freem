# Data transfer: форматы в отдельных проектах

Автор: Claude Code, claude-opus-5-5, 2026-10-25

Статус: **фазы 1-3 выполнены, не закоммичено**

## Зачем

После приборки (`ai/plans/data-transfer-tidy.md`) импорт и экспорт лежат в одной схеме
`DataTransfer/{Import,Export,Formats}`, но в `Freem.Domain.UseCases` по-прежнему перемешаны две вещи:

- знание конкретных форматов: разбор CSV Time Tracker и его колонки, модель файла собственного JSON, маркер,
  версия и имена свойств, потоковая запись JSON по секциям;
- логика сервиса: абстракции `IImportParser`/`IExportWriter`, реестры, описания форматов (`FormatDescription`),
  перевод в доменные `Import*`/`Export*`, проверка значений через value objects, use cases.

Цель — вынести знание форматов в отдельные проекты по образцу `Stashit.Banks.*`, чтобы сервис занимался только
своими делами, а новый формат добавлялся отдельным проектом и тонким адаптером.

## Образец: StashIt

- `Stashit.Banks.TBank` не знает домена: зависит только от библиотеки разбора, `Utilities.Errors` и
  `Utilities.Primitives`; читает файл в собственную модель (`TBankStatement`), ошибки — свой каталог
  (`TBankErrorCatalog`).
- Абстракция (`ITransactionsStatementParser`), реестр и описания форматов остаются в Core UseCases.
- Там же адаптер (`TBankTransactionsStatementParser`): вызывает парсер банка и переводит его модель в доменные
  сущности.

## Принятые решения

- **Вариант StashIt**: проекты форматов без ссылок на `Freem.Domain*` и `Freem.Time`; абстракции, реестры и
  адаптеры остаются в `Freem.Domain.UseCases`. Общий пакет-абстракцию не вводим.
- **Имена проектов**: `Freem.DataTransfer.ZaferErtas.TimeTracker` и `Freem.DataTransfer.NativeJson`, к каждому
  `*.UnitTests`. `DataTransfer` — область, уже используемая на всех слоях; `NativeJson` совпадает с текущими
  именами типов и не путается с инфраструктурой `*.Serialization.Json`. Внешние форматы именуются
  `Freem.DataTransfer.<Автор>.<Приложение>`: Time Tracker — Android-приложение Zafer Ertas
  (`zzz1zzz.tracktime`, см. `ai/plans/data-transfer/general/roadmap.md`), а приложений с таким названием много.
  Типы внутри пакета сохраняют префикс `TimeTracker`; ключ формата `time-tracker` и его отображаемое имя не
  меняются.
- **Собственный JSON выносится так же**, со своей моделью и двусторонним переводом в адаптерах (экспорт:
  `ExportItem` → модель пакета; импорт: модель пакета → `Import*`). Формат файла тем самым отвязан от доменной
  модели.
- **Зависимости проектов форматов**: `Utilities.Errors`, `Utilities.Primitives`, BCL (`System.Text.Json`).
  `DisableTransitiveProjectReferences` как в остальных проектах.
- **Граница ответственности**:
  - пакет проверяет то, что относится к самому файлу: пустой файл, заголовок CSV, число колонок, формат даты
    и времени Time Tracker, валидность JSON, маркер `freem` и версию;
  - адаптер проверяет то, что относится к домену: разбор идентификаторов и value objects (`ActivityName`,
    `RecordName`, `TimeZoneOffset` и т.д.), уникальность идентификаторов, ссылки на неизвестные теги и
    активности, конец периода раньше начала, обрезку заметок по `RecordName.MaxLength`, применение
    `ImportParseOptions.TimeZoneOffset`.
- **Предупреждения по строкам**: модель Time Tracker возвращает, кроме строк, проблемы уровня файла по строкам
  (номер строки и сообщение) для строк, которые пакет пропустил. Адаптер переводит их в `ImportParseWarning`
  и добавляет свои (доменные) предупреждения. `RowsInFile` считает пакет.
- **Константы форматов** переезжают в пакеты (`TimeTrackerFormat.Columns` → константы колонок в
  `Freem.DataTransfer.ZaferErtas.TimeTracker`; `NativeJsonFormat.FormatMarker`, `CurrentVersion`, `Properties` → схема в
  `Freem.DataTransfer.NativeJson`). В UseCases `TimeTrackerFormat`/`NativeJsonFormat` остаются только с
  `Description` (`FormatDescription` — доменный value object). Классы в пакетах называются иначе, чтобы не
  было совпадения имён.
- **Экспорт остаётся потоковым**: писатель в пакете принимает упорядоченный поток своих элементов (теги →
  активности → записи) и сам проверяет порядок, как сейчас `NativeJsonExportWriter`. У элементов модели (тег,
  активность, запись) общий базовый тип, сигнатура `WriteAsync(IAsyncEnumerable<...>, Stream, CancellationToken)`
  повторяет текущий контракт.
- **Собственные ошибки пакетов**: у каждого пакета свой каталог (`TimeTrackerErrorCatalog`,
  `NativeJsonErrorCatalog`, по образцу `TBankErrorCatalog`). Адаптер переводит ошибку пакета в
  `DomainErrorCatalog.Factories.CreateInvalidRequest(message)`, чтобы ответ API (400 и его тело) остался прежним.
- **Адаптеры группируются по формату**: `Implementations/DataTransfer/Formats/TimeTracker/TimeTrackerImportParser.cs`,
  `Implementations/DataTransfer/Formats/NativeJson/{NativeJsonImportParser,NativeJsonExportWriter}.cs` (аналог
  `Parsers/Implementations/TBank/` в StashIt). Абстракции и реестры остаются в `Import/` и `Export/`.
- **Поведение API не меняется**: те же форматы, ключи, ответы и тексты ошибок и предупреждений; миграций,
  изменений контрактов и Web нет.

## Фазы

Каждая фаза заканчивается зелёной сборкой и юнит-тестами; коммиты — только по просьбе пользователя.

1. **`Freem.DataTransfer.NativeJson`**: проект и тесты в `Freem.slnx`; модель файла (перенос `NativeJsonFile`
   и вложенных типов, публичные), схема (маркер, версия, имена свойств), читатель и потоковый писатель, каталог
   ошибок. Адаптеры `NativeJsonImportParser` и `NativeJsonExportWriter` в UseCases переводятся на пакет.
   Тесты: проверки уровня файла и круговой тест писатель → читатель на модели пакета — в
   `Freem.DataTransfer.NativeJson.UnitTests`; доменные проверки и `NativeJsonRoundTripTests` остаются в
   `Freem.Domain.UseCases.UnitTests`.
2. **`Freem.DataTransfer.ZaferErtas.TimeTracker`**: проект и тесты; модель (`TimeTrackerFile`, строка, проблема по
   строке), константы колонок, читатель, каталог ошибок. Адаптер `TimeTrackerImportParser` переводится на
   пакет. Тесты разделяются так же: разбор CSV — в пакете, доменные проверки и предупреждения — в UseCases.
3. **Завершение**: перенос адаптеров в `Formats/{TimeTracker,NativeJson}/`, DI-регистрация,
   `FormatRegistrationTests`, `FakeFormats` при необходимости; раздел "Repository Layout" в `CLAUDE.md`
   (строка про `Freem.DataTransfer.*` — форматы импорта/экспорта без домена); проверка, что в UseCases не
   осталось знания форматов (колонок, имён свойств, маркера).

## Проверка

- `dotnet build Freem.slnx` и `dotnet test Freem.slnx` (юнит-тесты) после каждой фазы.
- Проверка зависимостей: `Freem.DataTransfer.*` не ссылаются на `Freem.Domain*`.
- Интеграционные тесты (use cases, API) — в `docker/test`, запускает пользователь; ожидается, что они проходят
  без изменений.
- Ручной прогон в браузере — только по согласованию (`ai/skills/web-manual-test/SKILL.md`).

## Статус и изменения

- 2026-10-25 (Claude Code, claude-opus-5-5): план одобрен пользователем, фазы 1-3 выполнены, не закоммичено.
  Проверка: `dotnet build Freem.slnx` без ошибок (предупреждения только в старых файлах тестов Events, их не трогали);
  `dotnet test Freem.slnx` зелёный, включая интеграционные тесты storage, use cases и API на локальных конфигах
  (`docker/test` агентом не запускался). `Freem.DataTransfer.*` не ссылаются на `Freem.Domain*`, библиотеки
  попадают в вывод `Freem.Api.Public`.
  Что вышло за буквальный текст плана:
  - адаптеры сразу положены в `Formats/{NativeJson,TimeTracker}/` (в фазах 1-2, а не в фазе 3); туда же переехали
    `NativeJsonFormat`/`TimeTrackerFormat` с одним `Description`; тесты адаптеров — в
    `Freem.Domain.UseCases.UnitTests/DataTransfer/Formats/{NativeJson,TimeTracker}/`;
  - константы пакетов: `NativeJsonSchema` и `TimeTrackerColumns`; читатели `NativeJsonReader`/`TimeTrackerReader`,
    писатель `NativeJsonWriter`; проблемы строк Time Tracker — `TimeTrackerLineIssue`;
  - писатель NativeJson пропускает свойства со значением `null` (раньше так было только для необязательных полей;
    адаптер обязательные поля всегда заполняет, вывод не изменился);
  - граница для Time Tracker: пакет, кроме колонок и дат, проверяет пустой ID и повтор ID (уникальность строк
    файла), чтобы сохранить прежний порядок проверок. Адаптер проверяет разбор `ImportRecordId`, конец раньше
    начала, активность, обрезку заметок и сводит предупреждения по номеру строки. Отличие от прежнего поведения
    только в крайнем случае: непустой, но неразбираемый ID теперь проверяется после повтора и дат;
  - в `CLAUDE.md` раздел Layers дополнен строкой про `Freem.DataTransfer.*`.
