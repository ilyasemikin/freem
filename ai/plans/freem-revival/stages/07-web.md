# Этап: Фронтенд

Автор: Claude Code, claude-sonnet-5-5, 2026-10-18

См. общий контекст и место этого этапа: `ai/plans/freem-revival/general/roadmap.md` (этап 7).

## Зачем именно так

Решение пользователя: фронтенд остаётся на Vite + PrimeReact (переход на Mantine/TanStack Query, как у Trandoma,
не рассматривается). Поэтому этап — не переписывание, а актуализация: зависимости, линтер, структура каталогов,
соответствие API, которые после этапов 1–6 остались в «шаблонном» состоянии.

## Зафиксированные факты

Проверено запуском инструментов и чтением кода `source/Freem.Web`.

- `npm run build` проходит; единственное предупреждение сборки — чанк `index-*.js` 616 кБ (> 500 кБ), код-сплиттинга нет.
- `npm run lint` — 0 ошибок, 3 предупреждения `react-hooks/exhaustive-deps` (перенесены с этапа 1):
  `ActivitiesDashboard.tsx:75`, `RecordsDashboard.tsx:42`, `StatisticsDashboard.tsx:37` (в зависимостях эффектов нет
  `cache.*` и `unit`).
- Зависимости (`npm outdated`), установленное → доступное в пределах диапазона `package.json` / последнее:
  `react`/`react-dom` 19.0.0 → 19.3.1; `react-router` 7.4.0 → 7.18.4 (8.4.0); `primereact` 10.9.3 → 10.9.9 (11.2.0);
  `primeicons` 7.0.0 (8.0.2); `chart.js` 4.4.9 → 4.5.1; `vite` 6.2.3 → 6.4.4 (8.3.4); `@vitejs/plugin-react` 4.3.4 →
  4.7.0 (6.1.2); `typescript` 5.7.3 (7.0.2); `eslint`/`@eslint/js` 9.23.0 → 9.39.5 (10.x); `eslint-plugin-react-hooks`
  5.2.0 (7.1.1); `typescript-eslint` 8.28.0 → 8.71.2; `globals` 15.15.0 (17.13.0). Для сравнения, Trandoma.Web:
  vite ^8.2, `@vitejs/plugin-react` ^6.0, typescript ~6.0, react ^19.2, react-router-dom ^7.18, линтер `oxlint`.
- `Dockerfile` собирает на `node:22` и отдаёт статику через `nginx:1.29-alpine`; `dist` в `.gitignore`.
- Структура `src/`: страницы (`*Dashboard.tsx`) и их части (`*Panel`, `*Editor`, `*UpdateModal`, `*AddModal`,
  `*List`, `Statistics*Chart`) лежат плоско в корне `src/` (31 файл, ~2200 строк в `.tsx` корня); отдельно
  `clients/`, `components/` (+ `inputs/`), `contexts/` (auth, cache, clients, dialog), `data/`, `entities/`, `helpers/`.
  Trandoma.Web для сравнения: `app/`, `authorization/`, `clients/`, `components/`, `pages/`.
- Маршруты в `main.tsx`: `/`, `/activities`, `/records`, `/records/statistics`, `/tags` (`createBrowserRouter`).
- Соответствие API: все URL клиентов (`activities`, `activities/by-name`, `records`, `records/by-period`,
  `records/running[/start|/stop]`, `statistics/per-days|per-period`, `tags`, `user/me|register|settings`,
  `auth/register|login|logout`) есть среди маршрутов контроллеров; `auth/refresh` вызывается из `AuthSession.ts`.
  Точечная сверка методов и тел запросов — работа при реализации. Эндпоинта `api/v1/events` на фронтенде нет (функции
  для него в UI не было).
- Клиентская сторона авторизации (этап 6) уже на месте: `contexts/auth/*`, `clients/AuthClient.ts`,
  `clients/AuthSession.ts`, `LoginPage.tsx`; `HttpClient` подставляет `Authorization` и `credentials: "include"`.

## Один-два иллюстративных примера (не исчерпывающий список!)

- Исправление предупреждения lint: `RecordsDashboard.tsx:42` — эффект зависит от `cache.records`, но не перечисляет его.
- Структура: `ActivitiesDashboard.tsx`, `ActivityPanel.tsx`, `ActivityEditor.tsx`, `ActivityAddModal.tsx`,
  `ActivityUpdateModal.tsx`, `ActivitiesListPanel.tsx` — кандидаты на каталог `activities/` (аналогично `records/`,
  `tags/`, `statistics/`).

## Направление решения (не пошаговая реализация)

Предварительное, до ответов пользователя:

1. **Зависимости.** Обновить в пределах мажорных версий текущих диапазонов (react 19.3, react-router 7.18, primereact
   10.9.9, vite 6.4, eslint 9.39, typescript-eslint 8.71, chart.js 4.5) — низкий риск. Мажорные скачки (Vite 8,
   TypeScript 6/7, react-router 8, PrimeReact 11, ESLint 10, react-hooks 7) — отдельное решение, т. к. PrimeReact 11
   и react-router 8 меняют API и затрагивают весь UI.
2. **Линтер.** Оставить ESLint (после обновления) либо перейти на `oxlint` как в Trandoma; 3 предупреждения
   исправить в любом случае (стабильные ссылки/зависимости эффектов, без отключения правила).
3. **Структура каталогов.** Сгруппировать плоские файлы `src/` по областям (`pages/` или каталог на область:
   activities, records, tags, statistics) по образцу Trandoma; только перенос и правка импортов, без смены логики.
4. **Соответствие API.** Сверить типы запросов/ответов в `clients/models` с контрактами
   `Freem.Web.Api.Public.Contracts` (после этапов 2–6 они менялись), убрать неиспользуемое.
5. **Сборка.** Код-сплиттинг (ленивые маршруты, отдельный чанк для chart.js) или явное повышение
   `chunkSizeWarningLimit` — решить; `README.md` шаблона Vite заменить описанием проекта.

## Решения пользователя (2026-10-18)

Референс по стеку и структуре — `~/Sources/StashIt/Core/Stashit.Core.Web` (React 19.2, react-router 8, Vite 8,
TypeScript 6, ESLint 10 + `eslint-plugin-react-hooks` 7, `@vitejs/plugin-react` 6; PrimeReact остаётся 10.9.x;
структура `app/`, `pages/`, `components/`, `clients/`, плюс каталоги по возможностям: `authorization/`, `dialog/`,
`cache/`; код-сплиттинга нет).

1. Зависимости обновляются по максимуму, включая мажорные.
2. Линтер — по полезности и актуальности, с оглядкой на StashIt: остаётся ESLint, обновлённый до 10.
3. Структура каталогов — по образцу StashIt.
4. Код-сплиттинг: предупреждение влияет только на время первой загрузки (один большой JS-файл); для личного
   приложения это несущественно, в StashIt его нет. Не делаем; предупреждение остаётся.
5. Только техническая актуализация; внешний вид — позже.

Оговорка: PrimeReact 11.2 в StashIt не используется (там 10.9.7). «По максимуму» для PrimeReact проверяется на
шаге 1 плана: если 11.x ломает UI или несовместим с React 19.2/Vite 8, остаёмся на 10.9.x и пишем это в статус.

## Явно не входит в этот файл (пока)

Список затронутых файлов, точные версии пакетов, порядок переноса файлов.

## Критерий готовности

- `npm run build` без ошибок, `npm run lint` без предупреждений; `npm outdated` не показывает отставания в
  согласованных пределах.
- Структура `src/` соответствует согласованной; логика не изменена (переносы и правки импортов).
- Типы клиентов совпадают с контрактами API; приложение работает против API в `docker/service` (проверка в
  браузере — только по согласованию, см. `CLAUDE.md`).
- Изменений схемы БД и контрактов API нет.

## План реализации

Ждёт одобрения. Критерий каждого шага — `npm run build` и `npm run lint`; в браузере не проверяем (см. `CLAUDE.md`).

1. **Зависимости.** Обновить `package.json` до последних версий (React 19.3, react-router 8, Vite 8,
   `@vitejs/plugin-react` 6, TypeScript 6, ESLint 10 и плагины, typescript-eslint, chart.js 4.5, PrimeReact — см.
   оговорку выше), `npm install`, поправить `tsconfig*`, `eslint.config.js`, `vite.config.ts` и код под
   breaking changes (react-router 8, TS 6). Образец конфигов — StashIt. Проверка: build + lint.
2. **Предупреждения lint.** Исправить 3 `exhaustive-deps` (и новые правила `react-hooks` 7, если появятся) без
   отключения правил — стабильные ссылки/полные зависимости. Проверка: lint без предупреждений.
3. **Структура.** Разложить плоские файлы `src/` по образцу StashIt (`app/`, `pages/`, каталоги по возможностям и
   т. п.; точная раскладка — по ходу, с показом пользователю перед переносом), только `git mv` и правка импортов,
   без смены логики. Проверка: build + lint.
4. **Сверка с API.** Сравнить `clients/models` с `Freem.Web.Api.Public.Contracts`, исправить расхождения, убрать
   неиспользуемое. Проверка: build + lint.
5. **Docker и документация.** Базовый образ `Dockerfile` (node) под требования новых версий; `README.md` шаблона Vite
   заменить описанием проекта; при необходимости строка в `CLAUDE.md`. Проверка: `docker build` не запускаю
   (Docker недоступен агенту) — фиксирую как непроверенное.

## Статус и изменения

- 2026-10-18 (Claude Code, claude-sonnet-5-5): исследование записано, ожидает ответов на открытые вопросы и согласования направления; реализация не начата.
- 2026-10-18 (Claude Code, claude-sonnet-5-5): получены ответы пользователя (обновлять по максимуму, ESLint как в StashIt, структура по StashIt, без код-сплиттинга, только техническая актуализация), план реализации записан и ожидает одобрения; реализация не начата.
- 2026-10-18 (Claude Code, claude-sonnet-5-5): план одобрен пользователем; шаги 1–5 выполнены, этап ожидает проверки и закрытия. `npm run build` проходит, `npm run lint` — 0 ошибок и 0 предупреждений; `docker build` и поведение в браузере не проверялись. Отклонения и находки: (1) PrimeReact остался на 10.9.9 — 11.x имеет другое API (пути импорта и компоненты), потребовал бы переписать весь UI; StashIt тоже на 10.9.x; остальное обновлено до последних версий (React 19.3, react-router 8, Vite 8, TypeScript 6, ESLint 10, react-hooks 7); `package-lock.json` пересоздан с нуля из-за конфликта peer-зависимостей; (2) `react-hooks` 7 добавил ошибки (`set-state-in-effect`, `immutability`), поэтому изменена логика: `EntitiesCacheProvider` создаёт кэши один раз (`useMemo`) вместо пересоздания на каждом рендере, флаг `loading` в дашбордах теперь снимается после завершения загрузки (раньше сразу), `AuthSession.onSignedOut` стал методом-подпиской с отпиской при размонтировании, в `StatisticsDashboard` убрана очистка данных при смене периода (до прихода новых показываются прежние); (3) раскладка по StashIt: `app/router.tsx` вынесен из `main.tsx`, `pages/{home,login,activities,records,statistics,tags}`, `authorization/`, `cache/`, `clients/context/`, `dialog/` (`contexts/` удалён); (4) удалён неиспользуемый `ErrorResponse.ts` (поле `type` вместо `code` контракта); (5) `Dockerfile` — `node:24`, шаблонный `README.md` заменён, в `CLAUDE.md` react-router 8. Находка вне объёма (контракты API не меняются): `UpdateRecordRequest` на сервере не содержит `period`, а фронтенд (`RecordList.tsx`) отправляет его — изменение периода записи при редактировании молча не сохраняется; требует решения пользователя.
- 2026-10-18 (Claude Code, claude-sonnet-5-5): по решению пользователя расхождение с периодом записи исправлено в этом этапе, в том числе в API (изменение контракта, схема БД не затронута — хранилище уже сохраняло `Period` в `RecordsRepository.UpdateAsync`): поле `Period` (`UpdateField<DateTimePeriod>`) добавлено в `UpdateRecordRequest` API и use case'а (вместе с `HasChanges`), в `UpdateRecordUseCase` и маппинг `UpdateRecordController`; добавлены тесты `ShouldSuccess_WhenUpdatePeriod` (`RecordsUpdateUseCaseTests`, проверяет сохранённый период) и `Request_ShouldSuccess_WhenUpdatePeriod` (`UpdateRecordTests`). Фронтенд менять не потребовалось — он уже отправляет `period`. `dotnet build Freem.sln` — 0/0, unit-тесты зелёные; новые integration/functional тесты не запускались (нужен `docker/test`, запускает пользователь).
- 2026-10-18 (Claude Code, claude-sonnet-5-5): независимая проверка отдельным агентом: APPROVE WITH NOTES (блокирующих нет). Решения пользователя по замечаниям: (1) TypeScript 7.0.2 опробован — `tsc -b` и `vite build` проходят, но `npm run lint` падает: `typescript-eslint` 8.71.2 не поддерживает TS 7 (peer `<6.1.0`); возвращён TypeScript ^6.0.3 (оговорка наряду с PrimeReact 11: «последние версии» не относится к `typescript`, пока `typescript-eslint` не поддержит 7); (2) уточнение к шагу 4: сверка `clients/models` с контрактами API выполнялась по спискам полей и URL, а не полной проверкой каждого типа и сериализации; найдены и исправлены `period` и `ErrorResponse`; (3) исправлен порядок `using` в трёх C#-файлах; (4) функциональный тест `Request_ShouldSuccess_WhenUpdatePeriod` теперь читает запись через `GetAsync` и проверяет сохранённый период (не запускался — нужен `docker/test`); (5) в дашбордах добавлена защита от гонок ответов (флаг `current` в эффекте) и показ ошибки загрузки (`ValidationError`; в Records/Statistics — внутри страницы, в Tags/Activities — вместо неё), `AuthSession.onSignedOut` поддерживает несколько слушателей и возвращает функцию отписки. Остальные замечания пользователь счёл незначительными. После правок: `dotnet build Freem.sln` 0/0, `npm run build` проходит, `npm run lint` 0/0.
- 2026-10-18 (Claude Code, claude-sonnet-5-5): этап 7 закрыт пользователем; реализация закоммичена (`b9ca5e7..63ec692`).
