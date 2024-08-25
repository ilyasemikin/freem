using System.Linq.Expressions;

namespace Freem.Sorting.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> OrderBy<T, TSortField, TSortKey>(
        this IQueryable<T> queryable, 
        IEnumerable<SortOption<TSortField>> options, 
        Func<TSortField, Expression<Func<T, TSortKey>>> selectorFactory)
        where TSortField : struct, Enum
    {
        using var iterator = options.GetEnumerator();

        if (!iterator.MoveNext())
            return queryable;

        var selector = selectorFactory(iterator.Current.Field);
        var orderedQueryable = iterator.Current.Order switch
        {
            SortOrder.Ascending => queryable.OrderBy(selector),
            SortOrder.Descending => queryable.OrderByDescending(selector),
            _ => throw new ArgumentOutOfRangeException(nameof(selector)),
        };

        while (iterator.MoveNext())
        {
            selector = selectorFactory(iterator.Current.Field);

            orderedQueryable = iterator.Current.Order switch
            {
                SortOrder.Ascending => orderedQueryable.ThenBy(selector),
                SortOrder.Descending => orderedQueryable.ThenByDescending(selector),
                _ => throw new ArgumentOutOfRangeException(nameof(selector))
            };
        }

        return orderedQueryable;
    }
}
