# Этап: use case'ы, API и статистика

Автор: Claude Code, claude-sonnet-5-5, 2026-10-20

См. общий контекст и место этого этапа: `plans/record-periods/general/roadmap.md` (этап 2).

## Зачем именно так

Этап 1 оставил мост: домен и БД хранят набор периодов, но use case'ы и публичный API по-прежнему работают с одним
периодом (обновление заменяет весь набор одним периодом с новым `RecordPeriodId`, API отдаёт «охватывающий» период).
Этап 2 убирает мост: запись создаётся и читается как набор периодов, а менять отдельный период можно отдельными
операциями (решение пользователя: отдельные операции проще для потребителя). Обратная совместимость не нужна (БД пуста),
поэтому старое поле `period` из контрактов убирается, а не поддерживается параллельно.

## Зафиксированные факты

Проверено по коду (2026-10-20, после коммита `2fedc9d`):

- Вертикальный срез одной операции над записями состоит из: контракта use case'а (`Freem.Domain.UseCases.Contracts/Records/<Op>`,
  тип запроса в `RequestTypes.Records`), обработчика (`Freem.Domain.UseCases/Records`, регистрация `AddRecordsUseCases` в
  `Freem.Domain.UseCases/_Common/DependencyInjection/...`), API-контракта (`Freem.Api.Public.UseCases.Contracts/Records/Api<Op>Request`),
  API-обработчика (`Freem.Api.Public.UseCases/Implementations/Handlers/Records`, регистрация в `ServiceCollectionExtensions` рядом),
  мапперов (`Mappers/Requests|Responses|Dtos`), DTO (`Freem.Api.Public.Contracts/DTO/Records`), действия контроллера
  `Freem.Api.Public/Controllers/V1/RecordsController.cs`, метода клиента `Freem.Api.Public.Client/Implementations/RecordsClient.cs`,
  тестов (`Freem.Domain.UseCases.IntegrationTests`, `Freem.Api.Public.IntegrationTests/Tests/V1/Records`).
- `RemoveRecordUseCase` — образец: блокировка `Lock.Prefix + id`, поиск через `FindByMultipleIdAsync(RecordAndUserIds)`,
  изменение в `StorageTransactionRunner` вместе с публикацией события через `EventProducer`.
- `UpdateRecordUseCase` (мост) при `request.Period` создаёт новый `RecordPeriodId` и заменяет весь набор; `CreateRecordUseCase`
  принимает один `DateTimePeriod` и возвращает `PeriodIsInvalid` для нулевой длины; `StopRunningRecordUseCase` строит
  `CreateRecordRequest` с одним периодом и пропускает создание записи, если период пуст.
- Контракты API: `CreateRecordRequest` (DTO) имеет `Period`, `UpdateRecordRequest` — `UpdateField<DateTimePeriod>? Period`,
  `RecordResponse` — `Period`; `RecordMapper` (Api.UseCases, `Mappers/Dtos`) отдаёт охватывающий период с `TODO`. `CreateRecordResponse` API
  возвращает только id записи.
- JSON: имена в `snake_case` (`JsonNamingPolicy.SnakeCaseLower`), `DateTimePeriod` и идентификаторы сериализуются строкой через
  `ParsableJsonConverter<T>`, регистрация конвертеров — `Freem.Domain.Serialization.Json/EntitiesJsonSerializationConverters.cs`.
  Новые `RecordPeriodId`, `RecordPeriodDescription` нужно добавить туда же.
- Ошибки в HTTP (`CommonResponseMapper.MapStatusCode`): `PeriodIsInvalid`, `RelatedEntitiesNotFound` → 422, `EntityNotFound` → 404,
  `InvalidRequest`/`NothingToDo` → 400. Каталог: `Freem.Domain/DomainErrorCatalog.cs`.
- Статистика (`TimeStatisticsCalculator`, `StatisticsPerDays/PerPeriodUseCase`) уже работает с набором периодов (этап 1); публичные
  DTO статистики не содержат периодов записей.
- Фронтенд (`Freem.Web/src/clients/models/records/*`) использует поле `period: string` в запросах и ответах записей; после
  изменения контракта он перестанет соответствовать API до этапа 3.
- Документация OpenAPI строится из XML-комментариев DTO и контроллера; комментарий `by-period` в `RecordsController` описывает
  старую семантику («начало или конец внутри периода»).

## Один-два иллюстративных примера (не исчерпывающий список!)

- `POST api/v1/records` с телом `{"periods":[{"period":"2026-10-20T10:00:00Z,2026-10-20T11:00:00Z","description":"утро"}], "activities":[...]}`.
- Новая операция: `DELETE api/v1/records/{recordId}/periods/{periodId}` → use case `RemoveRecordPeriodRequest`.

## Направление решения (не пошаговая реализация)

Предлагаемый контракт (подлежит согласованию, см. вопросы):

- **Запись.** `CreateRecordRequest`: вместо `period` — `periods` (непустой список `{period, description?}`); `RecordResponse`: вместо
  `period` — `periods` (список `{id, period, description?}`, по возрастанию начала). `PUT records/{id}` перестаёт менять период: поле
  `period` убирается, периоды меняются только операциями ниже.
- **Операции над периодами** (вложенные ресурсы записи, блокировка по id записи, транзакция, событие `RecordUpdatedEvent` со снимком):
  `POST api/v1/records/{recordId}/periods` (создать период, ответ 201 с id периода), `PUT api/v1/records/{recordId}/periods/{periodId}`
  (частичное изменение: `period?`, `description?`, `null` в описании очищает), `DELETE api/v1/records/{recordId}/periods/{periodId}`.
- **Ошибки.** Пересечение периодов и нулевая длина — `PeriodIsInvalid` (422); неизвестная запись или период — `EntityNotFound` (404);
  удаление единственного периода — ошибка (запись удаляется операцией удаления записи).
- **Домен.** Use case'ы работают через `RecordPeriodsCollection` (`TryAdd`/`TryReplace`/`TryRemove`); `CreateRecordRequest` домена получает список
  периодов с описанием; `StopRunningRecordUseCase` передаёт один период. Мост в `UpdateRecordUseCase` удаляется.
- **Документация/клиент.** XML-комментарии новых DTO и действий, методы `RecordsClient`, правка описания `by-period`.
- **Статистика.** Публичный контракт не меняется; добавляются тесты use case'ов статистики на запись с несколькими периодами.

Решения пользователя (2026-10-20):

1. `PUT records/{id}` не меняет периоды (только имя, описание, активности, теги); периоды меняются отдельными операциями.
2. URL вложенный: `api/v1/records/{recordId}/periods[/{periodId}]`.
3. Удаление единственного периода: `PeriodIsInvalid` (422) с понятным сообщением.
4. Отдельные типы событий не нужны, используется `RecordUpdatedEvent` со снимком периодов.
5. Ответ создания записи остаётся «только id записи»; id периодов клиент получает из `GET`.
6. Фронтенд не работает с API между этапами 2 и 3, правка фронтенда в этом этапе не делается.

## Явно не входит в этот файл (пока)

Пошаговый план реализации, точные сигнатуры, список затронутых файлов, тестовая матрица. Фронтенд (этап 3).

## Критерий готовности

- `dotnet build Freem.sln` без новых предупреждений; `dotnet test Freem.sln` (unit-тесты) зелёные.
- `.IntegrationTests` в `docker/test` (запускает пользователь) зелёные: запись создаётся и читается с несколькими периодами;
  операции добавления, изменения и удаления периода работают, пересечение и нулевая длина отклоняются (422), последний период
  не удаляется, чужие записи недоступны (404); статистика за период корректна для записи с несколькими периодами.
- Мост удалён: нет поля `period` в контрактах записи, нет «охватывающего периода» и `TODO` в `RecordMapper`.
- Документация OpenAPI актуальна (XML-комментарии новых DTO и действий).

## План реализации

1. **Контракты и JSON.** `RecordPeriodId` и `RecordPeriodDescription` в `EntitiesJsonSerializationConverters`; типы запросов
   `records.periods.add|update|remove` в `RequestTypes`; контракты use case'ов `AddRecordPeriodRequest/Response`,
   `UpdateRecordPeriodRequest`, `RemoveRecordPeriodRequest`; `CreateRecordRequest` домена получает список периодов с описанием
   (отдельный тип в контрактах), из `UpdateRecordRequest` убирается `Period`. Проверка: сборка контрактов, тест круга сериализации
   новых идентификаторов.
2. **Use case'ы записей.** `CreateRecordUseCase` принимает набор периодов (валидация: непустой, без пересечений и нулевой длины →
   `PeriodIsInvalid`); `UpdateRecordUseCase` без периода (мост удаляется, `HasChanges` обновлён); `StopRunningRecordUseCase` передаёт
   один период с описанием `null`. Проверка: `dotnet build`, существующие use case-тесты правятся под новый контракт.
3. **Use case'ы периодов.** Три обработчика по образцу `RemoveRecordUseCase` (блокировка по id записи, `FindByMultipleIdAsync`,
   `RecordPeriodsCollection.TryAdd/TryReplace/TryRemove`, транзакция, `RecordUpdatedEvent`); ошибки: неизвестная запись или период —
   `EntityNotFound`, пересечение, нулевая длина и удаление единственного периода — `PeriodIsInvalid` с сообщением; регистрация в DI.
   Проверка: новые тесты в `Freem.Domain.UseCases.IntegrationTests` (добавление, изменение, удаление, пересечение, касание, последний
   период, чужая запись, событие со снимком).
4. **Публичный API.** DTO: `periods` в `CreateRecordRequest`, `RecordResponse` (+ `RecordPeriodResponse`), `AddRecordPeriodRequest`,
   `UpdateRecordPeriodRequest`, ответ с id периода; убрать `Period` из `UpdateRecordRequest`; API-контракты и обработчики, мапперы,
   действия `RecordsController` с `ProducesResponseType` и XML-комментариями, правка описания `by-period`, удаление моста и `TODO` в
   `RecordMapper`. Проверка: сборка, OpenAPI формируется без предупреждений.
5. **Клиент и тесты API.** Методы `RecordsClient`, правка подготовки данных (`RecordsPreparer`, `RecordTestBase`), тесты
   `Freem.Api.Public.IntegrationTests` на создание с несколькими периодами и три новые операции (успех, 404, 422, чужая запись).
   Проверка: `dotnet build Freem.sln`.
6. **Статистика.** Тесты use case'ов статистики на запись с несколькими периодами (границы дня, пересечение с другими записями).
   Проверка: unit- и интеграционные тесты.
7. **Итог.** `dotnet test` для unit-тестов; интеграционные в `docker/test` запускает пользователь. Записать расхождения в
   «Статус и изменения».

## Статус и изменения

- 2026-10-20 (Claude Code, claude-sonnet-5-5): пользователь ответил на открытые вопросы (внесены в текст), написан план
  реализации, ожидает просмотра; реализация не начата.
- 2026-10-20 (Claude Code, claude-sonnet-5-5): шаги 1-7 плана выполнены, этап ожидает ревью и прогона `docker/test` пользователем.
  `dotnet build Freem.sln`: 0 ошибок, новых предупреждений нет. Unit-тесты проходят (`Freem.Domain.UnitTests` 175). Интеграционные тесты
  (use case'ы, API, статистика) собираются, но не запускались: Docker недоступен агенту. Что сделано и отличия от плана:
  - Контракты use case'ов `AddPeriod`/`UpdatePeriod`/`RemovePeriod`, типы запросов, `NewRecordPeriod` для создания записи; в
    `UpdateRecordRequest` убран `Period`. В `CreateRecordRequest` домена оставлен удобный конструктор с одним `DateTimePeriod`
    (используют тесты, не обязателен для кода).
  - `RecordPeriodId` добавлен в `EntityIdNameProvider`/`DomainErrorCatalog.Properties` (иначе `EntityNotFound` по неизвестному периоду бросал
    бы исключение); `RecordPeriodsCollection.TryCreate` добавлен для валидации набора без исключений; общая проверка периодов вынесена в
    `RecordPeriodsFactory`.
  - Публичный API: `periods` в создании и ответе записи, `period` убран из `PUT records/{id}`; `POST|PUT|DELETE records/{recordId}/periods[/{periodId}]`;
    DTO `RecordPeriodRequest`, `UpdateRecordPeriodRequest`, `RecordPeriodResponse`, `AddRecordPeriodResponse`; клиент `RecordsClient`;
    мост в `RecordMapper` и `TODO` удалены; описание `by-period` обновлено. Новые типы JSON-конвертеров `RecordPeriodId`/`RecordPeriodDescription`.
  - Тесты: use case-тесты `RecordsAdd|Update|RemovePeriodUseCaseTests`, `StatisticsSeveralPeriodsUseCaseTests`, дополнены `RecordsCreateUseCase`; API-тесты
    `RecordPeriodTests`, дополнен `CreateRecordTests`; удалены тесты обновления периода через `PUT records/{id}`.
  - Замечание ревью этапа 1 (новые `RecordPeriodId` при обновлении через мост) снято удалением моста.
  - Не проверено: фактический вид OpenAPI-документа и HTTP-поведение (привязка `RecordPeriodId` из маршрута) — проверяют интеграционные API-тесты.
- 2026-10-20 (Claude Code, claude-sonnet-5-5): прогон `docker/test` (пользователь): всё зелёное, кроме одного теста
  `RecordsUpdatePeriodUseCaseTests.ShouldSuccess_WhenUpdateAndClearDescription` (`ArgumentNullException` в `UpdateField<T>`). Причина:
  конструктор `UpdateField<TValue>` (use case-контракты) запрещал `null`, поэтому очистить значение (`null` в описании периода, а также имени и
  описания записи, как обещает документация API) было нельзя, и API при таком теле отвечал бы 500 (существующий дефект, обнаружен при
  этой работе). Проверка `null` убрана (ограничение остаётся в аннотациях nullable), добавлен API-тест очистки описания периода.
  Ожидается повторный прогон `docker/test`.
- 2026-10-20 (Claude Code, claude-sonnet-5-5): независимая проверка отдельным агентом: APPROVE WITH NOTES (блокирующих замечаний нет). По замечаниям ревью
  по просьбе пользователя дописаны тесты (собираются, не запускались): чужая запись для `UpdatePeriod`/`RemovePeriod` на уровне use case'ов и для трёх операций в API
  (404), API-тесты 422 для `PUT` (пересечение, нулевая длина), `POST` с нулевой длиной и создания записи без периодов, проверка снимка периодов в
  `RecordUpdatedEvent` (use case-тест `ShouldPublishRecordUpdatedEventWithPeriodsSnapshot`). Ожидается повторный прогон `docker/test`.
- 2026-10-20 (Claude Code, claude-sonnet-5-5): прогон `docker/test` (пользователь): API (96), хранилище (111) зелёные; в use case-тестах (112) упал один новый тест
  `ShouldPublishRecordUpdatedEventWithPeriodsSnapshot`: ошибка в тесте, а не в коде (тип события сравнивался через `==`, у `EventType` оператор не
  переопределён, событие `records.updated` в выборке было). Исправлено на `Equals`. Ожидается повторный прогон use case-тестов.
