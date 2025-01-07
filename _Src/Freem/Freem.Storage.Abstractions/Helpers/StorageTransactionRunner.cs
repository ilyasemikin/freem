﻿namespace Freem.Storage.Abstractions.Helpers;

public sealed class StorageTransactionRunner
{
    private readonly IStorageTransactionFactory _factory;
    private IStorageTransaction? _transaction;

    public StorageTransactionRunner(IStorageTransactionFactory factory)
    {
        ArgumentNullException.ThrowIfNull(factory);
        
        _factory = factory;
        _transaction = null;
    }
    
    public async Task RunAsync(Func<IStorageTransaction, Task> function, CancellationToken cancellationToken = default)
    {
        if (_transaction is not null)
        {
            await function(_transaction);
            return;
        }

        await using var transaction = await _factory.CreateAsync(cancellationToken);
        _transaction = transaction;

        try
        {
            await function(transaction);
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.TryAbortAsync(cancellationToken);
            throw;
        }
    }

    public async Task<T> RunAsync<T>(Func<IStorageTransaction, Task<T>> function, CancellationToken cancellationToken = default)
    {
        if (_transaction is not null)
            return await function(_transaction);
        
        await using var transaction = await _factory.CreateAsync(cancellationToken);
        _transaction = transaction;

        try
        {
            var result = await function(transaction);
            await transaction.CommitAsync(cancellationToken);
            return result;
        }
        catch
        {
            await transaction.AbortAsync(cancellationToken);
            throw;
        }
    }
}
