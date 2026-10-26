#!/bin/bash
# Removes a worktree created by worktree-add.sh. Its own database (if any) is dropped only with --drop-db.
# The branch is kept. git refuses to remove a worktree with uncommitted or untracked changes: nothing is forced.
# Usage: ai/scripts/worktree-remove.sh <name> [--drop-db]
set -euo pipefail

name="${1:?usage: $0 <name> [--drop-db]}"
drop_db=0
[[ "${2:-}" == "--drop-db" ]] && drop_db=1

COMMON_DIR="$(git -C "$(dirname "${BASH_SOURCE[0]}")" rev-parse --path-format=absolute --git-common-dir)"
MAIN="$(dirname "$COMMON_DIR")"
WT="$MAIN/.claude/worktrees/$name"
[[ -d "$WT" ]] || { echo "no worktree $WT" >&2; exit 1; }

PORTS_FILE="$(git -C "$WT" rev-parse --path-format=absolute --git-dir)/freem-ports"
DATABASE="freem"
# shellcheck disable=SC1090
[[ -f "$PORTS_FILE" ]] && source "$PORTS_FILE"

# A running API/web keeps writing (bin/, .vite/) and would recreate the directory after removal
for port in ${API_PORT:-} ${WEB_PORT:-}; do
  if ss -Htln | awk '{n = split($4, a, ":"); print a[n]}' | grep -qx "$port"; then
    echo "port $port is in use: stop the API/web of this worktree first" >&2
    exit 1
  fi
done

if [[ "$DATABASE" != "freem" ]]; then
  if [[ "$drop_db" -eq 1 ]]; then
    connection=$(jq -r '.Postgres.ConnectionString' "$WT/source/Freem.Api.Public/appsettings.Development.json")
    dotnet ef database drop --force --project "$WT/source/Freem.Domain.Storage.PostgreSQL.Migrations" -- "$connection"
  else
    echo "database $DATABASE is kept (pass --drop-db to drop it)"
  fi
fi

branch=$(git -C "$WT" branch --show-current)
git -C "$MAIN" worktree remove "$WT"
echo "removed $WT; branch $branch is kept"
