const assert = require('node:assert/strict');
const fs = require('node:fs');
const os = require('node:os');
const path = require('node:path');
const { spawnSync } = require('node:child_process');
const { test } = require('node:test');

const hook = path.join(__dirname, 'block-commit-attribution.cjs');
function run(command, cwd = process.cwd(), field = 'command') {
  const result = spawnSync(process.execPath, [hook], {
    input: JSON.stringify({ cwd, tool_input: { [field]: command } }), encoding: 'utf8',
  });
  assert.equal(result.status, 0, result.stderr);
  return result.stdout ? JSON.parse(result.stdout).hookSpecificOutput : null;
}

for (const command of [
  'git commit -m "fix(Freem): correct routing"',
  'git commit -m "docs(Freem): document Co-Authored-By policy"',
  'git status',
  'printf "Co-Authored-By: someone"',
]) {
  test(`allows ${command}`, () => assert.equal(run(command), null));
}

for (const command of [
  'git commit -m "fix(Freem): routing\n\nCo-Authored-By: Agent <agent@example.com>"',
  'git -c user.name=Example commit -m "Co-Authored-By: Agent"',
  'git commit -m "fix: routing\\n\\nCo-Authored-By: Agent"',
  'git commit -m "fix: routing`n`nCo-Authored-By: Agent"',
  '& "C:/Program Files/Git/bin/git.exe" commit -m "Generated with Codex"',
  'git commit -m "fix: routing\n\nGenerated with Claude Code"',
  'git commit -m "fix: routing\n\nSigned-off-by: Agent"',
  'git commit -F -',
  'git commit -F "$MESSAGE_FILE"',
  'git commit --reuse-message=HEAD',
]) {
  test(`denies ${JSON.stringify(command)}`, () => {
    assert.equal(run(command)?.permissionDecision, 'deny');
  });
}

test('also accepts the raw exec_command cmd field', () => {
  assert.equal(run('git commit -m "Co-Authored-By: Agent"', undefined, 'cmd')?.permissionDecision, 'deny');
});

test('checks message files, paths with spaces and git -C without executing git', () => {
  const directory = fs.mkdtempSync(path.join(os.tmpdir(), 'freem-hook-'));
  try {
    fs.writeFileSync(path.join(directory, 'clean message.txt'), 'fix(Freem): routing\n');
    fs.writeFileSync(path.join(directory, 'bad message.txt'), 'fix: routing\n\nCo-Authored-By: Agent\n');
    assert.equal(run('git commit -F "clean message.txt"', directory), null);
    assert.equal(run('git commit --file="clean message.txt"', directory), null);
    assert.match(run('git commit --file="bad message.txt"', directory)?.permissionDecisionReason, /forbids commit authorship/);
    assert.equal(run(`git -C "${directory.replaceAll('\\', '/')}" commit -F "bad message.txt"`)?.permissionDecision, 'deny');
    assert.equal(run('git commit -F missing.txt', directory)?.permissionDecision, 'deny');
  } finally {
    fs.rmSync(directory, { recursive: true });
  }
});

test('configured Codex launcher works from a repository subdirectory', () => {
  const config = JSON.parse(fs.readFileSync(path.join(__dirname, '../../.codex/hooks.json'), 'utf8'));
  const handler = config.hooks.PreToolUse[0].hooks[0];
  const windows = process.platform === 'win32';
  const result = spawnSync(windows ? 'powershell.exe' : '/bin/sh',
    windows ? ['-NoProfile', '-Command', handler.commandWindows] : ['-c', handler.command], {
      cwd: __dirname,
      input: JSON.stringify({ tool_input: { command: 'git commit -m "Co-Authored-By: Agent"' } }),
      encoding: 'utf8',
    });
  assert.equal(result.status, 0, result.stderr);
  assert.equal(JSON.parse(result.stdout).hookSpecificOutput.permissionDecision, 'deny');
});

const posixShell = process.platform === 'win32'
  ? path.join(process.env.ProgramFiles ?? 'C:/Program Files', 'Git/bin/bash.exe')
  : '/bin/sh';
test('POSIX Codex and Claude launchers work from a repository subdirectory',
  { skip: !fs.existsSync(posixShell) }, () => {
    for (const configPath of ['../../.codex/hooks.json', '../../.claude/settings.json']) {
      const config = JSON.parse(fs.readFileSync(path.join(__dirname, configPath), 'utf8'));
      const command = config.hooks.PreToolUse[0].hooks[0].command;
      const result = spawnSync(posixShell, ['-c', command], {
        cwd: __dirname,
        input: JSON.stringify({ tool_input: { command: 'git commit -m "Co-Authored-By: Agent"' } }),
        encoding: 'utf8',
      });
      assert.equal(result.status, 0, result.stderr);
      assert.equal(JSON.parse(result.stdout).hookSpecificOutput.permissionDecision, 'deny');
    }
  });
