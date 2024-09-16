using Freem.Storage.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Freem.Storage.EFCore.Implementations;

internal sealed class ContaxtStorageTransactionFactory<TDbContext> : IStorageTransactionFactory
    where TDbContext : DbContext
{
    private readonly TDbContext _context;

    public ContaxtStorageTransactionFactory(TDbContext context)
    {
        _context = context;
    }

    public async Task<IStorageTransaction> CreateAsync(CancellationToken cancellationToken = default)
    {
        var transaction = new ContextStorageTransaction<TDbContext>(_context);
        return await Task.FromResult(transaction);
    }
}
