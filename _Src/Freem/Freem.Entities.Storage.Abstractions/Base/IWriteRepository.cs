using Freem.Entities.Abstractions;
using Freem.Entities.Abstractions.Identifiers;

namespace Freem.Entities.Storage.Abstractions.Base;

public interface IWriteRepository<TEntity, TEntityIdentifier>
    where TEntity : notnull, IEntity<TEntityIdentifier>
    where TEntityIdentifier : notnull, IEntityIdentifier
{
    Task CreateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task RemoveAsync(TEntityIdentifier id, CancellationToken cancellationToken = default);
}
