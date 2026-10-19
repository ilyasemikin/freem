namespace Freem.Domain.Tags.Extensions;

public static class EnumerableExtensions
{
    public static bool HasTagIds(this IEnumerable<Id> ids)
    {
        return ids.Any(id => id is TagId);
    }
}