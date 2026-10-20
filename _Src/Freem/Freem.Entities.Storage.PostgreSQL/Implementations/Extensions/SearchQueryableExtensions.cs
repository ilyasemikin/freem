using System.Linq.Expressions;
using Freem.Domain;
using Microsoft.EntityFrameworkCore;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Extensions;

internal static class SearchQueryableExtensions
{
    public static async Task<Result<TEntity, Error>> FindAsync<TDatabaseEntity, TEntity>(
        this IQueryable<TDatabaseEntity> queryable,
        Id id,
        Expression<Func<TDatabaseEntity, bool>> predicate,
        Func<TDatabaseEntity, TEntity?> mapper,
        CancellationToken cancellationToken = default)
            where TDatabaseEntity : class
    {
        var dbEntity = await queryable
            .AsNoTracking()
            .FirstOrDefaultAsync(predicate, cancellationToken);

        if (dbEntity is null)
            return DomainErrorCatalog.Factories.CreateEntityNotFound(id);

        var entity = mapper(dbEntity);
        if (entity is null)
            return DomainErrorCatalog.Factories.CreateEntityNotFound(id);
        
        return Result<TEntity, Error>.CreateSuccess(entity);
    }
}