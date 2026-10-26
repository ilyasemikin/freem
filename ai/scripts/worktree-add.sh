#!/bin/bash
# Creates (or only sets up an existing) worktree in .claude/worktrees/<name>: copies untracked local files
# (.nupkg, .env, skip-worktree configs), re-sets the skip-worktree bit, assigns an API/web port pair from
# 40000-40100 and points the copied configs at it. With --db the worktree API uses its own database
# freem_<name> (created by applying migrations). Ports and database are saved to <worktree git dir>/freem-ports.
# Usage: ai/scripts/worktree-add.sh <name> [branch] [--db]
set -euo pipefail

PORT_POOL_START=40000
PORT_POOL_END=40100

name=""
branch=""
own_db=0
for arg in "$@"; do
  case "$arg" in
    --db) own_db=1 ;;
    *) if [[ -z "$name" ]]; then name="$arg"; elif [[ -z "$branch" ]]; then branch="$arg"; else echo "unexpected argument: $arg" >&2; exit 1; fi ;;
  esac
done
[[ -n "$name" ]] || { echo "usage: $0 <name> [branch] [--db]" >&2; exit 1; }

COMMON_DIR="$(git -C "$(dirname "${BASH_SOURCE[0]}")" rev-parse --path-format=absolute --git-common-dir)"
MAIN="$(dirname "$COMMON_DIR")"
WT="$MAIN/.claude/worktrees/$name"

if [[ -e "$WT" ]]; then
  git -C "$WT" rev-parse --is-inside-work-tree > /dev/null 2>&1 || { echo "$WT exists and is not a worktree" >&2; exit 1; }
  echo "worktree exists, setting it up: $WT"
else
  [[ -n "$branch" ]] || { echo "branch is required to create a new worktree" >&2; exit 1; }
  if git -C "$MAIN" show-ref --verify --quiet "refs/heads/$branch"; then
    git -C "$MAIN" worktree add "$WT" "$branch"
  else
    git -C "$MAIN" worktree add -b "$branch" "$WT"
  fi
fi

# Untracked local files: copied only when missing, so a re-run keeps changes made in the worktree
copy_missing() {
  local path="$1"
  if [[ -f "$MAIN/$path" && ! -f "$WT/$path" ]]; then
    mkdir -p "$(dirname "$WT/$path")"
    cp "$MAIN/$path" "$WT/$path"
    echo "copied $path"
  fi
}

for package in "$MAIN"/source/packages/*.nupkg; do
  copy_missing "${package#"$MAIN/"}"
done
for env in docker/develop/.env docker/service/.env docker/test/.env source/Freem.Web/.env; do
  copy_missing "$env"
done

mapfile -t skipped < <(git -C "$MAIN" ls-files -v | sed -n 's/^S //p')
for path in "${skipped[@]}"; do
  if ! git -C "$WT" ls-files -v -- "$path" | grep -q '^S '; then
    cp "$MAIN/$path" "$WT/$path"
    git -C "$WT" update-index --skip-worktree -- "$path"
    echo "copied $path (skip-worktree)"
  fi
done

PORTS_FILE="$(git -C "$WT" rev-parse --path-format=absolute --git-dir)/freem-ports"
if [[ -f "$PORTS_FILE" ]]; then
  # shellcheck disable=SC1090
  source "$PORTS_FILE"
else
  # Taken: ports assigned to other worktrees (running or not) and ports listening right now
  taken=" "
  for assigned in "$COMMON_DIR"/worktrees/*/freem-ports; do
    [[ -f "$assigned" ]] && taken+="$(sed -n 's/^[A-Z_]*_PORT=//p' "$assigned" | tr '\n' ' ')"
  done
  taken+="$(ss -Htln | awk '{n = split($4, a, ":"); print a[n]}' | tr '\n' ' ')"
  free=()
  for ((port = PORT_POOL_START; port <= PORT_POOL_END && ${#free[@]} < 2; port++)); do
    [[ "$taken" == *" $port "* ]] || free+=("$port")
  done
  [[ ${#free[@]} -eq 2 ]] || { echo "no free port pair in $PORT_POOL_START-$PORT_POOL_END" >&2; exit 1; }
  API_PORT="${free[0]}"
  WEB_PORT="${free[1]}"
  DATABASE="freem"
  [[ "$own_db" -eq 1 ]] && DATABASE="freem_${name//[^a-zA-Z0-9]/_}"
  printf 'API_PORT=%s\nWEB_PORT=%s\nDATABASE=%s\n' "$API_PORT" "$WEB_PORT" "$DATABASE" > "$PORTS_FILE"
fi

APPSETTINGS="$WT/source/Freem.Api.Public/appsettings.Development.json"
if [[ -f "$APPSETTINGS" ]]; then
  patched=$(jq --arg origin "http://localhost:$WEB_PORT" --arg database "$DATABASE" \
    '.Cors.Origins = [$origin] | .Postgres.ConnectionString |= gsub("Database=[^;]*"; "Database=" + $database)' \
    "$APPSETTINGS")
  echo "$patched" > "$APPSETTINGS"
fi

WEB_ENV="$WT/source/Freem.Web/.env"
if [[ -f "$WEB_ENV" ]] && grep -q '^VITE_API_URL=' "$WEB_ENV"; then
  sed -i "s|^VITE_API_URL=.*|VITE_API_URL=http://localhost:$API_PORT|" "$WEB_ENV"
else
  echo "VITE_API_URL=http://localhost:$API_PORT" >> "$WEB_ENV"
fi

if [[ "$DATABASE" != "freem" ]]; then
  connection=$(jq -r '.Postgres.ConnectionString' "$APPSETTINGS")
  echo "applying migrations to $DATABASE"
  # dotnet ef does not restore: a fresh worktree has no obj/
  dotnet restore "$WT/source/Freem.Domain.Storage.PostgreSQL.Migrations"
  dotnet ef database update --project "$WT/source/Freem.Domain.Storage.PostgreSQL.Migrations" -- "$connection"
fi

echo
echo "worktree: $WT"
echo "database: $DATABASE"
echo "api:      cd $WT/source && dotnet run --project Freem.Api.Public/ --urls http://localhost:$API_PORT"
echo "web:      cd $WT/source/Freem.Web && npm install && npm run dev -- --port $WEB_PORT"
