# Agent commit attribution hook

Both agents use `block-commit-attribution.cjs`. Install Node.js on PATH on each Windows or Linux
machine; no npm packages or jq are required. The hook reads a PreToolUse JSON event from stdin
and returns a denial for commit commands containing authorship trailers or generated-by footers.
It never executes the command being inspected.

## Connections

- Codex: `.codex/hooks.json` uses a POSIX command on Linux and `commandWindows` with Windows
  PowerShell on Windows. Both resolve the script from the Git root, including from subdirectories.
- Claude Code: `.claude/settings.json` runs the same script through its Bash command environment
  (Git Bash on Windows). The previous absolute Linux path is no longer used.
- `.claude/hooks/block-coauthored-commits.sh` remains a compatibility wrapper for older configurations.

Restart the agent after changing its hook configuration. In Codex, trust the project and review/trust
the hook through `/hooks` in the CLI. New or changed hooks are skipped until trusted. Do this on
each machine; committing the configuration does not establish local trust. See the
[official Codex hook documentation](https://learn.chatgpt.com/docs/hooks).

## Verification

```sh
node --test ai/hooks/block-commit-attribution.test.cjs
```

Tests feed synthetic events to the hook, including the configured Codex launcher. They do not
create commits. Run this command on each OS. To verify actual agent integration after trusting
the hook, inspect `/hooks` and test a harmless command such as
`git commit --dry-run -m "Co-Authored-By: Hook Test"`: the agent should report the hook's denial.
A direct terminal command does not invoke agent hooks.

## Scope

The guard checks inline command text and literal existing message/template files (`-F` / `-t`).
Prepare message files in a separate tool call. Stdin, shell-expanded paths and reused messages
(`-C` / `-c` after `commit`) are denied because this hook cannot inspect them reliably.

This is a guard against common attribution mistakes, not a full shell interpreter or a Git
`commit-msg` hook. It cannot guarantee inspection of messages assembled by arbitrary scripts,
interactive editors, Git defaults, aliases or later process input. It does not determine whether
the user authorized a commit; the shared authorization rules still apply. A missing runtime or
hook failure is not proof of a blocked commit: check that the agent actually reports a denial.
