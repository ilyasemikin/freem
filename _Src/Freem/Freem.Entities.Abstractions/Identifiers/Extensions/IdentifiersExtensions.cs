namespace Freem.Entities.Abstractions.Identifiers.Extensions;

public static class IdentifiersExtensions
{
    public static IEnumerable<string> AsValues(this IEnumerable<IEntityIdentifier> identifiers)
    {
        foreach (var identifier in identifiers)
            yield return identifier.Value;
    }
}
