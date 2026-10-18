# Этап: Общие библиотеки

Автор: Claude Code, claude-sonnet-5-5, 2026-10-18

См. общий контекст и место этого этапа: `plans/freem-revival/general/roadmap.md` (этап 4).

## Зачем именно так

В Freem 26 собственных мелких библиотек `Freem.*`; часть из них дублирует пакеты `Utilities.*` из CSharpUtilities,
которыми пользуются StashIt и другие проекты. Цель этапа — не заменить всё подряд, а по каждой библиотеке принять
решение «заменить / оставить / удалить» и выполнить замены, где аналог действительно эквивалентен. Прежде чем
менять, нужно согласовать с пользователем спорные случаи (отличается поведение или форма API), поэтому этап
начинается с таблицы решений.

## Зафиксированные факты

Проверено 2026-10-18 чтением кода (сборка и тесты в рамках исследования не запускались).

**Механизм получения пакетов (как в StashIt).** Пакеты нигде не публикуются: CSharpUtilities собирает `.nupkg`
(`docker/pack`, выход — `docker/out/packages`), потребитель кладёт их в каталог `packages/` рядом с решением,
`Directory.Build.props` (идентичный в StashIt и CSharpUtilities) добавляет этот каталог в
`RestoreAdditionalProjectSources`. В StashIt `packages/*` в `.gitignore`, в git лежит только `packages/.gitkeep`;
то есть чистый клон без копирования `.nupkg` не восстанавливается. В Freem ни `Directory.Build.props`, ни
`packages/` нет.

**Доступные пакеты** (теги в CSharpUtilities): `Utilities.Clock` 0.0.2, `Utilities.Errors` 0.0.1,
`Utilities.Errors.Serialization.Json` 0.0.1, `Utilities.Http` 0.0.8, `Utilities.Http.SystemNet` 0.0.8,
`Utilities.Primitives` 0.0.3, `Utilities.Serialization.Json` 0.0.1, `Utilities.UseCases` 0.1.0,
`Utilities.UseCases.Audit` 0.0.1; без тега, но в репозитории: `Utilities.Collections` (net9.0),
`Utilities.Storage.Transactions` и `.EFCore` (net9.0, EF Core 9.0.*).

**Сопоставление.** Размер — число `.cs`-файлов; «исп.» — число проектов, ссылающихся на библиотеку.

| Freem | Размер / исп. | Аналог | Что отличается |
|---|---|---|---|
| `Freem.Time` — `ICurrentTimeGetter`, `UtcCurrentTimeGetter` | 7 файлов кода используют интерфейс | `Utilities.Clock` (`IClock`, `UtcClock`) | `UtcCurrentTimeGetter` обнуляет миллисекунды (`DateTimeOperations.EraseMilliseconds`), `UtcClock` отдаёт `UtcNow` как есть; регистрации в DI в пакете нет |
| `Freem.Time` — модели (`DatePeriod`, `DateTimePeriod`, `Month`…), расширения | 14 файлов, 16 проектов | нет | остаётся |
| `Freem.Time.Serialization.Json` | 2 файла | нет | остаётся |
| `Freem.Results` — `Result<T>` (без ошибки) | 1 файл, 10 мест использования | `Utilities.Primitives` (`Result<TError>`, `Result<TValue,TError>`) | другая форма: у Freem результат без типа ошибки |
| `Freem.Collections` | 2 файла, 4 проекта | `Utilities.Collections` | в пакете только `With`; `ExceptMutual`/`UnorderedEquals` Freem в нём нет |
| `Freem.Storage.Abstractions` + `Storage.EFCore` | 5 + 3 файла | `Utilities.Storage.Transactions(.EFCore)` | у Freem есть `IStorageTransactionExceptionHandler` и `TryCommit/TryAbort`, у пакета нет; пакет `.EFCore` собран на EF Core 9, в Freem EF Core 10 |
| `Freem.Http.Requests` | 13 файлов, 2 проекта (оба клиента API), 30 мест использования | `Utilities.Http` + `Utilities.Http.SystemNet` | типы называются так же (`HttpRequest`, `QueryParameters`, `IHttpClient`), состав и сигнатуры не сверены |
| JSON-конвертеры в `Freem.Entities.Serialization.Json` | ~17 конвертеров | `Utilities.Serialization.Json.ParsableJsonConverter<T>` (для `IParsable<T>`) | часть конвертеров Freem пишет значение иначе (например, `TagName` через неявное приведение); какие из них сводятся к `IParsable`, не проверено |
| `Freem.Clones`, `Freem.Enums`, `Freem.Exceptions`, `Freem.Converters`, `Freem.Identifiers`, `Freem.Reflection`, `Freem.Sorting`, `Freem.TextProcessing`, `Freem.Validation`, `Freem.Timeouts`, `Freem.Configurations`, `Freem.EFCore.Extensions`, `Freem.DependencyInjection.Microsoft`, `Freem.Locking.*`, `Freem.Storage.Migrations`, `Freem.AutoFixture` | — | нет | остаются (кроме возможного пересмотра в архитектурном плане) |

**Мусор.** Каталог `Freem.Linq` — только `bin/obj`, ни одного трекаемого файла (не в решении, не в git).

## Один-два иллюстративных примера (не исчерпывающий список!)

- `Freem.Time/Implementations/UtcCurrentTimeGetter.cs`: замена на `UtcClock` меняет точность времён записей
  (миллисекунды перестанут обнуляться) — это поведение, а не механика.
- `Freem.Results/Result.cs`: `Result<T>` без ошибки против `Result<TValue, TError>` из `Utilities.Primitives`.

## Направление решения (не пошаговая реализация)

1. Сначала механизм: `Directory.Build.props` + `packages/` (в `.gitignore`, `.gitkeep`) по образцу StashIt; описать в
   `CLAUDE.md`, откуда брать `.nupkg`. Проверить влияние на `Dockerfile` API (в контекст сборки нужен `packages/`).
2. Дальше по каждой строке таблицы — решение пользователя. Рекомендации исполнителя (на согласование):
   - `Utilities.Http` + `Http.SystemNet` вместо `Freem.Http.Requests` — самая «чистая» замена (пересекающиеся
     имена типов), но требует сверки API;
   - `Utilities.Clock` вместо `ICurrentTimeGetter` — только если решено, что обнуление миллисекунд не нужно
     (или добавить его в пакет / обернуть);
   - `ParsableJsonConverter<T>` — для тех конвертеров, которые сводятся к `Parse`/`ToString`;
   - `Result<T>` → `Utilities.Primitives` — меняет форму результата в 10 местах, вероятно, лучше отложить до
     `plans/freem-architecture` (UseCases);
   - `Storage.Transactions` — пакет беднее и на EF 9; без доработки пакета заменять нельзя;
   - `Freem.Collections` оставить; кандидаты на вынос в CSharpUtilities — отдельное решение;
   - удалить мусорный `Freem.Linq`.
3. Если замена требует правки самого CSharpUtilities (новая версия пакета, тег `<PackageId>/v<Version>`), это
   отдельное согласование с пользователем.

Открытые вопросы к пользователю:

- Какие строки таблицы входят в этот этап, а какие откладываются (в частности `Result<T>`, транзакции, время)?
- Нужны ли миллисекунды в `Record`-временах (определяет судьбу `ICurrentTimeGetter`)?
- Менять ли CSharpUtilities (доработка пакетов под Freem) или только потреблять существующие версии?

## Явно не входит в этот файл (пока)

План реализации по шагам; точные API-различия `Http.Requests`/`Utilities.Http` и конвертеров JSON (проверяются
после выбора строк таблицы).

## Критерий готовности

- `dotnet build Freem.sln` без ошибок и предупреждений; `dotnet test Freem.sln` — unit-тесты зелёные.
- Для каждой строки таблицы зафиксировано решение; выбранные замены выполнены, замещённые проекты удалены из
  решения и с диска, ссылки на них в `.csproj` и `using` отсутствуют.
- Пакеты `Utilities.*` восстанавливаются из `packages/`; в `CLAUDE.md` описано, откуда их брать.
- Схема БД не меняется.

## План реализации

Решения пользователя: `Freem.Storage.*` и `Freem.Results` остаются; `Utilities.Collections` и `Utilities.Clock` доработаны
отдельным агентом (готово, см. «Статус»); `WithQueryParameterIfNotNull` заменяется локальным `if`; `Freem.Linq` удаляется.

Проверено после доработки (2026-10-18): `.nupkg` лежат в `~/Sources/CSharpUtilities/docker/out/packages/`:
`Utilities.Collections` 0.1.0 (net10.0; `ExceptMutual`, `UnorderedEquals`, `NullableUnorderedEquals`),
`Utilities.Clock` 0.1.0 (`UtcTruncatedToSecondsClock`, `AddUtcTruncatedToSecondsClock`), `Utilities.Http` 0.0.8,
`Utilities.Http.SystemNet` 0.0.8. Отклонение от требования: `AddUtcTruncatedToSecondsClock` использует `AddSingleton`, а не
`TryAddSingleton`; в Freem регистрация вызывается один раз, повторных вызовов нет.

Находка по JSON-конвертерам: ни один тип значений Freem не реализует `IParsable<T>` (нет и `Parse`), конвертеры работают
через неявные операторы приведения к `string`. `ParsableJsonConverter<T>` применим только после добавления `IParsable<T>`
(`Parse`/`TryParse`) в ~10 value object'ов — это правка домена, не механическая замена. Решение пользователя: конвертеры не трогаем (пункт 6).

1. **Механизм пакетов.** `_Src/Freem/Directory.Build.props` (как в StashIt), `_Src/Freem/packages/.gitkeep`, исключение
   `_Src/Freem/packages/*` (кроме `.gitkeep`) в `.gitignore`; скопировать четыре `.nupkg` в `packages/`. `Dockerfile` API уже
   копирует `./` целиком, отдельной правки не требует. Проверка: `dotnet restore Freem.sln`.
2. **Collections.** Заменить ссылки на `Freem.Collections` (4 `.csproj`) на `PackageReference Utilities.Collections 0.1.0`,
   `using Freem.Collections.Extensions` → `using Utilities.Collections`; удалить `Freem.Collections` и
   `Freem.Collections.UnitTests` (тесты перенесены в CSharpUtilities) из решения и с диска. Проверка: `dotnet build`.
3. **Clock.** `ICurrentTimeGetter` → `IClock` (`Utilities.Clock.Abstractions`) в 2 контроллерах и `TestContext.cs`;
   `AddUtcCurrentTimeGetter()` → `AddUtcTruncatedToSecondsClock()` в `Program.cs` и `TestContext.cs`. Из `Freem.Time` удалить
   `Abstractions`, `Implementations`, `DependencyInjection` (и ненужную зависимость от DI-абстракций); удалить тест регистрации в
   `Freem.Time.UnitTests`. Тест `AddUtcCurrentTimeGetter_ShouldAddGetterOnlyOneTime_…` в
   `Freem.Entities.Storage.PostgreSQL.UnitTests` удалить целиком (решение пользователя: в таком виде он не нужен).
   `DateTimeOperations` и модели времени остаются. Проверка: `dotnet build`, `dotnet test`.
4. **Http.** В `Web.Api.Public.Client` и `SyncClient` заменить `Freem.Http.Requests` на `Utilities.Http` 0.0.8 +
   `Utilities.Http.SystemNet` 0.0.8; `using`-ы; `ReadJsonAsAsync` → `ReadFromJsonAsync`; `AddNetHttpClient` →
   `AddSystemNetHttpClient`; единственный вызов `WithQueryParameterIfNotNull` → локальный `if`. Удалить `Freem.Http.Requests`.
   Проверка: `dotnet build`, `dotnet test` (unit); поведение клиентов — по `FunctionalTests`, если среда доступна.
5. **Мусор.** Удалить каталог `Freem.Linq` (только `bin/obj`, не трекается).
6. **JSON-конвертеры** — не входят в этап (решение пользователя): остаются как есть, к ним вернёмся в `plans/freem-architecture`.
7. **Итог.** Обновить `CLAUDE.md` (раздел «Repository Layout»/«Local Environment»: откуда брать `.nupkg`, список `Utilities.*`);
   `dotnet build Freem.sln` без предупреждений, `dotnet test Freem.sln` (unit зелёные).

## Статус и изменения

- 2026-10-18 (Claude Code, claude-sonnet-5-5): план одобрен пользователем (JSON-конвертеры вне этапа, тест репозиториев в `PostgreSQL.UnitTests` удалён); шаги 1–5 и 7 выполнены, не закоммичено. Подключена локальная лента `_Src/Freem/packages/` (`Directory.Build.props`; правка `_Src/.gitignore` — исключение для `.gitkeep`, корневого `.gitignore` нет). `Freem.Collections`(+тесты) → `Utilities.Collections` 0.1.0; `ICurrentTimeGetter` → `IClock` (`AddUtcTruncatedToSecondsClock`), из `Freem.Time` удалены абстракция, реализация и DI; `Freem.Http.Requests` → `Utilities.Http`/`Http.SystemNet` 0.0.8 (`WithQueryParameterIfNotNull` заменён локальным `if`, заодно вынесены `ToString()`-локали). Отклонения: (1) `Utilities.Clock.0.1.0.nupkg` в `docker/out/packages/` оказался устаревшим (нет `AddUtcTruncatedToSecondsClock`) — позже пересобран (см. запись о проверке); (2) `Freem.Linq` уже был удалён на этапе 1, на диске лежали только неотслеживаемые `bin/obj` (стёрты). `dotnet build Freem.sln`: 0 предупреждений, 0 ошибок; unit-тесты зелёные, интеграционные/функциональные падают без БД и Redis (не критерий до этапа 5). `CLAUDE.md` обновлён. Ожидает проверки и закрытия пользователем.
- 2026-10-18 (Claude Code, claude-sonnet-5-5): независимая проверка отдельным агентом: APPROVE WITH NOTES (блокирующих нет). Пользователь решил: пустую `ItemGroup` в `Freem.Time.csproj` удалить, порядок `using` поправить, `PackageReference` вынести из групп с `ProjectReference` (слиты с существующими группами пакетов или вынесены в отдельную), устаревший `Utilities.Clock.0.1.0.nupkg` пересобрать самому, `AddSingleton` вместо `TryAddSingleton` оставить как есть, проверку «схема БД не меняется» записать. Выполнено: пакет `Utilities.Clock` 0.1.0 пересобран из исходников CSharpUtilities (`dotnet build -c Release --no-incremental` + `pack`; причиной была устаревшая `obj` Release) и положен в `docker/out/packages/` и `_Src/Freem/packages/`; правки по замечаниям внесены, `dotnet build Freem.sln`: 0 предупреждений, 0 ошибок, unit-тесты зелёные. Схема БД не менялась: миграции не тронуты, в `PostgreSQL/Database/Entities/*` и `TriggerConstraintError.cs` изменены только `using`.
- 2026-10-18 (Claude Code, claude-sonnet-5-5): этап 4 закрыт пользователем; реализация закоммичена (a3d6f8f).
