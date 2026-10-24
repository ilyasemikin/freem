using Freem.Domain.Relations.Collections.Exceptions;

namespace Freem.Domain.Relations.Collections.Base;

public class RelatedIdsCollection<TEntityId> 
    : IRelatedIdsCollection<TEntityId> 
    where TEntityId : Id
{
    private readonly HashSet<TEntityId> _ids;

    public const int DefaultMinCount = 0;
    public const int DefaultMaxCount = int.MaxValue;
    
    public int Count => _ids.Count;
    
    public IEnumerable<TEntityId> Ids => _ids;
    
    public int MinCount { get; }
    public int MaxCount { get; }

    public RelatedIdsCollection(
        IEnumerable<TEntityId>? ids = null,
        int minCount = DefaultMinCount,
        int maxCount = DefaultMaxCount)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(minCount);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxCount);

        ArgumentOutOfRangeException.ThrowIfGreaterThan(minCount, maxCount);

        ids ??= [];
        
        MinCount = minCount;
        MaxCount = maxCount;

        _ids = [];
        FillSet(_ids, ids);

        if (!IsCountAllowed(_ids.Count))
            throw new InvalidRelatedCountException(MinCount, MaxCount, _ids.Count);
    }
    
    public bool Contains(TEntityId id)
    {
        return _ids.Contains(id);
    }

    public bool TryAdd(TEntityId id)
    {
        if (_ids.Contains(id) || !IsCountAllowed(Count + 1))
            return false;
        
        _ids.Add(id);
        return true;
    }

    public bool TryRemove(TEntityId id)
    {
        return IsCountAllowed(Count - 1) && _ids.Remove(id);
    }

    protected void Update(IReadOnlyRelatedIdsCollection<TEntityId> other)
    {
        _ids.Clear();

        FillSet(_ids, other.Ids);
    }

    private bool IsCountAllowed(int newCount)
    {
        return newCount >= MinCount && newCount <= MaxCount;
    }

    private static void FillSet(HashSet<TEntityId> set, IEnumerable<TEntityId> ids)
    {
        foreach (var id in ids)
        {
            if (id is null)
                throw new InvalidOperationException("All identifiers must not be null");
            
            set.Add(id);
        }
    }
}