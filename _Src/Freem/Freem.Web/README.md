# Freem.Web

Web client of Freem: React 19 + Vite + PrimeReact + Chart.js, routing by react-router.

## Commands

```bash
npm install
npm run dev      # dev server on http://localhost:10191 (strict port)
npm run build    # tsc -b && vite build, output in dist/
npm run lint     # eslint
```

## Configuration

`VITE_API_URL` is the address of the Freem API (see `.env.example`; the local `dotnet run` API listens on 10190).
It is baked into the bundle at build time, so the Docker image takes it as the build argument `VITE_API_URL`.

## Layout

```
src/
  app/            router
  pages/          screens by area: home, login, activities, records, statistics, tags
  authorization/  sign-in state (context, AuthGate)
  cache/          entities cache (context + per-entity caches)
  clients/        HTTP clients, request/response models, context with the clients
  dialog/         dialog host and window components
  components/     shared UI components and inputs
  data/ entities/ helpers/
```

Authentication goes through the Freem API (`api/v1/auth/*`): the access token is kept in memory, the refresh token
lives in an HttpOnly cookie.
