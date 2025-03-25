using Freem.Entities.Storage.Abstractions.Models.Filters.Abstractions;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Extensions;

internal static class FilterQueryableExtensions
{
    public static IQueryable<TDatabaseEntity> SliceByLimitFilter<TDatabaseEntity, TFilter>(
        this IQueryable<TDatabaseEntity> queryable,
        TFilter filter)
        where TFilter : ILimitFilter
    {
        return queryable
            .Take(filter.Limit.Value);
    }
    
    public static IQueryable<TDatabaseEntity> SliceByLimitAndOffsetFilter<TDatabaseEntity, TFilter>(
        this IQueryable<TDatabaseEntity> queryable,
        TFilter filter)
        where TFilter : ILimitFilter, IOffsetFilter
    {
        return queryable
            .Skip(filter.Offset.Value)
            .Take(filter.Limit.Value);
    }
}
