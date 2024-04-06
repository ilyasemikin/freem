using Freem.Collections.Identifiers.Validation.Abstractions;
namespace Freem.Collections.Identifiers.Validation;

public sealed class RangeCountIdentifierActionCheckerStrategy : IIdentifierActionCheckerStrategy
{
    private string ErrorMessage => $"Items count must be in range [{MinCount}, {MaxCount}]";

    public int MinCount { get; }
    public int MaxCount { get; }

    public RangeCountIdentifierActionCheckerStrategy(int minCount, int maxCount)
    {
        if (minCount > maxCount)
            throw new InvalidOperationException($"'{nameof(minCount)}' cannot be more than {nameof(maxCount)}");

        MinCount = minCount; 
        MaxCount = maxCount;
    }

    public void CheckAdd(IdentifiersCollection current, string addedItem)
    {
        if (current.Count + 1 > MaxCount)
            throw new InvalidOperationException();
    }

    public void CheckAddRange(IdentifiersCollection current, IReadOnlyCollection<string> addedItems)
    {
        if (current.Count + addedItems.Count > MaxCount)
            throw new InvalidOperationException(ErrorMessage);
    }

    public void CheckClear(IdentifiersCollection current)
    {
        if (MinCount > 0)
            throw new InvalidOperationException(ErrorMessage);
    }

    public void CheckRemove(IdentifiersCollection current, string removedItem)
    {
        if (current.Count - 1 < MinCount)
            throw new InvalidOperationException(ErrorMessage);
    }

    public void CheckUpdate(IdentifiersCollection current, IReadOnlyCollection<string> updateItems)
    {
        if (updateItems.Count < MinCount || updateItems.Count > MaxCount)
            throw new InvalidOperationException(ErrorMessage);
    }
}
