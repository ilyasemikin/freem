using Freem.Entities.Storage.Abstractions.Base;
using Freem.Storage.Abstractions;

namespace Freem.Entities.Storage.Abstractions.Extensions;

public static class BaseRepositoryExtensions
{
    public static async Task RemoveAsync<TRepository, TEntity>(
        this TRepository repository, 
        string id, 
        IStorageTransaction? transaction = default, 
        CancellationToken cancellationToken = default)
        where TRepository : IBaseWriteRepository<TEntity>, IBaseSearchByIdRepository<TEntity>
        where TEntity : class
    {
        var result = await repository.FindByIdAsync(id, cancellationToken);
        if (!result.Founded)
            return;

        var entity = result.Entity;
        await repository.RemoveAsync(entity, transaction, cancellationToken);
    }
}