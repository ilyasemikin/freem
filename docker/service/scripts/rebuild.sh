#!/bin/bash
set -e

cd "$(dirname "$0")/.."

GIT_COMMIT=$(git rev-parse HEAD)
export GIT_COMMIT

docker compose build "$@"
