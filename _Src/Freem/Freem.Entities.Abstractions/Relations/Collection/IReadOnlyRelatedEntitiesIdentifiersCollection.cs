using Freem.Entities.Abstractions.Identifiers;

namespace Freem.Entities.Abstractions.Relations.Collection;

public interface IReadOnlyRelatedEntitiesIdentifiersCollection<TEntityIdentifier>
    where TEntityIdentifier : IEntityIdentifier
{
    int Count { get; }
    
    IEnumerable<TEntityIdentifier> Identifiers { get; }
    
    bool Contains(TEntityIdentifier identifier);
}