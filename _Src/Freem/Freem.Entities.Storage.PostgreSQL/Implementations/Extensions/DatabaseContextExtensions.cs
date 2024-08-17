using Freem.Entities.Storage.PostgreSQL.Database;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Extensions;

internal static class DatabaseContextExtensions
{
    public static IEnumerable<string> FindRelatedIds<TDatabaseRelation>(
        this DatabaseContext context,
        Expression<Func<TDatabaseRelation, bool>> whereExpression,
        Expression<Func<TDatabaseRelation, string>> idSelectorExpression)
            where TDatabaseRelation : class
    {
        return context
            .Set<TDatabaseRelation>()
            .AsNoTracking()
            .Where(whereExpression)
            .Select(idSelectorExpression)
            .AsEnumerable();
    }

    public static async Task RemoveRelationsAsync<TDatabaseRelation>(
        this DatabaseContext context,
        Expression<Func<TDatabaseRelation, bool>> whereExpression,
        CancellationToken cancellationToken = default)
            where TDatabaseRelation : class
    {
        await context
            .Set<TDatabaseRelation>()
            .Where(whereExpression)
            .ExecuteDeleteAsync(cancellationToken);
    }
}
