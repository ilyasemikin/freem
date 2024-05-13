using Freem.Storage.Abstractions;

namespace Freem.Entities.Storage.Abstractions.Base;

public interface IBaseWriteRepository<TEntity>
    where TEntity : class
{
    Task<IStorageTransaction> CreateAsync(TEntity entity, IStorageTransaction? transaction = default, CancellationToken cancellationToken = default);
    Task<IStorageTransaction> UpdateAsync(TEntity entity, IStorageTransaction? transaction = default, CancellationToken cancellationToken = default);
    Task<IStorageTransaction> RemoveAsync(TEntity entity, IStorageTransaction? transaction = default, CancellationToken cancellationToken = default);
}
