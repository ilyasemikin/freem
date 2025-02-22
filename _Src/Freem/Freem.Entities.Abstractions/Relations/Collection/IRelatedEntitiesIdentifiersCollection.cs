using Freem.Entities.Abstractions.Identifiers;

namespace Freem.Entities.Abstractions.Relations.Collection;

public interface IRelatedEntitiesIdentifiersCollection<TEntityIdentifier> 
    : IReadOnlyRelatedEntitiesIdentifiersCollection<TEntityIdentifier>
    where TEntityIdentifier : IEntityIdentifier
{
    bool TryAdd(TEntityIdentifier identifier);
    bool TryRemove(TEntityIdentifier identifier);
}