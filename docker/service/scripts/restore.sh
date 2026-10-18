#!/bin/sh
set -e

if [ -n "$1" ]; then
  file="/backups/backup_freem_$1.dump"
else
  file=$(ls -1 /backups/backup_freem_*.dump | sort | tail -n 1)
fi

test -f "$file" || { echo "Backup file not found: $file"; exit 1; }

echo "Restoring from $file"
pg_restore -d freem --clean --if-exists --no-owner "$file"
