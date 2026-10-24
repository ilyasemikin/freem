using Freem.Storage.Transactions.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Freem.Storage.Transactions.EFCore.Implementations;

internal sealed class ContextStorageTransactionFactory<TDbContext> : IStorageTransactionFactory
    where TDbContext : DbContext
{
    private readonly TDbContext _context;

    public ContextStorageTransactionFactory(TDbContext context)
    {
        _context = context;
    }

    public async Task<IStorageTransaction> CreateAsync(CancellationToken cancellationToken = default)
    {
        var transaction = new ContextStorageTransaction<TDbContext>(_context);
        return await Task.FromResult(transaction);
    }
}
