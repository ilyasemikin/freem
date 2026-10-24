#!/bin/bash
# Ensures the dev account exists (EgoHub identity + Freem user) and prints "<username> <password>".
# Credentials come from FREEM_DEV_USERNAME / FREEM_DEV_PASSWORD in docker/develop/.env (not tracked).
# Usage: ai/scripts/dev-account.sh [api_url]
set -euo pipefail

ROOT="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
API_URL="${1:-http://localhost:10190}/api/v1"

set -a
# shellcheck disable=SC1091
source "$ROOT/docker/develop/.env"
set +a

: "${FREEM_DEV_USERNAME:?set FREEM_DEV_USERNAME in docker/develop/.env}"
: "${FREEM_DEV_PASSWORD:?set FREEM_DEV_PASSWORD in docker/develop/.env}"

credentials=$(jq -n --arg u "$FREEM_DEV_USERNAME" --arg p "$FREEM_DEV_PASSWORD" '{username: $u, password: $p}')

login() {
  curl -s -o /tmp/freem-dev-login.json -w "%{http_code}" -X POST \
    -H "Content-Type: application/json" -d "$credentials" "$API_URL/auth/login"
}

code=$(login)
if [[ "$code" != "200" ]]; then
  curl -sf -X POST -H "Content-Type: application/json" -d "$credentials" "$API_URL/auth/register" > /dev/null
  code=$(login)
  [[ "$code" == "200" ]] || { echo "login failed (HTTP $code)" >&2; exit 1; }
fi

token=$(jq -r '.accessToken' /tmp/freem-dev-login.json)
rm -f /tmp/freem-dev-login.json

nickname=$(jq -n --arg n "$FREEM_DEV_USERNAME" '{nickname: $n}')
# 200 = created, 409 = Freem user already exists
curl -s -o /dev/null -X POST -H "Content-Type: application/json" \
  -H "Authorization: Bearer $token" -d "$nickname" "$API_URL/user/register"

echo "$FREEM_DEV_USERNAME $FREEM_DEV_PASSWORD"
