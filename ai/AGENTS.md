# Shared Agent Instructions

This file is the source of truth for repository instructions shared by Codex and Claude Code.
Maintain common guidance here; keep agent-specific integration details in the root entry files.

## User Authorization and Clarification

- Do not change code, configuration, documentation, files or environment state without the user's explicit
  instruction to make those changes. Questions, suggestions, ideas and requests for an opinion are discussion,
  not authorization to implement. In particular, "what do you think?", "I would suggest" and similar wording
  do not authorize changes. Respond to the discussion and wait for an explicit implementation instruction.
- Do not infer permission to execute from the user's intent, an implied next step or your own interpretation
  of a proposal. Permission to inspect or discuss is not permission to modify. Apply only the changes
  explicitly authorized; do not add adjacent improvements.
- If a request, answer, scope or authorization is ambiguous, or you are unsure how to interpret it, ask the
  user before taking the dependent action. Do not guess, invent missing requirements or silently choose an
  interpretation. A reply to a multi-part proposal is not consent to all parts.
- Never commit on your own initiative. Before committing, obtain the user's explicit permission for that
  commit, either through a direct instruction to commit or an affirmative answer to your permission question.
  Permission to edit, test or prepare a commit message is not permission to commit. Earlier commit permission
  does not authorize later commits or additional changes.

## Project Overview

Freem is a personal time tracker: the user explicitly starts an activity (a running record), stops it, and the
record is stored in a database for later analysis. Entities: users, activities, tags, records, running records,
statistics, events. The project was dormant from 2025-06 and is being revived; the sibling project
`../StashIt` is the reference for conventions, structure and workflow.

## Git Commits

Commits follow [Conventional Commits v1.0.0](https://www.conventionalcommits.org/en/v1.0.0/): `type(scope): description`.
Types: `feat`, `fix`, `refactor`, `chore`, `docs`, `test`. Scope: the affected project name (`Freem.Api.Public`,
`Freem.Domain`, `Freem.Web`, ...), or `Freem` for cross-cutting changes. Description: lowercase, imperative,
concise, no trailing period. History before the revival does not follow this convention.

Do not add `Co-Authored-By` or any other authorship trailers, nor a "Generated with Claude Code" footer, to
commits.

**When to commit:** only when the user explicitly asks. Permission covers exactly the work it was given for;
for any later change, stop, show the result and wait for a fresh go-ahead. Never commit in the same turn in which
the change was first written unless the user asked for that commit in advance. Do not run `git push`.

## Planning Documents

Large, multi-stage initiatives get a persistent plan under `ai/plans/` (not `docs/`). Either one flat file or
`ai/plans/<slug>/general/roadmap.md` + `ai/plans/<slug>/stages/NN-<stage>.md`. Conventions are formalized as project skills
in `ai/skills/` (`roadmap`, `roadmap-stage`), copied from StashIt. Before writing or continuing a
plan, read the matching `SKILL.md`, even if it did not trigger automatically. Agents must read the
file directly if the skill is not available through their skill integration. Plans are written in Russian. Effort is estimated by
code impact (point fix vs structural change, review volume), not in days/weeks.

## Code Style

- No expression-bodied methods or operators: always a block body with braces (properties may use `=> expr`).
- No object construction nested deeper than one level: extract intermediate values (including `.ToString()` /
  `.Parse()` results passed as arguments) into named locals before the call or fluent chain.
- Names are self-sufficient, without prefixes redundant with the containing project/folder.
- Never expose `Dictionary<string, string>` as a domain type: wrap key-value data in a value object.
- `Parse` of value objects throws `FormatException` on bad input (so ASP.NET/System.Text.Json return a clean 400).

## Local Environment

- Sibling repositories live at `~/Sources/<Name>`: `StashIt` (reference project), `EgoHub` (auth service),
  `Trandoma`, `CSharpUtilities` (shared `Utilities.*` packages, tag `<PackageId>/v<Version>` on every version
  bump). Scope searches to the repo or these directories; never `find`/`grep` from `/` or `$HOME`.
- Docker Compose is assumed to be already running where it exists; do not start or restart environments unless
  asked. Docker socket may be inaccessible from the agent session — the user runs `docker compose` themselves.
- Host port pool: each project has a 50-port range (StashIt 10000-10049, EgoHub 10050-10099, Trandoma
  10100-10149). Freem takes **10150-10199** (confirmed by the user): `docker/develop` 10150-10169, `docker/service` 10170-10189 (reserved; no published ports), local
  `dotnet run`/misc 10190-10199. Never bind default ports (5432, 6379, 9092, ...). Runs from worktrees take ports
  from **40000-40100** (allowed by the user).
- Parallel features: git worktrees in `.claude/worktrees/<name>`, created, set up and removed per
  `ai/skills/worktree/SKILL.md` (agents must read it).
- Local secrets in tracked config (`appsettings.Development.json`, `configuration.json`) are hidden with
  `git update-index --skip-worktree <file>`, never `.gitignore`. `git stash` and `git checkout -- <path>` clear
  the bit: re-check with `git ls-files -v` (`S` = set, `H` = cleared).
- EgoHub is started manually by the user and may be down; ask the user to start it.
- Kill only processes you started yourself, by PID, never by name grep.
- Docker environments live in `docker/{develop,service,test}` (PostgreSQL 18, Redis, `postgres-migration` container
  built from `Freem.Domain.Storage.PostgreSQL.Migrations/Dockerfile`; `service` adds API, web and manual
  `postgres-backup`/`postgres-restore`). `.env` is created from `.env.example` by the user. Ports: `develop` Postgres
  10150, Redis 10151; `service` API and web use an external reverse proxy network (required
  `PROXY_NETWORK_NAME`, no default) with aliases `freem-api:8080` and `freem-web:80`, without published host ports;
  local `dotnet run` API 10190, `npm run dev` 10191 (strict).
  Images need `.nupkg` files in `source/packages/`.
- `.IntegrationTests` (storage, Redis, use cases, API) run in `docker/test` (`docker compose up --build --abort-on-container-exit
  test`, then `down -v`; no ports published, test configs in `docker/test/configs/`; EgoHub address and token come from `docker/test/.env` (see `.env.example`); each test assembly has its own database `freem_{storage,usecases,api}_test`, because assemblies run in parallel and truncate tables). `TruncateTables` refuses a
  database whose name does not end with `_test`. The committed `configuration.json` files of the test projects hold
  empty placeholders for local runs against `develop` (use a `*_test` database).
- Do not spend time on manual browser passes once build and tests pass; ask first. When the user agrees, follow
  `ai/skills/web-manual-test/SKILL.md` (Playwright MCP from `.mcp.json`, `ai/scripts/web-env-check.sh`, `ai/scripts/dev-account.sh`;
  agents must read the `SKILL.md`). The dev account credentials are `FREEM_DEV_USERNAME`/`FREEM_DEV_PASSWORD` in
  `docker/develop/.env`; agents may also create extra accounts when a check needs a clean state.

## Repository Layout

Repository root: `source/` (all code), `docker/` (environments), `ai/` (everything agent-related: `plans/`, `skills/`,
`scripts/` helpers for the skills, and shared instructions in `ai/AGENTS.md`), `tasks.md`. Root `AGENTS.md`
and `CLAUDE.md` direct each agent to these shared instructions. Agent-specific configuration stays in its
respective integration files and directories.

All code lives in `source/` (solution `Freem.slnx`, ~28 projects, `net10.0`, nullable enabled). Frontend is
`source/Freem.Web` (React 19 + Vite + PrimeReact + Chart.js, react-router 8).

Layers (names follow StashIt, without the `Core` segment):

```
Freem.Api.Public                           # ASP.NET Core API: thin controllers over the API mediator (+ .Client, .IntegrationTests)
Freem.Api.Public.UseCases(.Contracts)      # API-level handlers: DTO <-> domain mapping, Error -> HTTP code + ErrorResponse
Freem.Domain                               # ids, value objects, events, statistics data, DomainErrorCatalog (StashIt `Domain`)
Freem.Domain.Entities                      # aggregates and comparers; aggregates reference each other by Id only
Freem.Domain.UseCases(.Contracts)          # use cases (Utilities.UseCases handlers, Result<…, Error>) and their DTOs
Freem.Domain.Storage                       # one repository interface per aggregate + its filters
Freem.Domain.Storage.PostgreSQL(.Migrations)  # EF Core + Npgsql implementation
Freem.Storage.Transactions(.EFCore)        # storage transactions + EF Core implementation
Freem.DataTransfer.*                       # import/export file formats without domain (NativeJson, <Author>.<App>, StashIt `Banks.*`); adapters in Domain.UseCases
Freem.*                                    # small shared libraries (Time, Sorting, ...)
```

Shared `Utilities.*` packages (`App`, `Collections`, `Clock`, `Http`, `Http.SystemNet`, `Serialization.Json`, `Primitives`, `UseCases`, `Errors`, `Errors.Serialization.Json`, `Http.Errors`, `Locking`, `Locking.Redis`, `Locking.InMemory`) come from a local NuGet feed:
`source/Directory.Build.props` registers `source/packages/`, whose `.nupkg` files are not in git (only
`.gitkeep`). Build them in `~/Sources/CSharpUtilities` (`docker/out/packages/`) and copy the needed versions into
`packages/` before `dotnet restore`; a clean clone does not restore without them. Versions are pinned in the `.csproj`.

There is no own authentication: the API validates EgoHub access tokens (`EgoHub.Authentication.AspNetCore`, bearer) and
takes the Freem user id from the token `sub` claim (`UseCaseContextProvider`). EgoHub is an external service configured
by `Integrations:EgoHubClient:Address` and `Integrations:EgoHubApiKey` (service token `<KeyId>:<Value>`); the web client signs in through the Freem API
(`api/v1/auth/{register,login,refresh,logout}`, refresh token in an HttpOnly cookie), which proxies to EgoHub. A Freem
user is created explicitly by `POST api/v1/user/register` after the EgoHub identity exists. `Freem.Api.Public.IntegrationTests` need a
running EgoHub (`docker/test` reaches it via `host.docker.internal`) and a valid token in the local test config.

## Commands

```bash
cd source
dotnet build Freem.slnx
dotnet test Freem.slnx          # unit tests only are self-contained
dotnet run --project Freem.Api.Public/
cd Freem.Web && npm install && npm run dev   # also: npm run build, npm run lint
docker build -f Freem.Api.Public/Dockerfile .   # run from source
```

`.IntegrationTests` projects (PostgreSQL, Redis, UseCases, API) keep a committed
`configuration.json` with empty placeholders; hide local values with `git update-index --skip-worktree` (see
"Local Environment"). Frontend API address: `VITE_API_URL` (`Freem.Web/.env.example`).
