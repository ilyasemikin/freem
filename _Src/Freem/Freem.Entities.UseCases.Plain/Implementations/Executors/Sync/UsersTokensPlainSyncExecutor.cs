﻿using Freem.Entities.UseCases.Plain.Implementations.Executors.Async;
using Freem.UseCases.Abstractions;

namespace Freem.Entities.UseCases.Plain.Implementations.Executors.Sync;

public sealed class UsersTokensPlainSyncExecutor
{
    private readonly UsersTokensPlainExecutor _executor;

    public UsersTokensPlainSyncExecutor(IUseCaseExecutor<UseCaseExecutionContext> executor)
    {
        ArgumentNullException.ThrowIfNull(executor);
        
        _executor = new UsersTokensPlainExecutor(executor);
    }

    public UsersTokensPlainExecutor.Tokens Refresh(UseCaseExecutionContext context, string refreshToken)
    {
        return _executor.RefreshAsync(context, refreshToken)
            .GetAwaiter()
            .GetResult();
    }
}