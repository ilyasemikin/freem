using Freem.Collections.Identifiers.Validation.Abstractions;

namespace Freem.Collections.Identifiers.Validation;

public sealed class AllAllowedIdentifierActionCheckerStrategy : IIdentifierActionCheckerStrategy
{
    internal AllAllowedIdentifierActionCheckerStrategy()
    {
    }

    public void CheckAdd(IdentifiersCollection current, string addedItem)
    {
    }

    public void CheckAddRange(IdentifiersCollection current, IReadOnlyCollection<string> addedItems)
    {
    }

    public void CheckClear(IdentifiersCollection current)
    {
    }

    public void CheckRemove(IdentifiersCollection current, string removedItem)
    {
    }

    public void CheckUpdate(IdentifiersCollection current, IReadOnlyCollection<string> updateItems)
    {
    }
}
