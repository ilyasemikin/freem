# docker/service

Environment for running Freem: PostgreSQL, Redis, migrations, API and web.
API and web are reached through Caddy, managed in a separate Compose project on the same Docker host.
No services in this project publish host ports.

```bash
cp .env.example .env   # fill credentials, PROXY_NETWORK_NAME, integration settings and public URLs
docker network create <network-name>   # use PROXY_NETWORK_NAME from .env; skip if the network exists
docker compose up -d --build
```

`source/packages/` must contain the `.nupkg` files (see [the shared repository instructions](../../ai/AGENTS.md)), otherwise the API and migrations
images are not built. The web address in the browser is `WEB_PUBLIC_URL`; the API address baked into the web build is
`API_PUBLIC_URL` (rebuild `web` after changing it).

EgoHub is an external, manually started service (not part of this compose file): `EGOHUB_INTEGRATION_URL` is its
public API address as seen from the `api` container, `EGOHUB_INTEGRATION_TOKEN` is a service token `<KeyId>:<Value>`.
Users sign in through the Freem API (`api/v1/auth/*`), which proxies to EgoHub.

The refresh cookie is `Secure`: browsers accept it on `localhost` over http (Safari does not), anywhere else the
API and web must be served over https.

## Caddy network and routing

Set `PROXY_NETWORK_NAME` in `.env` to the shared network name. It is required and has no default;
Compose fails with an error if the variable is unset or empty.
Create that network before starting either project. Both Compose projects declare it as external,
so neither project creates or removes it. API and web join it with the aliases `freem-api` and
`freem-web`; PostgreSQL, Redis and maintenance containers stay only on `freem_network`.
Use these application-specific aliases in Caddy, since generic names such as `api` and `web`
can also belong to other projects on the shared network. Containers on the shared network can
reach each other's listening ports; only attach services that need proxy access.

Add the network to the existing Caddy Compose configuration, preserving its other settings and networks:

```yaml
services:
  caddy:
    networks:
      - proxy_network

networks:
  proxy_network:
    external: true
    name: ${PROXY_NETWORK_NAME:?Set PROXY_NETWORK_NAME to the existing reverse proxy network name}
```

Set `PROXY_NETWORK_NAME` in the Caddy project's environment to the same value as in Freem's `.env`.

For separate web and API domains, the corresponding Caddyfile routes are:

```caddyfile
freem.example.com {
    reverse_proxy freem-web:80
}

api.freem.example.com {
    reverse_proxy freem-api:8080
}
```

Replace these example domains with your domains and set `WEB_PUBLIC_URL` and `API_PUBLIC_URL`
accordingly. The browser still needs public HTTPS URLs; Docker aliases are only for traffic between
containers. With these domain-based routes, keep `WEB_BASE_PATH=/`. If your existing Caddy routes
serve web under a path prefix, preserve the prefix stripping and matching `WEB_BASE_PATH`.
Only Caddy needs to publish the public HTTP/HTTPS ports.

### Migrating an existing deployment

1. Create the shared network if needed, and attach the existing Caddy service to it by updating
   its Compose configuration and recreating that service.
2. Add `PROXY_NETWORK_NAME` to Freem's `.env`. Remove the unused `API_PORT` and `WEB_PORT`
   settings. Keep the existing public URLs if the browser-facing addresses are unchanged.
3. From `docker/service`, run `docker compose config --quiet`, then `docker compose up -d --build`.
   This recreates API and web with the shared network and removes their host port mappings.
4. Change the existing Caddy upstreams to `freem-api:8080` and `freem-web:80`, validate the
   Caddyfile and reload Caddy. Keep the existing domain names and path routing.
5. Check the public web URL and API health endpoint (`/_health/liveness`), and confirm that
   `docker compose ps` shows no published host ports for API or web.

Allow for a brief interruption while the Freem containers and Caddy upstreams are updated.
No `docker compose down` or volume removal is needed.

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
