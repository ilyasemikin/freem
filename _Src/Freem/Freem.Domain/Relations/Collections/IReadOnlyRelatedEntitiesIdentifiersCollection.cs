using Freem.Domain.Identifiers;

namespace Freem.Domain.Relations.Collections;

public interface IReadOnlyRelatedEntitiesIdentifiersCollection<TEntityIdentifier>
    where TEntityIdentifier : IEntityIdentifier
{
    int Count { get; }
    
    IEnumerable<TEntityIdentifier> Identifiers { get; }
    
    bool Contains(TEntityIdentifier identifier);

    static bool Equals(
        IReadOnlyRelatedEntitiesIdentifiersCollection<TEntityIdentifier> x, 
        IReadOnlyRelatedEntitiesIdentifiersCollection<TEntityIdentifier> y)
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);

        if (x.Count != y.Count)
            return false;

        var pairs = x.Identifiers.Zip(y.Identifiers);
        foreach (var (xItem, yItem) in pairs)
            if (!xItem.Equals(yItem))
                return false;

        return true;
    }
}