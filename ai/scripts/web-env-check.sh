#!/bin/bash
# Checks that everything needed for a manual web pass is reachable. Starts nothing.
# Usage: [API_PORT=...] [WEB_PORT=...] ai/scripts/web-env-check.sh (worktree ports: see ai/skills/worktree)
set -uo pipefail

API_PORT="${API_PORT:-10190}"
WEB_PORT="${WEB_PORT:-10191}"
POSTGRES_PORT=10150
REDIS_PORT=10151
EGOHUB_URL="${EGOHUB_URL:-http://127.0.0.1:25001}"

failed=0

check_tcp() {
  local name="$1" port="$2" hint="$3"
  if (exec 3<>"/dev/tcp/127.0.0.1/$port") 2>/dev/null; then
    echo "ok    $name (127.0.0.1:$port)"
  else
    echo "DOWN  $name (127.0.0.1:$port) - $hint"
    failed=1
  fi
}

check_http() {
  local name="$1" url="$2" hint="$3"
  if curl -s -o /dev/null --max-time 3 "$url"; then
    echo "ok    $name ($url)"
  else
    echo "DOWN  $name ($url) - $hint"
    failed=1
  fi
}

if [[ -f "$(dirname "$0")/../../source/Freem.Web/.env" ]]; then
  echo "ok    Freem.Web/.env"
else
  echo "MISSING Freem.Web/.env - cp source/Freem.Web/.env.example source/Freem.Web/.env (without it VITE_API_URL is unset and every API call hits Vite and gets 404); restart Vite afterwards"
  failed=1
fi

check_tcp "postgres" "$POSTGRES_PORT" "ask the user: docker compose up in docker/develop"
check_tcp "redis" "$REDIS_PORT" "ask the user: docker compose up in docker/develop"
check_http "egohub" "$EGOHUB_URL" "ask the user to start EgoHub"
check_http "api" "http://localhost:$API_PORT/api/v1/user/me" "dotnet run --project Freem.Api.Public/ (from source)"
check_http "web" "http://localhost:$WEB_PORT" "cd source/Freem.Web && npm run dev"

exit "$failed"
