using Freem.Storage.Abstractions;

namespace Freem.Entities.Storage.Abstractions.Base;

public interface IBaseWriteRepository<TEntity>
    where TEntity : class
{
    Task CreateAsync(TEntity entity, IStorageTransaction? transaction = default, CancellationToken cancellationToken = default);
    Task UpdateAsync(TEntity entity, IStorageTransaction? transaction = default, CancellationToken cancellationToken = default);
    Task RemoveAsync(TEntity entity, IStorageTransaction? transaction = default, CancellationToken cancellationToken = default);
}
