using Freem.Domain.Identifiers;
using Freem.Domain.Relations.Collections.Exceptions;

namespace Freem.Domain.Relations.Collections.Base;

public class RelatedEntitiesIdentifiersCollection<TEntityIdentifier> 
    : IRelatedEntitiesIdentifiersCollection<TEntityIdentifier> 
    where TEntityIdentifier : IEntityIdentifier
{
    private readonly HashSet<TEntityIdentifier> _identifiers;

    public const int DefaultMinCount = 0;
    public const int DefaultMaxCount = int.MaxValue;
    
    public int Count => _identifiers.Count;
    
    public IEnumerable<TEntityIdentifier> Identifiers => _identifiers;
    
    public int MinCount { get; }
    public int MaxCount { get; }

    public RelatedEntitiesIdentifiersCollection(
        IEnumerable<TEntityIdentifier>? identifiers = null,
        int minCount = DefaultMinCount,
        int maxCount = DefaultMaxCount)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(minCount);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxCount);

        ArgumentOutOfRangeException.ThrowIfGreaterThan(minCount, maxCount);

        identifiers ??= [];
        
        MinCount = minCount;
        MaxCount = maxCount;

        _identifiers = [];
        FillSet(_identifiers, identifiers);

        if (!IsCountAllowed(_identifiers.Count))
            throw new InvalidRelatedCountException(MinCount, MaxCount, _identifiers.Count);
    }
    
    public bool Contains(TEntityIdentifier identifier)
    {
        return _identifiers.Contains(identifier);
    }

    public bool TryAdd(TEntityIdentifier identifier)
    {
        if (_identifiers.Contains(identifier) || !IsCountAllowed(Count + 1))
            return false;
        
        _identifiers.Add(identifier);
        return true;
    }

    public bool TryRemove(TEntityIdentifier identifier)
    {
        return IsCountAllowed(Count - 1) && _identifiers.Remove(identifier);
    }

    protected void Update(IReadOnlyRelatedEntitiesIdentifiersCollection<TEntityIdentifier> other)
    {
        _identifiers.Clear();

        FillSet(_identifiers, other.Identifiers);
    }

    private bool IsCountAllowed(int newCount)
    {
        return newCount >= MinCount && newCount <= MaxCount;
    }

    private static void FillSet(HashSet<TEntityIdentifier> set, IEnumerable<TEntityIdentifier> identifiers)
    {
        foreach (var id in identifiers)
        {
            if (id is null)
                throw new InvalidOperationException("All identifiers must not be null");
            
            set.Add(id);
        }
    }
}