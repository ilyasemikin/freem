namespace Freem.Sorting.Extensions;

public static class EnumerableExtensions
{
    public static IEnumerable<T> OrderBy<T, TSortField, TSortKey>(
        this IEnumerable<T> enumerable, 
        IEnumerable<SortOption<TSortField>> options, 
        Func<TSortField, Func<T, TSortKey>> selectorFactory)
        where TSortField : struct, Enum
    {
        using var iterator = options.GetEnumerator();

        if (iterator.MoveNext())
            return enumerable;

        var selector = selectorFactory(iterator.Current.Field);
        var orderedEnumerable = iterator.Current.Order switch
        {
            SortOrder.Ascending => enumerable.OrderBy(selector),
            SortOrder.Descending => enumerable.OrderByDescending(selector),
            _ => throw new InvalidOperationException()
        };

        while (iterator.MoveNext())
        {
            selector = selectorFactory(iterator.Current.Field);
            orderedEnumerable = iterator.Current.Order switch
            {
                SortOrder.Ascending => orderedEnumerable.OrderBy(selector),
                SortOrder.Descending => orderedEnumerable.OrderByDescending(selector),
                _ => throw new InvalidOperationException()
            };
        }

        return orderedEnumerable;
    }
}
