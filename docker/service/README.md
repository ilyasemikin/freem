# docker/service

Environment for running Freem: PostgreSQL, Redis, migrations, API and web.

```bash
cp .env.example .env   # fill POSTGRES_USER / POSTGRES_PASSWORD
docker compose up -d --build
```

`_Src/Freem/packages/` must contain the `.nupkg` files (see the root `CLAUDE.md`), otherwise the API and migrations
images are not built. The web address in the browser is `WEB_PUBLIC_URL`; the API address baked into the web build is
`API_PUBLIC_URL` (rebuild `web` after changing it).

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
