---
name: web-manual-test
description: Manual browser pass over Freem.Web with Playwright MCP against the real API and Vite dev server. Use only when the user asked for a manual web check (CLAUDE.md: ask first once build and tests pass).
---

# Manual web test (Freem.Web)

Drive the real stack in a headless browser through the Playwright MCP server (`.mcp.json`, tools `mcp__playwright__*`).
Subagents cannot use the Skill tool for project skills: `Read` this file instead. If the `mcp__playwright__*` tools are
not loaded, load them with ToolSearch (`select:mcp__playwright__browser_navigate,...` or keyword `playwright`).

## 1. Preconditions

```bash
scripts/web-env-check.sh
```

Starts nothing, prints `ok`/`DOWN` per component. What to do about `DOWN`:

| Component | Action |
|---|---|
| postgres, redis | ask the user to run `docker compose up` in `docker/develop` (the socket may be unavailable to the agent) |
| egohub | ask the user to start it (127.0.0.1:25001) |
| api (10190) | start it yourself: `cd _Src/Freem && dotnet run --project Freem.Api.Public/` in the background, remember the PID |
| Freem.Web/.env | `cp _Src/Freem/Freem.Web/.env.example _Src/Freem/Freem.Web/.env` (gitignored), then restart Vite: it reads env only at startup. Symptom of a missing file: every login says "Login failed" and API calls 404 on the Vite port |
| web (10191) | start it yourself: `cd _Src/Freem/Freem.Web && npm run dev` in the background, remember the PID (port is strict) |

Starting the API and web is allowed without asking; stop what you started when the pass is over. Kill only your own
processes, by PID (`dotnet run` spawns a child: kill the whole process group or the actual server PID found by
port, e.g. `ss -ltnp | grep :10190`). If the API or web was already running, leave it running.

## 2. Account

```bash
scripts/dev-account.sh        # prints "<username> <password>"
```

Idempotent: logs in as the dev account, creating the EgoHub identity and the Freem user if missing. Credentials are
`FREEM_DEV_USERNAME` / `FREEM_DEV_PASSWORD` in `docker/develop/.env`; if empty, ask the user to fill them.

Use the dev account by default. Create a fresh account (`POST api/v1/auth/register` + `POST api/v1/user/register`, or
the web register form) when the check needs a clean state or an empty-data case; name it `pw-<yyyymmdd>-<n>`.

## 3. Driving the browser

- Open `http://localhost:10191`, sign in through the web login form.
- Prefer `browser_snapshot` (accessibility tree) over screenshots for finding and asserting elements; take screenshots
  only to check layout. The MCP server writes files only inside the repo: pass `filename` under
  `/home/ilyasemikin/Sources/Freem/.playwright-mcp/` (gitignored), not the scratchpad.
- Icon-only buttons (`+`, play, stop, `⋮`) have empty accessible names in the snapshot. Target them by CSS instead of
  `ref`: `.panel-header:has(h2:text("Activities")) .pi-plus`, `.pi-play-circle`, `.pi-stop-circle`, `.pi-ellipsis-v`.
- After each scenario check `browser_console_messages` (errors) and `browser_network_requests` (4xx/5xx).
- Responsive checks: `browser_resize` (1280x800, 375x812).
- The refresh token is an HttpOnly cookie on the API origin; Playwright runs `--isolated`, so every session starts
  signed out.

Known noise, not bugs: `401 POST auth/refresh` on first load (no cookie yet) and `404 GET records/running` when nothing is running.

`.mcp.json` pins the cached Chromium via `--executable-path` (the MCP default is the system Google Chrome, which is not installed). If `mcp__playwright__*` tools are unavailable (`.mcp.json` not approved, browser revision mismatch), fall back to a Node script
with `playwright-core` (e.g. from `~/Sources/Trandoma/Trandoma.Web/node_modules`) and `executablePath` pointing at the
cached `~/.cache/ms-playwright/chromium_headless_shell-*/…/chrome-headless-shell`. Icon-only buttons are found by their
PrimeIcons class: `.pi-plus` (inside `.panel-header`), `.pi-play-circle`, `.pi-stop-circle`, `.pi-ellipsis-v`.

## 4. Report

Per scenario: what was done, what was observed, console/network errors, viewport. State plainly what was not
checked. Close the browser (`browser_close`) and stop the API and web you started.
