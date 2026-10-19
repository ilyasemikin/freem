namespace Freem.Domain.Relations.Collections;

public interface IReadOnlyRelatedIdsCollection<TEntityId>
    where TEntityId : Id
{
    int Count { get; }
    
    IEnumerable<TEntityId> Ids { get; }
    
    bool Contains(TEntityId id);

    static bool Equals(
        IReadOnlyRelatedIdsCollection<TEntityId> x, 
        IReadOnlyRelatedIdsCollection<TEntityId> y)
    {
        ArgumentNullException.ThrowIfNull(x);
        ArgumentNullException.ThrowIfNull(y);

        if (x.Count != y.Count)
            return false;

        var pairs = x.Ids.Zip(y.Ids);
        foreach (var (xItem, yItem) in pairs)
            if (!xItem.Equals(yItem))
                return false;

        return true;
    }
}