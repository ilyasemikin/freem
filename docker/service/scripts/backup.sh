#!/bin/sh
set -e

timestamp=$(date +%Y-%m-%d_%H-%M)

pg_dump -d freem -Fc -f "/backups/backup_freem_${timestamp}.dump"
pg_dump -d freem -f "/backups/backup_freem_${timestamp}.sql"

echo "Backup saved: backup_freem_${timestamp}"
