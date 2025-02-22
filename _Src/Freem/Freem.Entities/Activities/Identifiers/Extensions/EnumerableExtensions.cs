using Freem.Entities.Identifiers;
using Freem.Identifiers.Abstractions;

namespace Freem.Entities.Activities.Identifiers.Extensions;

public static class EnumerableExtensions
{
    public static bool HasActivitiesIdentifiers(this IEnumerable<IIdentifier> identifiers)
    {
        return identifiers.Any(id => id is ActivityIdentifier);
    }
}