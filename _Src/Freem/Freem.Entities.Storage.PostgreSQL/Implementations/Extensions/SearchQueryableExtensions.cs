using System.Linq.Expressions;
using Freem.Entities.Storage.Abstractions.Models;
using Microsoft.EntityFrameworkCore;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Extensions;

internal static class SearchQueryableExtensions
{
    public static async Task<SearchEntityResult<TEntity>> FindAsync<TDatabaseEntity, TEntity>(
        this IQueryable<TDatabaseEntity> queryable,
        Expression<Func<TDatabaseEntity, bool>> predicate,
        Func<TDatabaseEntity, TEntity> mapper,
        CancellationToken cancellationToken = default)
            where TDatabaseEntity : class
    {
        var dbEntity = await queryable
            .AsNoTracking()
            .FirstOrDefaultAsync(predicate, cancellationToken);

        if (dbEntity is null)
            return SearchEntityResult<TEntity>.NotFound();

        var entity = mapper(dbEntity);
        return SearchEntityResult<TEntity>.Found(entity);
    }
}