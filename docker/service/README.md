# docker/service

Environment for running Freem: PostgreSQL, Redis, migrations, API and web.

```bash
cp .env.example .env   # fill POSTGRES_USER / POSTGRES_PASSWORD and the EGOHUB_INTEGRATION_* values
docker compose up -d --build
```

`source/packages/` must contain the `.nupkg` files (see the root `CLAUDE.md`), otherwise the API and migrations
images are not built. The web address in the browser is `WEB_PUBLIC_URL`; the API address baked into the web build is
`API_PUBLIC_URL` (rebuild `web` after changing it).

To serve the web app under a path prefix behind a reverse proxy that strips it (e.g. Caddy `handle_path /freem/*`),
set `WEB_BASE_PATH=/freem/` and rebuild `web`: asset links and client-side routes get the prefix, while nginx inside
the container still serves from `/`.

EgoHub is an external, manually started service (not part of this compose file): `EGOHUB_INTEGRATION_URL` is its
public API address as seen from the `api` container, `EGOHUB_INTEGRATION_TOKEN` is a service token `<KeyId>:<Value>`.
Users sign in through the Freem API (`api/v1/auth/*`), which proxies to EgoHub.

The refresh cookie is `Secure`: browsers accept it on `localhost` over http (Safari does not), anywhere else the
API and web must be served over https.

## Manual operations

### PostgreSQL backup

```bash
docker compose --profile manual run --rm postgres-backup
```

Dumps `freem` into `backups/`:

- `backup_freem_YYYY-MM-DD_HH-MM.dump` — custom format, used by restore
- `backup_freem_YYYY-MM-DD_HH-MM.sql` — plain-text copy

### PostgreSQL restore

Restore from the latest backup:

```bash
docker compose --profile manual run --rm postgres-restore
```

Restore from a specific backup (the timestamp from the file name, e.g. `2026-10-18_12-00`):

```bash
docker compose --profile manual run --rm postgres-restore 2026-10-18_12-00
```
