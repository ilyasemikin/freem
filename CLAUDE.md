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
`Freem.Entities`, `Freem.Web`, ...), or `Freem` for cross-cutting changes. Description: lowercase, imperative,
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
- `.IntegrationTests` run via local `dotnet test` against locally running services, not via a Docker test pipeline.
- Do not spend time on manual browser passes once build and tests pass; ask first.

## Repository Layout

All code lives in `_Src/Freem/` (solution `Freem.sln`, ~65 projects, `net9.0`, nullable enabled). Frontend is
`_Src/Freem/Freem.Web` (React 19 + Vite + PrimeReact + Chart.js, react-router 7).

Layers (names differ from StashIt):

```
Freem.Web.Api.Public                       # ASP.NET Core API (+ .Client, .SyncClient, .FunctionalTests)
Freem.Entities                             # domain entities per area: Activities, Records, RunningRecords, Tags, Users, Statistics, Events
Freem.Entities.UseCases(.Contracts/.Plain) # use cases and their DTOs
Freem.Entities.Storage.Abstractions        # repository interfaces and filters
Freem.Entities.Storage.PostgreSQL(.Migrations)  # EF Core + Npgsql implementation
Freem.Entities.Events.*                    # event producers/consumers (InPlace, Local, Kafka)
Freem.Entities.Tokens.JWT, Freem.Tokens.*  # JWT auth, token blacklist in Redis
Freem.Locking.*                            # locking (Local, Redis)
Freem.*                                    # small shared libraries (Results, Collections, Linq, Time, ...)
```

## Commands

```bash
cd _Src/Freem
dotnet build Freem.sln
dotnet test Freem.sln          # unit tests only are self-contained
dotnet run --project Freem.Web.Api.Public/
cd Freem.Web && npm install && npm run dev   # also: npm run build, npm run lint
docker build -f Freem.Web.Api.Public/Dockerfile .   # run from _Src/Freem
```

`.IntegrationTests` projects (PostgreSQL, Redis, UseCases) copy a `configuration.json` that is not committed
(gitignored), so a fresh `dotnet build Freem.sln` fails with MSB3030 on those three projects until the file exists.
