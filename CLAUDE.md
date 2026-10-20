# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Freem is a personal time tracker: the user explicitly starts an activity (a running record), stops it, and the
record is stored in a database for later analysis. Entities: users, activities, tags, records, running records,
statistics, events. The project was dormant from 2025-06 and is being revived; the sibling project
`../StashIt` is the reference for conventions, structure and workflow.

## Git Commits

Commits follow [Conventional Commits v1.0.0](https://www.conventionalcommits.org/en/v1.0.0/): `type(scope): description`.
Types: `feat`, `fix`, `refactor`, `chore`, `docs`, `test`. Scope: the affected project name (`Freem.Web.Api.Public`,
`Freem.Domain`, `Freem.Web`, ...), or `Freem` for cross-cutting changes. Description: lowercase, imperative,
concise, no trailing period. History before the revival does not follow this convention.

Do not add `Co-Authored-By` or any other authorship trailers, nor a "Generated with Claude Code" footer, to
commits (trailers are enforced by `.claude/hooks/block-coauthored-commits.sh`). The "Generated with Claude Code"
line belongs only in pull request descriptions.

**When to commit:** only when the user explicitly asks. Permission covers exactly the work it was given for;
for any later change, stop, show the result and wait for a fresh go-ahead. Never commit in the same turn in which
the change was first written unless the user asked for that commit in advance. `git push` is denied;
`git commit` always prompts (`.claude/settings.json`).

## Working With the User

If the user's answer or instruction is ambiguous, ask what exactly they meant. Do not pick an interpretation
yourself. This covers code, plans, docs and git. A reply to a multi-part proposal is not consent to all parts.

## Planning Documents

Large, multi-stage initiatives get a persistent plan under `plans/` (not `docs/`). Either one flat file or
`plans/<slug>/general/roadmap.md` + `plans/<slug>/stages/NN-<stage>.md`. Conventions are formalized as project skills
in `skills/` (`roadmap`, `roadmap-stage`), copied from StashIt. Before writing or continuing a
plan, read the matching `SKILL.md` — even if it did not trigger automatically (subagents cannot see project
skills via the Skill tool and must `Read` the `SKILL.md`). Plans are written in Russian. Effort is estimated by
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
  10100-10149). Freem takes **10150-10199** (confirmed by the user): `docker/develop` 10150-10169, `docker/service` 10170-10189, local
  `dotnet run`/misc 10190-10199. Never bind default ports (5432, 6379, 9092, ...).
- Local secrets in tracked config (`appsettings.Development.json`, `configuration.json`) are hidden with
  `git update-index --skip-worktree <file>`, never `.gitignore`. `git stash` and `git checkout -- <path>` clear
  the bit: re-check with `git ls-files -v` (`S` = set, `H` = cleared).
- EgoHub is started manually by the user and may be down; ask the user to start it.
- Kill only processes you started yourself, by PID, never by name grep.
- Docker environments live in `docker/{develop,service,test}` (PostgreSQL 18, Redis, `postgres-migration` container
  built from `Freem.Entities.Storage.PostgreSQL.Migrations/Dockerfile`; `service` adds API, web and manual
  `postgres-backup`/`postgres-restore`). `.env` is created from `.env.example` by the user. Ports: `develop` Postgres
  10150, Redis 10151; `service` API 10170, web 10171; local `dotnet run` API 10190, `npm run dev` 10191 (strict).
  Images need `.nupkg` files in `_Src/Freem/packages/`.
- `.IntegrationTests` and `FunctionalTests` run in `docker/test` (`docker compose up --build --abort-on-container-exit
  test`, then `down -v`; no ports published, test configs in `docker/test/configs/`; EgoHub address and token come from `docker/test/.env` (see `.env.example`); each test assembly has its own database `freem_{storage,usecases,api}_test`, because assemblies run in parallel and truncate tables). `TruncateTables` refuses a
  database whose name does not end with `_test`. The committed `configuration.json` files of the test projects hold
  empty placeholders for local runs against `develop` (use a `*_test` database).
- Do not spend time on manual browser passes once build and tests pass; ask first.

## Repository Layout

All code lives in `_Src/Freem/` (solution `Freem.sln`, ~28 projects, `net10.0`, nullable enabled). Frontend is
`_Src/Freem/Freem.Web` (React 19 + Vite + PrimeReact + Chart.js, react-router 8).

Layers (names differ from StashIt):

```
Freem.Web.Api.Public                       # ASP.NET Core API (+ .Client, .SyncClient, .FunctionalTests)
Freem.Domain                               # ids, value objects, events, statistics data, DomainErrorCatalog (StashIt `Domain`)
Freem.Domain.Entities                      # aggregates and comparers; aggregates reference each other by Id only
Freem.Entities.UseCases(.Contracts)        # use cases (Utilities.UseCases handlers, Result<…, Error>) and their DTOs
Freem.Domain.Storage                       # one repository interface per aggregate + its filters
Freem.Entities.Storage.PostgreSQL(.Migrations)  # EF Core + Npgsql implementation
Freem.Entities.Events.Producer             # event production (NullEventPublisher)
Freem.Locking.*                            # locking (Redis)
Freem.*                                    # small shared libraries (Time, Storage, Locking, ...)
```

Shared `Utilities.*` packages (`Collections`, `Clock`, `Http`, `Http.SystemNet`, `Serialization.Json`, `Primitives`, `UseCases`, `Errors`) come from a local NuGet feed:
`_Src/Freem/Directory.Build.props` registers `_Src/Freem/packages/`, whose `.nupkg` files are not in git (only
`.gitkeep`). Build them in `~/Sources/CSharpUtilities` (`docker/out/packages/`) and copy the needed versions into
`packages/` before `dotnet restore`; a clean clone does not restore without them. Versions are pinned in the `.csproj`.

There is no own authentication: the API validates EgoHub access tokens (`EgoHub.Authentication.AspNetCore`, bearer) and
takes the Freem user id from the token `sub` claim (`UseCaseContextProvider`). EgoHub is an external service configured
by `EgoHubIntegration:Url/Token` (service token `<KeyId>:<Value>`); the web client signs in through the Freem API
(`api/v1/auth/{register,login,refresh,logout}`, refresh token in an HttpOnly cookie), which proxies to EgoHub. A Freem
user is created explicitly by `POST api/v1/user/register` after the EgoHub identity exists. `FunctionalTests` need a
running EgoHub (`docker/test` reaches it via `host.docker.internal`) and a valid token in the local test config.

## Commands

```bash
cd _Src/Freem
dotnet build Freem.sln
dotnet test Freem.sln          # unit tests only are self-contained
dotnet run --project Freem.Web.Api.Public/
cd Freem.Web && npm install && npm run dev   # also: npm run build, npm run lint
docker build -f Freem.Web.Api.Public/Dockerfile .   # run from _Src/Freem
```

`.IntegrationTests` projects (PostgreSQL, Redis, UseCases) and `FunctionalTests` keep a committed
`configuration.json` with empty placeholders; hide local values with `git update-index --skip-worktree` (see
"Local Environment"). Frontend API address: `VITE_API_URL` (`Freem.Web/.env.example`).
