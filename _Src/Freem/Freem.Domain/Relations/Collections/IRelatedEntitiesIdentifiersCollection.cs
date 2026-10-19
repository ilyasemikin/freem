using Freem.Domain.Identifiers;

namespace Freem.Domain.Relations.Collections;

public interface IRelatedEntitiesIdentifiersCollection<TEntityIdentifier> 
    : IReadOnlyRelatedEntitiesIdentifiersCollection<TEntityIdentifier>
    where TEntityIdentifier : IEntityIdentifier
{
    bool TryAdd(TEntityIdentifier identifier);
    bool TryRemove(TEntityIdentifier identifier);
}