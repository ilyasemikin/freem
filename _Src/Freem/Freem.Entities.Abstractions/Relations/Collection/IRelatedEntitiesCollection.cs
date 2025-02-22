using Freem.Entities.Abstractions.Identifiers;

namespace Freem.Entities.Abstractions.Relations.Collection;

public interface IRelatedEntitiesCollection<TEntity, TEntityIdentifier> :
    IRelatedEntitiesIdentifiersCollection<TEntityIdentifier>,
    IReadOnlyRelatedEntitiesCollection<TEntity, TEntityIdentifier> 
    where TEntity : IEntity<TEntityIdentifier>
    where TEntityIdentifier : IEntityIdentifier
{
    bool TryAdd(TEntity entity);
    bool TryUpdate(TEntity entity);
}
