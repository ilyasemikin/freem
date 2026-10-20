using Freem.Domain.Storage.Models.Filters.Abstractions;
using Microsoft.EntityFrameworkCore;
using Utilities.Primitives;

namespace Freem.Domain.Storage.PostgreSQL.Implementations.Extensions;

internal static class QueryableExtensions
{
    public static async Task<AsyncBatch<TEntity>> CountAndMapAsync<TDatabaseEntity, TEntity>(
        this IQueryable<TDatabaseEntity> queryable,
        Func<TDatabaseEntity, TEntity> mapper,
        CancellationToken cancellationToken = default)
    {
        var totalCount = await queryable.CountAsync(cancellationToken);

        var enumerable = queryable.AsAsyncEnumerable();
        var entities = MapAsync(enumerable, mapper);
        
        return new AsyncBatch<TEntity>(entities, totalCount);
    }

    public static async Task<AsyncBatch<TEntity>> CountAndMapAsync<TDatabaseEntity, TEntity>(
        this IQueryable<TDatabaseEntity> queryable,
        ILimitFilter filter,
        Func<TDatabaseEntity, TEntity> mapper,
        CancellationToken cancellationToken = default)
    {
        var totalCount = await queryable.CountAsync(cancellationToken);

        var enumerable = queryable
            .SliceByLimitFilter(filter)
            .AsAsyncEnumerable();
        var entities = MapAsync(enumerable, mapper);
        
        return new AsyncBatch<TEntity>(entities, totalCount);
    }

    private static async IAsyncEnumerable<TEntity> MapAsync<TDatabaseEntity, TEntity>(
        IAsyncEnumerable<TDatabaseEntity> enumerable,
        Func<TDatabaseEntity, TEntity> mapper)
    {
        await foreach (var entity in enumerable)
            yield return mapper(entity);
    }
}