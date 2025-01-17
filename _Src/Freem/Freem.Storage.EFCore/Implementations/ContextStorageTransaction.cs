using Freem.Storage.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Freem.Storage.EFCore.Implementations;

internal sealed class ContextStorageTransaction<TDbContext> : IStorageTransaction
    where TDbContext : DbContext
{
    private bool _disposed;
    private bool _completed;

    private readonly IDbContextTransaction _transaction;

    public ContextStorageTransaction(TDbContext context)
    {
        _transaction = context.Database.BeginTransaction();
    }

    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        _completed = true;
        await _transaction.CommitAsync(cancellationToken);
    }

    public async Task AbortAsync(CancellationToken cancellationToken)
    {
        _completed = true;
        await _transaction.RollbackAsync(cancellationToken);
    }

    public async Task<bool> TryCommitAsync(CancellationToken cancellationToken = default)
    {
        if (_completed)
            return false;

        _completed = true;
        await _transaction.CommitAsync(cancellationToken);
        return true;
    }

    public async Task<bool> TryAbortAsync(CancellationToken cancellationToken = default)
    {
        if (_completed)
            return false;

        _completed = true;
        await _transaction.RollbackAsync(cancellationToken);
        return true;
    }

    public async ValueTask DisposeAsync()
    {
        if (_disposed)
            return;

        await _transaction.DisposeAsync();

        _disposed = true;

        GC.SuppressFinalize(this);
    }
}