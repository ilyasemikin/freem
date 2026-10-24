# Этап: домен и хранилище

Автор: Claude Code, claude-sonnet-5-5, 2026-10-20

См. общий контекст и место этого этапа: `ai/plans/record-periods/general/roadmap.md` (этап 1).

## Зачем именно так

Период становится дочерней сущностью агрегата `Record`: инварианты набора периодов (не пусто, нет
пересечений, порядок) проверяются в одном месте и в одной транзакции, а отдельный репозиторий, события и
блокировки не нужны. Этап ограничен доменом и хранилищем, чтобы форма данных, схема БД и поведение выборки
были проверены интеграционными тестами репозитория до того, как менять use case'ы и публичный API.
Обратная совместимость не нужна (БД пустая): миграция только создаёт схему, старый формат событий не читается.

## Зафиксированные факты

Проверено по коду (2026-10-20):

- `Record.Period` — единственное свойство периода (`DateTimePeriod`, с `set`), конструктор `Record` принимает
  его последним параметром. Конструируется в трёх местах: `RecordMapper.MapToDomainEntity` (хранилище),
  `CreateRecordUseCase`, `TimeStatisticsTests`.
- `DateTimePeriod` — неизменяемый класс; конструктор округляет до секунд (`EraseMilliseconds`) и приводит к UTC,
  бросает `ArgumentException` при `StartAt > EndAt`. `IsNonOverlapsed` использует строгое сравнение
  (`left.EndAt < right.StartAt`), то есть периоды, у которых конец одного совпадает с началом другого, уже
  считаются пересекающимися. `operator ==` для двух `null` возвращает `false`.
- `RecordEntity` хранит `StartAt`/`EndAt` колонками таблицы `records`, check-констрейнт `start_at <= end_at`
  (`EntitiesNames.Records.Constraints.TimePeriodCheck`), индекс только по `user_id`. Связи с активностями и
  тегами — через `RecordActivityRelationEntity`/`RecordTagRelationEntity`, на них висят триггеры
  (`UserIdCheckTrigger`).
- `RecordsRepository`: `UpdateAsync` сравнивает через `RecordEqualityComparer` (учитывает `Period`) и
  записывает `StartAt`/`EndAt`; оба `FindAsync` сортируют по `StartAt, EndAt, Id`. В `FindAsync(RecordByPeriodFilter)`
  условие попадания — «начало или конец записи лежит внутри фильтра», поэтому запись, полностью накрывающая
  период фильтра (началась раньше и закончилась позже), не попадает в выборку. Условие keyset-пагинации
  (`AfterRecordId`) сравнивает `e.StartAt == filter.Period.EndAt`, а не позицию курсора, то есть выглядит
  ошибочным (проверить тестом на этапе реализации).
- События `RecordCreatedEvent`/`RecordUpdatedEvent` содержат `required DateTimePeriod Period`.
  `EventPayload` — плоский словарь `string → string?` (`Set`, `SetDate`, `SetList` для списка строк);
  `EventPayloadSerializer` кладёт период как два ключа `PeriodStartAt`/`PeriodEndAt`.
- EF-миграции лежат в `Freem.Domain.Storage.PostgreSQL.Migrations/Instances` (последняя —
  `20261020055037_RecreateEvents`, `DatabaseContextModelSnapshot.cs` рядом); применяются
  `postgres-migration` контейнером. Имена таблиц, колонок и констрейнтов — константы в
  `EntitiesNames` (`Records.Properties`, `Records.Constraints`).
- Идентификатор записи — `RecordId : Id` с `Generate()`, `Parse`/`TryParse` (`FormatException` на мусор);
  `RecordDescription` — value object с `MaxLength = 1024`.

## Один-два иллюстративных примера (не исчерпывающий список!)

- `Record.Period { get; set; }` заменяется на коллекцию периодов (value object или коллекция, как
  `RelatedActivityIdsCollection`), каждый элемент — `RecordPeriod` с `RecordPeriodId`, `DateTimePeriod` и
  `RecordPeriodDescription?`.
- В `RecordsRepository.FindAsync(RecordByPeriodFilter)` условие по `e.StartAt/e.EndAt` превращается в условие
  по связанной таблице периодов.

## Направление решения (не пошаговая реализация)

- Домен: `RecordPeriodId`, `RecordPeriodDescription` (value object, `Parse` бросает `FormatException`), `RecordPeriod`,
  коллекция периодов в `Record`, на которой держатся инварианты. Имена без лишних префиксов относительно
  папки (`Records/`).
- БД: таблица `record_periods(id PK, record_id FK ON DELETE CASCADE, start_at, end_at, description,
  created_at/updated_at по соглашению auditable-сущностей, RowVersion при необходимости)`, check
  `start_at <= end_at`, индексы по `record_id` и `(start_at, end_at)`. Колонки `start_at`/`end_at` убираются
  из `records`. Одна новая миграция.
- Репозиторий: загрузка записи вместе с периодами; обновление списка периодов по `Id` (добавление,
  изменение, удаление). Фильтр по периоду — запись попадает в выборку, если пересекается хотя бы один её
  период. Порядок списка записей — по определённому ниже ключу.
- События: тело `RecordCreatedEvent`/`RecordUpdatedEvent` — список периодов (с id и описанием).
- `RecordEqualityComparer` учитывает набор периодов.

Решения пользователя (2026-10-20):

1. Периоды одной записи могут касаться (конец одного = началу другого); запрещено только пересечение с положительной
   длиной. `DateTimePeriod` не меняется, проверка живёт в коллекции периодов записи (или новым методом).
2. Порядок списка записей: `MIN(start_at)`, затем `MAX(end_at)`, затем `Id`.
3. Период нулевой длины (`StartAt == EndAt`) запрещён: проверка в `RecordPeriod`, в БД check `start_at < end_at`
   (в `DateTimePeriod` нулевая длина остаётся допустимой, он используется и в других местах).
4. Периоды в `EventPayload` кодируются одним ключом со списком JSON-объектов (id, начало, конец, описание).
5. Существующие use case'ы создания и обновления записи до этапа 2 работают с одним периодом (обновление
   заменяет весь набор одним).
6. Фильтр по периоду и keyset-пагинацию чиним в рамках этапа: запись попадает в выборку, если хотя бы один её период
   пересекается с периодом фильтра (в том числе накрывает его целиком), курсор пагинации сравнивает позицию
   по ключу из п. 2.

## Явно не входит в этот файл (пока)

Пошаговый план реализации, точные сигнатуры, список затронутых файлов, тестовая матрица. Use case'ы и API
(этап 2) и фронтенд (этап 3) не затрагиваются.

## Критерий готовности

- `dotnet build Freem.sln` без новых предупреждений; `dotnet test Freem.sln` (unit-тесты) зелёные, включая новые
  тесты инвариантов коллекции периодов и сериализации событий.
- `.IntegrationTests` хранилища в `docker/test` зелёные: запись с несколькими периодами сохраняется, читается,
  обновляется по `Id` периодов, удаляется каскадом; выборка по периоду и порядок списка соответствуют решениям
  1-3 и 6; миграция применяется на чистой БД.
- Существующие use case'ы создания и обновления записи работают с одним периодом (решение 5).

## План реализации

1. **Value objects и сущность.** `RecordPeriodId`, `RecordPeriodDescription`, `RecordPeriod` (нулевая длина
   запрещена, `ArgumentException`/`FormatException` по соглашениям проекта) и коллекция периодов с инвариантами
   (не пусто, нет пересечения с положительной длиной, касание разрешено, отсортирована по началу). Unit-тесты на
   инварианты. Проверка: `dotnet test Freem.Domain.UnitTests`.
2. **`Record` и компаратор.** Заменить `Period` на коллекцию периодов, обновить конструктор, события
   `Created`/`Updated` и `RecordEqualityComparer`; трёх мест конструирования (`CreateRecordUseCase`,
   `TimeStatisticsTests`, маппер) править минимально: один период. Статистика пока читает периоды
   плоским перебором без изменения алгоритма. Проверка: сборка решения, unit-тесты.
3. **Сериализация событий.** Кодирование списка периодов JSON-значением в `EventPayloadSerializer`
   (сериализация и десериализация), тесты `EventPayloadSerializerTests`/`EventsRepositoryTests`. Проверка: unit-тесты.
4. **Схема БД.** `RecordPeriodEntity` и конфигурация (`record_periods`, FK с каскадом, check `start_at < end_at`,
   индексы по `record_id` и `(start_at, end_at)`), константы в `EntitiesNames`, удаление `StartAt`/`EndAt` из
   `RecordEntity`, одна миграция. Проверка: миграция применяется на чистой БД (`docker/test`).
5. **Репозиторий.** Маппинг записи с периодами, `Include` периодов, обновление набора периодов по `Id`
   (добавить/изменить/удалить), порядок из п. 2, фильтр по периоду и пагинация из п. 6. Проверка:
   интеграционные тесты `RecordsRepositoryTests` (несколько периодов, накрывающая запись, пагинация через
   границы страниц, каскадное удаление).
6. **Мост для use case'ов.** `CreateRecordUseCase`/`UpdateRecordUseCase` и API-тесты работают с одним
   периодом (п. 5 решений); исправить тесты, которые читали `Record.Period`. Проверка: `dotnet build`,
   `dotnet test` и `.IntegrationTests` в `docker/test`.

## Статус и изменения

- 2026-10-20 (Claude Code, claude-sonnet-5-5): пользователь ответил на все открытые вопросы (внесены в текст),
  написан план реализации, ожидает просмотра; реализация не начата.
- 2026-10-20 (Claude Code, claude-sonnet-5-5): шаг 1 плана выполнен (value objects, `RecordPeriod`, `RecordPeriodsCollection`, unit-тесты; `Freem.Domain.UnitTests` 172/172). Коллекция лежит в `Freem.Domain.Entities/Records`, а не в `Freem.Domain`, потому что зависит от `RecordPeriod`. Ожидает просмотра, шаги 2-6 не начаты.
- 2026-10-20 (Claude Code, claude-sonnet-5-5): шаги 2-6 плана выполнены, этап ожидает ревью пользователя. Сборка
  `Freem.sln`: 0 ошибок. Unit-тесты проходят (`Freem.Domain.UnitTests` 173, `Freem.Domain.Storage.PostgreSQL.UnitTests` 61 и др.).
  **Интеграционные тесты (`docker/test`) не запускались**: Docker-сокет недоступен агенту, нужен запуск пользователем
  (`docker compose up --build --abort-on-container-exit test`, затем `down -v`). SQL ключевых запросов репозитория
  проверен через `ToQueryString()` без БД (подзапросы `min/max`, `EXISTS`, курсор). Расхождения с планом:
  - События не могут ссылаться на `RecordPeriod` (он в `Freem.Domain.Entities`), поэтому добавлен
    `RecordPeriodSnapshot` в `Freem.Domain/Records/Models`; события несут `IReadOnlyList<RecordPeriodSnapshot>`.
  - `TimeStatisticsCalculator` пришлось изменить (в плане «без изменения алгоритма»): он рассчитывал на
    последовательность записей по возрастанию начала, а с несколькими периодами записи интервалы разных записей
    перемежаются. Теперь периоды всех записей разворачиваются в плоский список и сортируются по началу; добавлен тест.
  - Пагинация: `RecordByPeriodFilter.AfterRecordId` теперь курсор по ключу (`MIN(start_at)`, `MAX(end_at)`, `Id`) записи
    с этим `Id`, период фильтра остаётся неизменным; `Count` в ответе — общее число подходящих записей, а не число
    после курсора. `StatisticsPerDaysUseCase`/`StatisticsPerPeriodUseCase` переведены на этот курсор (раньше они
    сдвигали начало периода фильтра) и завершают цикл, если страница не добавила записей.
  - Семантика фильтра: запись попадает, если хотя бы один период имеет положительное пересечение с периодом фильтра
    (`start < filter.end && end > filter.start`); запись, лишь касающаяся границы, не попадает.
  - Мост для API: `RecordMapper` публичного API отдаёт охватывающий период (начало первого - конец последнего),
    помечено TODO до этапа 2. Обновление записи через use case заменяет весь набор одним новым периодом (с новым `Id`).
  - Миграция `20261020101405_RecordPeriods` создана `dotnet ef`: удаляет колонки `start_at`/`end_at` и check из `records`,
    создаёт `record_periods`; данные не переносятся (БД пустая).
- 2026-10-20 (Claude Code, claude-sonnet-5-5): по выводу `docker/test` (запуск пользователя) падали API-тесты записей: `RecordTestBase` создавал
  запись с периодом нулевой длины (`i = 0` часов), `RecordPeriod` их теперь запрещает, use case падал с 500. Исправлено: `Create/UpdateRecordUseCase`
  возвращают `PeriodIsInvalid` (422) для периода нулевой длины, `RecordTestBase` использует `i + 1` часов, `RecordsPreparer.StartRunningAsync`
  по умолчанию стартует на 10 минут раньше (иначе стоп сразу после старта даёт нулевой период). Добавлен тест use case'а на нулевую длину.
  Следствие решения 3: остановка running record в ту же секунду, когда он стартовал, теперь даёт `PeriodIsInvalid`. Повторный прогон `docker/test` ожидается.
- 2026-10-20 (Claude Code, claude-sonnet-5-5): второй прогон `docker/test` (пользователь): unit-тесты и Redis зелёные; в выводе видны три падения.
  1) `RecordsRepositoryTests.CreateAsync_ShouldSuccess_WhenPassValid`: в тестовой фабрике период создавался в `.Do(...)` кастомизации
  AutoFixture с `RecordId`, не совпадавшим с `Id` записи (воспроизведено локально без БД); `CreateRecord()` теперь сам задаёт период
  с `RecordId` записи, `CreateRecords` идёт через него.
  2) Падения API-тестов `StartRunningRecordTests.Request_ShouldSuccess_WhenAnotherExists` и `UpdateRunningRecordTests.Request_ShouldFail_WhenRunningRecordNotExists`:
  остановка running record, запущенного менее секунды назад (в том числе автоостановка при старте нового), давала `PeriodIsInvalid`. Решение
  (в рамках решения 3): если после округления период пуст, запись не создаётся, running record просто останавливается (успех). Прежний
  `PeriodIsInvalid` при `EndAt` раньше старта сохранён. Добавлен тест `ShouldSuccessWithoutRecord_WhenPeriodIsEmpty`. Откатил
  изменение `RecordsPreparer.StartRunningAsync` (больше не нужно). Требуется третий прогон `docker/test`; вывод был обрезан, остальные падения (если есть) не видны.
- 2026-10-20 (Claude Code, claude-sonnet-5-5): по просьбе пользователя точность времени повышена с секунд до миллисекунд.
  `DateTimeOperations.EraseMilliseconds` заменён на `TruncateToMilliseconds` (отбрасывает всё тоньше миллисекунды; используется в `DateTimePeriod`,
  времени старта `RunningRecord` и тестовой фабрике хранилища). Миллисекунды без потерь помещаются в `timestamptz` (микросекунды) и в `Date` JS;
  фронтенд уже отправляет `toISOString()` с миллисекундами. Миграция не нужна. Ветка «пустой период при остановке» сохранена, но теперь
  срабатывает только при совпадении до миллисекунды. Unit-тесты проходят; интеграционные ждут прогона.
- 2026-10-20 (Claude Code, claude-sonnet-5-5): пользователь прогнал `docker/test`: все тесты проходят (unit и интеграционные), критерий готовности этапа выполнен. Этап ожидает ревью и коммита пользователя.
- 2026-10-20 (Claude Code, claude-sonnet-5-5): по замечанию независимой проверки (APPROVE WITH NOTES) и просьбе пользователя `EventPayloadSerializer` теперь бросает `FormatException` на невалидный JSON периодов и на период с началом позже конца (раньше `JsonException`/`ArgumentException`); добавлен тест на невалидный payload. Остальные замечания ревью (`out.log`, новые `RecordPeriodId` при обновлении через мост, устаревшие факты в stage-файле) оставлены без изменений по решению пользователя; второе замечание учесть на этапе 2.
- 2026-10-20 (Claude Code, claude-sonnet-5-5): этап 1 закрыт пользователем, коммит 2fedc9d. Независимая проверка отдельным агентом: APPROVE WITH NOTES (блокирующих замечаний нет); замечание о `FormatException` в сериализаторе исправлено по решению пользователя, остальные оставлены без изменений.
