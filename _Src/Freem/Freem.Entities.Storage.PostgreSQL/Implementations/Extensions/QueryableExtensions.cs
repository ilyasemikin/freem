using Freem.Entities.Storage.Abstractions.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Extensions;

internal static class QueryableExtensions
{
    public static async Task<SearchEntityResult<TEntity>> FindAsync<TDatabaseEntity, TEntity>(
        this IQueryable<TDatabaseEntity> queryable,
        Expression<Func<TDatabaseEntity, bool>> predicate,
        Func<TDatabaseEntity, TEntity> mapper)
    {
        var dbEntity = await queryable.FirstOrDefaultAsync(predicate);
        if (dbEntity is null)
            return SearchEntityResult<TEntity>.NotFound();

        var entity = mapper(dbEntity);
        return SearchEntityResult<TEntity>.Found(entity);
    }
}
