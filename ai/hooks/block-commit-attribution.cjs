// Shared Claude Code / Codex PreToolUse hook. Never executes the inspected command.
const fs = require('node:fs');
const path = require('node:path');

function deny(reason) {
  process.stdout.write(JSON.stringify({
    hookSpecificOutput: {
      hookEventName: 'PreToolUse',
      permissionDecision: 'deny',
      permissionDecisionReason: reason,
    },
  }));
}

function hasAttribution(text) {
  // Include shell-escaped newline forms used by Bash and PowerShell messages.
  const normalized = text.replace(/\\r\\n|\\n|`r`n|`n/g, '\n');
  return /(?:^|\n|["'])[ \t]*(?:co-authored-by|signed-off-by|authored-by|generated-by)[ \t]*:/im.test(normalized)
    || /(?:^|\n|["'])[ \t]*(?:[\p{Symbol}\p{Extended_Pictographic}]\s*)?generated\s+(?:with|by)\b/imu.test(normalized);
}

function check(event) {
  const command = event.tool_input?.command ?? event.tool_input?.cmd;
  if (typeof command !== 'string') {
    return;
  }

  // Recognize git global options, quoted executables, PowerShell's call operator,
  // shell separators and wrappers. This is a command guard, not a shell parser.
  const tokens = command.match(/(?:"(?:[^"\\]|\\.)*"|'[^']*'|[^\s"';|&])+/g) ?? [];
  const unquote = (token) => token.replace(/^(["'])([\s\S]*)\1$/, '$2');
  let commitIndex = -1;
  let directory = event.cwd ?? process.cwd();
  for (let index = 0; index < tokens.length; index++) {
    const executable = unquote(tokens[index]).replaceAll('\\', '/').split('/').pop();
    if (!/^git(?:\.exe)?$/i.test(executable)) {
      continue;
    }
    let candidateDirectory = event.cwd ?? process.cwd();
    for (let next = index + 1; next < tokens.length; next++) {
      const token = unquote(tokens[next]);
      if (token === '-C') {
        candidateDirectory = path.resolve(candidateDirectory, unquote(tokens[++next] ?? ''));
      } else if (['-c', '--git-dir', '--work-tree', '--namespace', '--config-env'].includes(token)) {
        next++;
      } else if (token.startsWith('-')) {
        continue;
      } else {
        if (token === 'commit') {
          commitIndex = next;
          directory = candidateDirectory;
        }
        break;
      }
    }
    if (commitIndex >= 0) {
      break;
    }
  }
  if (commitIndex < 0) {
    return;
  }

  const policy = 'ai/AGENTS.md forbids commit authorship trailers and generated-by footers. Remove the attribution and retry.';
  if (hasAttribution(command)) {
    deny(policy);
    return;
  }

  for (let index = commitIndex + 1; index < tokens.length; index++) {
    const token = unquote(tokens[index]);
    let messageFile;
    if (['-F', '--file', '-t', '--template'].includes(token)) {
      messageFile = unquote(tokens[++index] ?? '');
    } else if (/^--(?:file|template)=/.test(token)) {
      messageFile = unquote(token.slice(token.indexOf('=') + 1));
    } else if (/^-[Ft].+/.test(token)) {
      messageFile = unquote(token.slice(2));
    } else if (['-C', '-c', '--reuse-message', '--reedit-message'].includes(token)
      || /^--(?:reuse-message|reedit-message)=/.test(token)) {
      deny('Cannot inspect reused commit messages in this hook. Supply the reviewed message explicitly with -m or -F.');
      return;
    } else {
      continue;
    }

    // Do not evaluate shell variables, substitutions, stdin or process substitutions.
    if (!messageFile || messageFile === '-' || /[$`<>%~]/.test(messageFile)) {
      deny('Cannot inspect the commit message source. Use -m or -F with a literal existing file path.');
      return;
    }
    try {
      if (hasAttribution(fs.readFileSync(path.resolve(directory, messageFile), 'utf8'))) {
        deny(policy);
        return;
      }
    } catch {
      deny('Cannot read the commit message file before execution. Prepare the file in a separate command, then retry.');
      return;
    }
  }
}

try {
  check(JSON.parse(fs.readFileSync(0, 'utf8')));
} catch {
  deny('Cannot validate the hook input. Check the commit attribution hook configuration.');
}
