# Claude Code Instructions

Before working in this repository, read and follow [the shared repository instructions](ai/AGENTS.md).
Treat that file as part of these instructions. Maintain common rules there.

## Claude Code Integration

- `.claude/settings.json` invokes the shared attribution hook in `ai/hooks/` (requires Node.js on PATH).
- `.claude/settings.json` denies `git push` and requires confirmation for `git commit`.
  The shared rules about explicit user authorization still apply.
- The "Generated with Claude Code" line belongs only in pull request descriptions.
