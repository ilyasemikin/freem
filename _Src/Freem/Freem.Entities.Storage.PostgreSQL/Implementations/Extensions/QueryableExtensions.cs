using Freem.Entities.Storage.Abstractions.Models;
using Microsoft.EntityFrameworkCore;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Extensions;

internal static class QueryableExtensions
{
    public static async Task<SearchEntitiesAsyncResult<TEntity>> CountAndMapAsync<TDatabaseEntity, TEntity>(
        this IQueryable<TDatabaseEntity> queryable,
        Func<TDatabaseEntity, TEntity> mapper,
        CancellationToken cancellationToken = default)
    {
        var totalCount = await queryable.CountAsync(cancellationToken);

        var enumerable = queryable.AsAsyncEnumerable();
        
        return SearchEntitiesAsyncResult<TEntity>.Create(enumerable, mapper, totalCount);
    }
}