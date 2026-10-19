namespace Freem.Domain.Activities.Extensions;

public static class EnumerableExtensions
{
    public static bool HasActivityIds(this IEnumerable<Id> ids)
    {
        return ids.Any(id => id is ActivityId);
    }
}