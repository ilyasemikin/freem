#!/bin/sh
set -e

# POSTGRES_DB is a space-separated list of databases
for database in $POSTGRES_DB; do
  CONNECTION_STRING="Host=$POSTGRES_HOST;Database=$database;Username=$POSTGRES_USER;Password=$POSTGRES_PASSWORD"

  echo "Running migrations for $POSTGRES_HOST.$database"
  dotnet-ef database update -- "$CONNECTION_STRING"
done
