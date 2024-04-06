namespace Freem.Collections.Identifiers.Validation.Abstractions;

public interface IIdentifierActionCheckerStrategy
{
    void CheckAdd(IdentifiersCollection current, string addedItem);
    void CheckAddRange(IdentifiersCollection current, IReadOnlyCollection<string> addedItems);
    void CheckRemove(IdentifiersCollection current, string removedItem);
    void CheckClear(IdentifiersCollection current);
    void CheckUpdate(IdentifiersCollection current, IReadOnlyCollection<string> updateItems);

    static IIdentifierActionCheckerStrategy AllAllowedChecker { get; } = new AllAllowedIdentifierActionCheckerStrategy();

    static IIdentifierActionCheckerStrategy CreateRangeCountChecker(int minValue, int maxValue)
    {
        return new RangeCountIdentifierActionCheckerStrategy(minValue, maxValue);
    }
}
