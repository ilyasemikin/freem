# Этап: Идентификаторы

Автор: Claude Code, claude-opus-5-5, 2026-10-19

См. общий контекст и место этого этапа: `plans/freem-architecture/general/roadmap.md` (этап 4).

## Зачем именно так

Решение пользователя (roadmap, 2026-10-18): `Freem.Identifiers` убираем, все доменные идентификаторы переводим на
подход StashIt — базовый `Id` в `Domain`, `sealed` наследники с закрытым конструктором, `Generate()`,
`IParsable<T>`, без генераторов и DI. Сейчас идентификатор во Freem — это три слоя (`IIdentifier` →
`StringIdentifier` → `IEntityIdentifier`), генераторы через DI ради одной строки `Guid.NewGuid().ToString("N")` и
неявные преобразования из строки и в строку, из-за которых компилятор не видит, где строка становится Id. На этапе 3
идентификаторы перенесены в `Freem.Domain` как есть, поэтому теперь их можно переписать на месте. Заодно закрывается
отложенный вопрос о JSON-конвертерах: при `IParsable<T>` достаточно `ParsableJsonConverter<T>`.

## Зафиксированные факты

Проверено 2026-10-19 чтением кода Freem и StashIt, а также пробной сборкой в отдельном worktree (неявные операторы
удалены, рабочее дерево не тронуто).

**StashIt (образец):**

- `Core/Stashit.Core.Domain/Id.cs`: абстрактный `Id` со строковым значением (`ThrowIfNullOrWhiteSpace`), `Equals`,
  `GetHashCode`, `ToString`, `==`/`!=`, `protected static GenerateValue()` (`Guid.NewGuid().ToString("N")`). Неявных
  преобразований нет.
- Наследник `Tags/TagId.cs`: `sealed`, закрытый конструктор, `static Generate()`, `IParsable<TagId>`: `Parse` бросает
  `FormatException`, `TryParse` возвращает `false` на пустой строке. Лежит прямо в папке агрегата, без `Identifiers/`.
- Два расхождения образца с правилами, которые лучше не переносить: `Id.Equals` не сравнивает типы (`TagId("x")`
  равен `AccountId("x")`), а `TryParse` проверяет `IsNullOrEmpty`, поэтому строка из пробелов проходит проверку и падает
  в конструкторе с `ArgumentException`, а не с `FormatException`.
- JSON: `ParsableJsonConverter<T>` из `Utilities.Serialization.Json` 0.0.1 (пишет `ToString()`, читает `T.Parse`);
  пакет собран в `~/Sources/CSharpUtilities/docker/out/packages/`, в `_Src/Freem/packages/` его пока нет.
- События ссылаются на сущность через `EntityRef(EntityType, string)`. Во Freem отдельного этапа Events нет (решение
  roadmap), поэтому этот подход не переносится.

**Freem:**

- `Freem.Identifiers`: `IIdentifier`, `StringIdentifier` (сравнение с учётом типа, неявное преобразование в
  `string`), `IIdentifierGenerator<T>`, `GuidStringIdentifierGenerator<T>`, `TypedIdentifierGenerator` (ищет
  интерфейс по строке `"IIdentifierGenerator`1"`), DI-хелперы. Проект подключают 18 `.csproj`, включая все тестовые.
- Идентификаторы в `Freem.Domain/*/Identifiers/`: `ActivityIdentifier`, `RecordIdentifier`,
  `RunningRecordIdentifier`, `TagIdentifier`, `UserIdentifier`, `EventIdentifier`. Конструктор у всех открытый, есть
  неявное преобразование `string → Id`. У `RunningRecordIdentifier` есть ещё неявные преобразования в
  `UserIdentifier` и из него: Id запущенной записи совпадает с Id пользователя (`RunningRecord.Id => UserId`).
- `IEntityIdentifier` — ограничение generic-параметров `IEntity<T>`, `IEntityEvent<,>`/`EntityEvent<,>`, коллекций
  связей и тип в `NotFoundException`; `IEntityEvent<IEntityIdentifier, UserIdentifier>` проходит через репозиторий
  событий, producer, контракты use case'ов, контроллер и клиентов. `IIdentifier` используют `NotFoundRelatedException`,
  `DatabaseContextWriteContext` и `HasTagsIdentifiers`/`HasActivitiesIdentifiers` (10 вызовов в use case'ах).
- `IMultipleEntityIdentifier` реализуют `ActivityAndUserIdentifiers`, `RecordAndUserIdentifiers`,
  `TagAndUserIdentifiers`; свойство `Ids` не читает никто.
- `EntityIdentifierFactory` (имя сущности + строка → Id) и `EntityIdentifierNameProvider` (Id → имя) нужны журналу
  событий: в БД `events.entity_name`/`entity_id` — строки, имена из `EntityNames`. Используются в хранилище,
  `EventJsonConverter` и миграциях (check-констрейнты).
- Генераторы: `IIdentifierGenerator<T>` внедряется в `CreateActivityUseCase`, `CreateRecordUseCase`,
  `CreateTagUseCase`, `EventProducer`, `SimpleDistributedLocker` (`Freem.Locking.Redis`, `SimpleLockIdentifier` —
  внутренний токен блокировки в Redis); в тестах — `TypedIdentifierGenerator` (`BaseRepositoryTests`) и
  `TestContext.CreateIdentifier<T>`. Регистрация: `AddIdentifiersGenerators()` в `Freem.Domain/DependencyInjection`.
- Неявное преобразование `Id → string` используют 55 мест в `Storage.PostgreSQL` (мапперы и сравнения в LINQ-запросах
  EF, например `e.Id == ids.TagId`), 14 в `Serialization.Json`, 1 в `Locking.Redis.IntegrationTests`. Обратное
  `string → Id` используют конвертеры `Serialization.Json`, 24 места в `Storage.PostgreSQL.IntegrationTests` и 1 в
  `UseCases.IntegrationTests`. API и `FunctionalTests` в пробной сборке не собрались из-за `Serialization.Json`, их
  точное число выяснится при реализации.
- В БД Id — строки в отдельных классах-сущностях EF; `HasConversion` для идентификаторов нет. Схема БД от этапа не
  зависит.
- JSON: 6 конвертеров идентификаторов в `Freem.Entities.Serialization.Json/*/Identifiers/` (строка ↔ Id), плюс
  `EventJsonConverter` и `RelatedEntitiesIdentifiersCollectionJsonConverter<T>`, которые строят Id сами. Конвертеры
  value objects (`ActivityName`, `TagName`, `Nickname` и др.) и `Freem.Time.Serialization.Json` написаны вручную,
  `IParsable` у этих типов нет.
- Контроллеры получают Id из маршрута строкой и создают его конструктором (12 мест, например
  `GetActivityController`).
- Строковых ссылок на сборку `Freem.Identifiers` вне `.csproj`/`Freem.sln` нет (`docker/`, `Dockerfile`, `CLAUDE.md`);
  единственная строковая ссылка на тип (`"IIdentifierGenerator`1"`) уходит вместе с проектом.

**Value objects (проверено 2026-10-19):**

- В `Freem.Domain/*/Models/`: `ActivityName` (не пустая, ≤ 128), `ActivityStatus` (обёртка над enum `Value`, в JSON —
  имя `"Active"`/`"Archived"`), `EventAction` (не пустая), `RecordName` (пустая допустима, ≤ 128), `RecordDescription`
  (пустая допустима, ≤ 1024), `TagName` (≤ 128), `Nickname` (не пустая), `DayUtcOffset` (обёртка над `TimeSpan`, в JSON —
  формат `TimeSpan` из System.Text.Json). Конструкторы открытые, у всех неявные преобразования в обе стороны, `IParsable`
  нет. Конструкторами пользуются мало (1–6 мест на тип), остальное — неявные преобразования.
- Баг в `TagName`: `ArgumentException.ThrowIfNullOrWhiteSpace(nameof(value))` проверяет строку `"value"`, а не значение,
  поэтому пустое имя тега или имя из пробелов сейчас принимается.
- Конвертеры value objects превращают строку в объект неявным преобразованием. Если значение невалидно, конструктор
  бросает `ArgumentException`, а не `FormatException`.
- `Freem.Time`: `DatePeriod` и `DateTimePeriod` имеют собственный `TryParse(string, out)` без `IParsable` и строковый
  формат в JSON. Их конвертеры — единственное содержимое `Freem.Time.Serialization.Json`, которым пользуется только
  `Freem.Entities.Serialization.Json`. `TryParse` вызывают ещё и model binder'ы API (`ValueProviderResultExtensions`).
  У `DateTimePeriod` 57 вызовов конструктора `(start, end)`, это обычное создание, а не разбор строки.
- Образец StashIt (`Tags/TagName.cs`): закрытый конструктор, `IParsable` (`TryParse` отбрасывает пустую строку и
  обрезает пробелы), `ParsableJsonConverter<TagName>`; лежит в папке агрегата.

## Один-два иллюстративных примера (не исчерпывающий список!)

- `TagIdentifier : StringIdentifier, IEntityIdentifier` с открытым конструктором и `implicit operator` →
  `sealed class TagIdentifier : Id, IParsable<TagIdentifier>` с закрытым конструктором, `Generate()`, `Parse`/`TryParse`;
  `TagIdentifierJsonConverter` → `ParsableJsonConverter<TagIdentifier>`.
- `CreateTagUseCase` получает `IIdentifierGenerator<TagIdentifier>` из DI → вызывает `TagIdentifier.Generate()`.

## Направление решения (не пошаговая реализация)

- Базовый `Id` в `Freem.Domain` по образцу StashIt, но `Equals` учитывает тип, как сейчас `StringIdentifier`, а
  `TryParse` отбрасывает строку из пробелов. Неявных преобразований нет: строка → Id через `Parse` или `TryParse`,
  Id → строка через `ToString()`, по правилу стиля вынесенный в локальную переменную.
- `IIdentifier`, `IEntityIdentifier`, `IMultipleEntityIdentifier` удаляются; там, где они были ограничением или типом
  (события, коллекции связей, исключения хранилища, `IEntity<T>`), используется `Id`. `EntityIdentifierFactory`,
  `EntityIdentifierNameProvider` и `EntityNames` остаются (журнал событий), но работают с `Id`.
- Генераторы и `AddIdentifiersGenerators()` удаляются: use case'ы, producer и тесты вызывают `XIdentifier.Generate()`.
  В `Freem.Locking.Redis` токен блокировки становится обычной строкой (`Guid.NewGuid().ToString("N")`), а
  `SimpleLockIdentifier` и его регистрация удаляются.
- JSON: конвертеры идентификаторов → `ParsableJsonConverter<T>` (`Utilities.Serialization.Json` 0.0.1 скопировать в
  `_Src/Freem/packages/`); `EventJsonConverter` и конвертер коллекций связей остаются, но строят Id через `Parse`.
  Формат JSON не меняется.
- Мапперы хранилища и контроллеры создают Id через `Parse`; переделка самих контроллеров — этап 6.
- `Freem.Identifiers` и `Freem.Identifiers.UnitTests` удаляются; тесты базового класса (`StringIdentifierTests`)
  переносятся в `Freem.Domain.UnitTests` как тесты `Id`.

Решения пользователя (2026-10-19):

1. **Имена — как в StashIt, сразу:** `TagIdentifier` → `TagId` (так же `ActivityId`, `RecordId`, `RunningRecordId`,
   `UserId`, `EventId`), файлы из `*/Identifiers/` переезжают в папку агрегата.
2. **`RunningRecordId` — вариант (а):** тип остаётся, неявные преобразования заменяются явными методами
   (`FromUserId(UserId)`, `ToUserId()`), `Generate()` у него нет.
3. **Value objects — в этом этапе:** `IParsable<T>` (`Parse` бросает `FormatException`) и `ParsableJsonConverter<T>`
   для value objects домена и периодов `Freem.Time`.
4. **Отступления от StashIt подтверждены:** `Id.Equals` учитывает тип, `TryParse` отбрасывает строку из пробелов.

5. **Производные имена переименовываются сразу** (вариант (б)); соответствие — в плане реализации, шаг 1. Имена
   свойств DTO API слова `Identifier` не содержат (`Tags`, `Activities`, `Id`), поэтому формат JSON переименование не
   меняет.
6. **Value objects — предложенный вариант:**
   - строковые (`ActivityName`, `RecordName`, `RecordDescription`, `TagName`, `Nickname`, `EventAction`): закрытый
     конструктор, создание только через `Parse`/`TryParse`;
   - `ActivityStatus`, `DayUtcOffset`, `DatePeriod`, `DateTimePeriod`: конструкторы из типизированных значений и
     `ActivityStatus.Active`/`Archived` остаются, `IParsable` добавляется ради строкового формата;
   - неявные преобразования убираются у всех;
   - нынешние правила валидации и формат JSON сохраняются, без обрезки пробелов из StashIt;
   - `Freem.Time.Serialization.Json` удаляется, `ParsableJsonConverter` для периодов регистрируется в
     `EntitiesJsonSerialization`.
7. **Баг `TagName` исправляется сейчас:** пустое имя и имя из пробелов отклоняются, как у `ActivityName`.

## Явно не входит в этот файл (пока)

`Result`/`RepositoryContext` и типизированные ошибки вместо `NotFoundException`/`NotFoundRelatedException` (этап 5);
упрощение контроллеров и привязка Id из маршрута (этап 6); переименование `Freem.Entities.Serialization.Json`,
хранилища и прочих проектов (этап 7); переход событий на `EntityRef`/`EntityType` (отдельного этапа нет).

## Критерий готовности

- `Freem.Identifiers` и `Freem.Identifiers.UnitTests` удалены; в коде нет `IIdentifier`, `IEntityIdentifier`,
  `IMultipleEntityIdentifier`, генераторов идентификаторов и `AddIdentifiersGenerators()`.
- Все доменные идентификаторы — `sealed` наследники `Id` с закрытым конструктором, `IParsable<T>` (`Parse` бросает
  `FormatException`), без неявных преобразований; `Generate()` у тех, что создаются в домене.
- Value objects домена и периоды `Freem.Time` реализуют `IParsable<T>`, неявных преобразований нет.
- JSON-конвертеры идентификаторов и value objects — `ParsableJsonConverter<T>`. Формат JSON API не изменился, только
  невалидное значение в запросе теперь даёт 400, а не исключение из конструктора.
- `dotnet build Freem.sln` — 0 ошибок, 0 предупреждений; unit-тесты зелёные; `docker/test` зелёный (запускает
  пользователь).
- Схема БД (миграции, снимок модели) не меняется.

## План реализации

Каждый шаг: `dotnet build Freem.sln` — 0 ошибок, 0 предупреждений, unit-тесты зелёные. Кодировку (BOM) файлов
сохранять как в HEAD, лишние `using` в затронутых файлах убирать через `dotnet format` (IDE0005).

1. **Переименование без изменения поведения.** Шесть типов `*Identifier` → `*Id`, файлы из `*/Identifiers/` — в
   папку агрегата (пространство имён `Freem.Domain.Tags` и т. д.), `*/Identifiers/Extensions/` → `*/Extensions/`.
   Производные имена:
   - коллекции связей: `IReadOnlyRelatedEntitiesIdentifiersCollection<T>` → `IReadOnlyRelatedIdsCollection<T>`,
     `IRelatedEntitiesIdentifiersCollection<T>` → `IRelatedIdsCollection<T>`, `RelatedEntitiesIdentifiersCollection<T>`
     → `RelatedIdsCollection<T>`, `RelatedActivitiesIdentifiersCollection` → `RelatedActivityIdsCollection`,
     `RelatedTagsIdentifiersCollection` → `RelatedTagIdsCollection`, свойство `Identifiers` → `Ids`;
   - `ActivityAndUserIdentifiers`/`RecordAndUserIdentifiers`/`TagAndUserIdentifiers` → `*AndUserIds`
     (`Domain.Storage/Models/Identifiers/` → `Models/Ids/`);
   - `EntityIdentifierFactory` → `EntityIdFactory`, `EntityIdentifierNameProvider` → `EntityIdNameProvider`,
     `AddEntitiesIdentifiersNameConverters` → `AddEntityIdNameConverters`, `HasTagsIdentifiers`/`HasActivitiesIdentifiers`
     → `HasTagIds`/`HasActivityIds`, `DatabaseColumnToIdentifierPossibleConverter` → `DatabaseColumnToIdPossibleConverter`;
   - generic-параметры (`TEntityIdentifier` → `TEntityId` и т. п.), локальные переменные, параметры и поля
     (`identifiers` → `ids`), имена в тестах (`CreateIdentifier` → `CreateId`, `GenerateUserIdentifier` →
     `GenerateUserId`, `*RelatedIdentifiers` → `*RelatedIds`, моки `TestIdentifier`/`SampleIdentifier` → `TestId`/`SampleId`).
   Типы `Freem.Identifiers` и `IEntityIdentifier` на этом шаге не трогаются, они уходят на шаге 2.
2. **Идентификаторы по образцу StashIt.** Базовый `Freem.Domain.Id` (сравнение с учётом типа); шесть `sealed`
   наследников с закрытым конструктором, `IParsable<T>`, без неявных преобразований; `Generate()` у всех, кроме
   `RunningRecordId` (у него `FromUserId`/`ToUserId`). `IIdentifier`, `IEntityIdentifier`, `IMultipleEntityIdentifier`
   → `Id`; генераторы и `AddIdentifiersGenerators()` удалить, вызывающие (use case'ы, producer, тесты) → `Generate()`;
   `Freem.Locking.Redis` — токен блокировки строкой, `SimpleLockIdentifier` удалить. Мапперы, запросы EF и
   контроллеры — `Parse`/`ToString()`. Конвертеры идентификаторов → `ParsableJsonConverter<T>` (скопировать
   `Utilities.Serialization.Json` 0.0.1 в `packages/`). Удалить `Freem.Identifiers`, `Freem.Identifiers.UnitTests`
   (тесты базового класса → `Freem.Domain.UnitTests` как `IdTests`, плюс тесты `Parse`/`TryParse`).
3. **Value objects.** Решение 6 и исправление `TagName`; периоды `Freem.Time` — `IParsable` вместо собственного
   `TryParse` (вызовы в model binder'ах API обновить); удалить `Freem.Time.Serialization.Json`; конвертеры value
   objects → `ParsableJsonConverter<T>`. Тесты `Parse`/`TryParse` на граничных значениях, регрессионный тест на пустое
   имя тега.
4. **Итог.** Поиск строковых ссылок на удалённые проекты и типы по `_Src/Freem`, `docker/`, `CLAUDE.md`; проверка, что
   миграции и снимок модели не изменились и что имена свойств DTO не изменились; обновить `CLAUDE.md`, если он
   упоминает удалённое; `docker/test` запускает пользователь.

## Статус и изменения

- 2026-10-19 (Claude Code, claude-opus-5-5): исследование согласовано пользователем (решения 1–7 выше, этап расширен
  на value objects); план реализации записан, реализация начата.
- 2026-10-19 (Claude Code, claude-opus-5-5): шаги 1–4 выполнены. `Freem.Identifiers`, `Freem.Identifiers.UnitTests`,
  `Freem.Time.Serialization.Json` удалены (`.csproj` — 34, было 37); идентификаторы — `sealed` наследники
  `Freem.Domain.Id` с `IParsable`, value objects и периоды `Freem.Time` — `IParsable`, неявных преобразований нет;
  все конвертеры этих типов — `ParsableJsonConverter<T>` (`Utilities.Serialization.Json` 0.0.1 скопирован в
  `_Src/Freem/packages/`). `dotnet build Freem.sln` — 0 предупреждений, 0 ошибок; unit-тесты зелёные (5 проектов;
  `Freem.Domain.UnitTests` 52 → 150, `Freem.Time.UnitTests` 107 → 122). Миграции и снимок модели не изменились (в
  `ScriptExtractorFactory` — только имена классов коллекций); имена свойств DTO не изменились. Формат JSON проверен
  круговой сериализацией одних и тех же 15 образцов (Id, value objects, периоды, `DayUtcOffset`, события,
  `UpdateField`) сборкой из HEAD и текущей: вывод совпадает побайтно. Невалидное значение в JSON теперь даёт
  `FormatException` (в ASP.NET — 400), в HEAD было `ArgumentException`. `docker/test` не запускался, ждёт
  пользователя; не закоммичено. Расхождения с планом и решения по ходу:
  - `EntityEvent` принимает действие строкой и сам вызывает `EventAction.Parse`: 20 событий передают константы
    `*EventActions`, иначе в каждом пришлось бы писать `Parse`;
  - у `ActivityStatus` добавлен `ToValue()` (свойство `Value` конфликтует с вложенным enum `Value`), у `DayUtcOffset` —
    свойство `Value` (`TimeSpan`) и `ToString()` в формате `"c"`, совпадающем с форматом `TimeSpan` в System.Text.Json;
  - у `Nickname` не было `ToString()`: значение отдавал неявный оператор, без него в БД и JSON ушло бы имя типа;
    `ToString()` добавлен;
  - `DatePeriod.TryParse`/`DateTimePeriod.TryParse` возвращают `false`, если начало позже конца (раньше бросали
    `ArgumentException` из конструктора);
  - `Freem.Locking.Redis`: `SimpleDistributedLock.Identifier` → `Token` (строка);
  - хранилище: в запросах EF Id сравнивается со строковой локальной переменной (`idString`, `userIdString` и т. п.),
    вычисленной до запроса; фабрики связей запущенной записи принимают `RunningRecordId` вместо `UserId`;
  - `UseCaseContextProvider` берёт Id пользователя через `UserId.TryParse` (пустой claim → пустой контекст, как раньше);
  - модели пар Id в `Domain.Storage` — в `Models/Ids/`; мок `TestId` сохранил `IEquatable<TestId>` (нужен
    `UnorderedEquals`), для тестов сравнения добавлен второй мок `OtherTestId`;
  - в `CLAUDE.md` обновлено число проектов;
  - находка вне этапа (не исправлялась): у `DayUtcOffset` нет сравнения по значению, поэтому
    `UserSettingsEqualityComparer` сравнивает ссылки и `UsersRepository.UpdateAsync` всегда записывает настройки, даже
    без изменений (данные не портятся, лишняя запись).
- 2026-10-19 (Claude Code, claude-opus-5-5): `docker/test` зелёный у пользователя; критерий готовности выполнен; по просьбе пользователя запущена независимая проверка отдельным агентом.
- 2026-10-19 (Claude Code, claude-opus-5-5): независимая проверка отдельным агентом: APPROVE WITH NOTES, блокирующих
  замечаний нет. По решению пользователя исправлены: удалён оставшийся `implicit operator TimeSpan(DateTimePeriod)`
  (не использовался), в `EventJsonConverter.Write` значения вынесены в локальные переменные, убран лишний `!` у
  `id.ToString()` в `RelationsDatabaseContextExtensions`, в `CLAUDE.md` в список пакетов локального фида добавлен
  `Serialization.Json`. Оставлены как есть по решению пользователя: чтение из БД тегов с пустым именем (до этапа
  принимались из-за бага) упадёт на `TagName.Parse`; поиск тега по имени из пробелов в `FindTagByNameController` теперь,
  вероятно, отвечает 500 вместо 404 (отложено); утверждение «невалидное значение в JSON → 400» тестом не проверено;
  формат записей статуса. Сборка — 0 предупреждений, 0 ошибок, unit-тесты зелёные.
