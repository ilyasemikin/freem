using Freem.Storage.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Freem.Storage.EFCore.Implementations;

internal class StorageTransactionFactory<TDbContext> : IStorageTransactionFactory
    where TDbContext : DbContext
{
    private readonly IDbContextFactory<TDbContext> _factory;

    public StorageTransactionFactory(IDbContextFactory<TDbContext> factory)
    {
        _factory = factory;
    }

    public async Task<IStorageTransaction> CreateAsync(CancellationToken cancellationToken = default)
    {
        var context = await _factory.CreateDbContextAsync(cancellationToken);
        return new StorageTransaction<TDbContext>(context);
    }
}
