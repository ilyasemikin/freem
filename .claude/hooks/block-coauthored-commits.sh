#!/usr/bin/env bash
# Compatibility entry point; both agents now use the shared Node.js hook directly.
set -euo pipefail
exec node "$(git rev-parse --show-toplevel)/ai/hooks/block-commit-attribution.cjs"
