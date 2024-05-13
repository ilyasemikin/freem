using Freem.Storage.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Freem.Storage.EFCore.Implementations;

public class StorageTransaction<TDbContext> : IStorageTransaction
    where TDbContext : DbContext
{
    private bool _disposed = false;

    private readonly TDbContext _context;
    private readonly IDbContextTransaction _transaction;

    public StorageTransaction(TDbContext context)
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
