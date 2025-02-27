using Freem.Identifiers.Abstractions;

namespace Freem.Entities.Tags.Identifiers.Extensions;

public static class EnumerableExtensions
{
    public static bool HasTagsIdentifiers(this IEnumerable<IIdentifier> identifiers)
    {
        return identifiers.Any(id => id is TagIdentifier);
    }
}