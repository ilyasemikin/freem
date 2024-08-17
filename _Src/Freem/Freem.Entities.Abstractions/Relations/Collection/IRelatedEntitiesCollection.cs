using Freem.Entities.Abstractions.Identifiers;

namespace Freem.Entities.Abstractions.Relations.Collection;

public interface IRelatedEntitiesCollection<TEntity, TEntityIdentifier> : IReadOnlyRelatedEntitiesCollection<TEntity, TEntityIdentifier>
    where TEntity : notnull, IEntity<TEntityIdentifier>
    where TEntityIdentifier : notnull, IEntityIdentifier
{
    bool TryAdd(TEntityIdentifier identifier);
    bool TryAdd(TEntity entity);
    bool TryUpdate(TEntity entity);
    bool TryRemove(TEntityIdentifier identifier);
}
