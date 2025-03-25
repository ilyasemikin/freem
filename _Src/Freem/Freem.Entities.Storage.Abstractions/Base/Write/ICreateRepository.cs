using Freem.Entities.Abstractions;
using Freem.Entities.Abstractions.Identifiers;

namespace Freem.Entities.Storage.Abstractions.Base.Write;

public interface ICreateRepository<in TEntity, in TEntityIdentifier>
    where TEntity : IEntity<TEntityIdentifier>
    where TEntityIdentifier : IEntityIdentifier
{
    Task CreateAsync(TEntity entity, CancellationToken cancellationToken = default);
}