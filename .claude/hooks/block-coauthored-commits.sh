#!/usr/bin/env bash
# PreToolUse hook (Bash matcher): blocks `git commit` invocations whose message
# contains an actual Co-Authored-By trailer line (not just prose mentioning the
# term). This repo's CLAUDE.md explicitly forbids such trailers; this is a hard
# technical gate for that rule instead of relying on in-context judgment about
# which instruction wins.
set -euo pipefail

cmd=$(jq -r '.tool_input.command // empty')

# Matches "git commit" as an actual subcommand invocation (optionally after a
# shell separator or inside a command substitution, allowing git global flags
# like -C <path> before the subcommand), not just the words appearing anywhere.
git_commit_pattern='(^|[;&|]|\$\()[[:space:]]*git([[:space:]]+-[A-Za-z][A-Za-z-]*([[:space:]]+[^[:space:]]+)?)*[[:space:]]+commit([[:space:]]|$)'

# Git trailers are their own line, "Key: value" - anchor to line-start so prose
# that merely *mentions* "Co-Authored-By" (e.g. a commit describing this very
# hook) doesn't trip the check; only an actual trailer line does.
trailer_pattern='^[[:space:]]*Co-Authored-By[[:space:]]*:'

if printf '%s' "$cmd" | grep -qiE "$git_commit_pattern" \
  && printf '%s' "$cmd" | grep -qiE "$trailer_pattern"; then
  printf '{"hookSpecificOutput":{"hookEventName":"PreToolUse","permissionDecision":"deny","permissionDecisionReason":"CLAUDE.md in this repository forbids Co-Authored-By or other authorship trailers in commit messages. Remove the trailer and retry."}}'
fi
