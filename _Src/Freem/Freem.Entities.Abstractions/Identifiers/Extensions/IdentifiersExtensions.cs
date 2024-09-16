namespace Freem.Entities.Abstractions.Identifiers.Extensions;

public static class IdentifiersExtensions
{
    public static IEnumerable<string> AsValues<TEntityIdentifier>(this IEnumerable<TEntityIdentifier> identifiers)
        where TEntityIdentifier : IEntityIdentifier
    {
        foreach (var identifier in identifiers)
            yield return identifier.Value;
    }
}
