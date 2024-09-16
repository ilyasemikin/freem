using Freem.Storage.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Freem.Storage.EFCore.Implementations;

internal sealed class ContextStorageTransaction<TDbContext> : IStorageTransaction
    where TDbContext : DbContext
{
    private bool _disposed = false;

    private readonly TDbContext _context;
    private readonly IDbContextTransaction _transaction;

    public ContextStorageTransaction(TDbContext context)
    {
        _context = context;
        _transaction = _context.Database.BeginTransaction();
    }

    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        await _transaction.CommitAsync(cancellationToken);
    }

    public async Task AbortAsync(CancellationToken cancellationToken)
    {
        await _transaction.RollbackAsync(cancellationToken);
    }

    public async ValueTask DisposeAsync()
    {
        if (_disposed)
            return;

        await _transaction.DisposeAsync();
        await _context.DisposeAsync();

        _disposed = true;

        GC.SuppressFinalize(this);
    }
}