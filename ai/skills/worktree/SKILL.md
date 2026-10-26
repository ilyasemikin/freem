---
name: worktree
description: Work on several features in parallel through git worktrees in .claude/worktrees/<name>: create and set one up (local configs, skip-worktree, ports from 40000-40100, optional own database), run the API/web in it, remove it. Use when starting a parallel feature, after Claude Code created an isolated worktree, or when cleaning one up.
---

# Parallel features in worktrees

Each feature gets its own worktree in `.claude/worktrees/<name>` (gitignored) on its own branch; the main checkout
stays as it is. A branch can be checked out in only one worktree. Subagents cannot use the Skill tool for project
skills: `Read` this file instead.

A fresh worktree has only what is committed. Missing until set up: `source/packages/*.nupkg`, `.env` files, local
values of skip-worktree configs (`appsettings.Development.json`, `configuration.json` of the integration tests; the
bit itself is per worktree and starts cleared, so without setup local secrets show up in `git status`).

## 1. Create or set up

```bash
ai/scripts/worktree-add.sh <name> <branch> [--db]   # new worktree
ai/scripts/worktree-add.sh <name> [--db]            # already exists (e.g. created by Claude Code isolation): setup only
```

The script:
- copies missing `.nupkg`, `.env` (`docker/*`, `Freem.Web`) and skip-worktree configs from the main checkout and sets the bit;
- assigns the first free API/web port pair from **40000-40100** (skipping ports assigned to other worktrees and ports
  listening now), points `Cors:Origins` of `appsettings.Development.json` and `VITE_API_URL` of `Freem.Web/.env` at it;
- saves `API_PORT`, `WEB_PORT`, `DATABASE` to `<worktree git dir>/freem-ports` (outside the working tree, removed with
  the worktree); a re-run reuses them and does not overwrite files already present;
- prints the start commands.

Read the assignment later: `cat "$(git -C .claude/worktrees/<name> rev-parse --git-dir)/freem-ports"`.

Afterwards verify `git -C .claude/worktrees/<name> status` is clean and `git ls-files -v | grep '^S'` lists the same
files as in the main checkout.

## 2. Database

Default: the worktree uses the shared `freem` database of `docker/develop` (Postgres and Redis are shared, no second
compose stack). Pass `--db` when the feature changes the schema (adds migrations): the worktree then gets its own
database `freem_<name>` in the same Postgres, created by `dotnet ef database update` from the worktree's migrations
project. Otherwise its migrations would land in the shared database and break the other checkouts. Decide before the
first run: `--db` is honoured only when ports are assigned for the first time. If unsure whether the feature touches
the schema, ask the user.

Redis keys are shared between all checkouts (locks); this is fine for manual runs.

## 3. Run

```bash
cd .claude/worktrees/<name>/source && dotnet run --project Freem.Api.Public/ --urls http://localhost:<API_PORT>
cd .claude/worktrees/<name>/source/Freem.Web && npm install && npm run dev -- --port <WEB_PORT>
```

`--urls` / `--port` are required: `launchSettings.json` and `vite.config.ts` hold the main checkout ports
10190/10191. Same rules as in the main checkout: start in the background, remember the PID, kill only your own
processes. For a manual web pass (`ai/skills/web-manual-test/SKILL.md`) run its scripts from the worktree with the
ports: `API_PORT=<API_PORT> WEB_PORT=<WEB_PORT> ai/scripts/web-env-check.sh`, `ai/scripts/dev-account.sh
http://localhost:<API_PORT>`.

Integration tests (`docker/test`) use fixed `freem_*_test` databases and truncate tables: never run them from two
checkouts at the same time.

## 4. Remove

After the branch is merged or abandoned (ask the user before removing a worktree you did not create):

```bash
ai/scripts/worktree-remove.sh <name> [--drop-db]
```

Stop the API/web of the worktree first (the script refuses while its ports are listening): `dotnet run` and
`npm run dev` spawn children, so kill the process group of the actual server found by port
(`ss -ltnp | grep :<port>`, then `ps -o pgid= -p <pid>`). git refuses to remove a worktree with uncommitted or untracked changes; do
not force it, report to the user. The branch is kept; delete it only when the user asks. If a worktree directory was
deleted by hand, run `git worktree prune`.
