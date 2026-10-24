using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Domain.Storage.PostgreSQL.Implementations.Extensions;

internal static class SearchQueryableExtensions
{
    public static async Task<Result<TEntity, Error>> FindAsync<TDatabaseEntity, TEntity>(
        this IQueryable<TDatabaseEntity> queryable,
        Func<Error> notFoundFactory,
        Expression<Func<TDatabaseEntity, bool>> predicate,
        Func<TDatabaseEntity, TEntity?> mapper,
        CancellationToken cancellationToken = default)
            where TDatabaseEntity : class
    {
        var dbEntity = await queryable
            .AsNoTracking()
            .FirstOrDefaultAsync(predicate, cancellationToken);

        if (dbEntity is null)
            return notFoundFactory();

        var entity = mapper(dbEntity);
        if (entity is null)
            return notFoundFactory();
        
        return Result<TEntity, Error>.CreateSuccess(entity);
    }
}