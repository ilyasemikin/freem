using Freem.Entities.Abstractions;
using Freem.Entities.Abstractions.Identifiers;

namespace Freem.Entities.Storage.Abstractions.Base.Write;

public interface IWriteRepository<in TEntity, in TEntityIdentifier> : ICreateRepository<TEntity, TEntityIdentifier>
    where TEntity : IEntity<TEntityIdentifier>
    where TEntityIdentifier : IEntityIdentifier
{
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(TEntityIdentifier id, CancellationToken cancellationToken = default);
}
