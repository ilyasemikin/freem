using System.Linq.Expressions;
using Freem.Domain;
using Freem.Domain.Relations.Collections;
using Freem.Entities.Storage.PostgreSQL.Database;
using Microsoft.EntityFrameworkCore;
using Utilities.Collections;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Extensions;

internal static class RelationsDatabaseContextExtensions
{
    public static async Task UpdateRelatedEntitiesAsync<TRelatedEntityId, TDatabaseRelation>(
        this DatabaseContext context,
        IReadOnlyRelatedIdsCollection<TRelatedEntityId> relatedIds,
        Expression<Func<TDatabaseRelation, bool>> whereExpression,
        Expression<Func<TDatabaseRelation, string>> idSelectorExpression,
        Func<IEnumerable<string>, Expression<Func<TDatabaseRelation, bool>>> removeFilterFactory,
        Func<string, TDatabaseRelation> databaseRelationEntityFactory,
        CancellationToken cancellationToken = default)
            where TRelatedEntityId : Id
            where TDatabaseRelation : class
    {
        var existedIds = context.FindRelatedIds(whereExpression, idSelectorExpression);

        var ids = relatedIds.Ids.Select(id => id.ToString());
        var (idsToRemove, idsToAdd) = existedIds.ExceptMutual(ids);
        
        var dbRelationEntities = idsToAdd.Select(databaseRelationEntityFactory);
        await context.AddRangeAsync(dbRelationEntities, cancellationToken);
        
        var removeFilter = removeFilterFactory(idsToRemove);
        context.RemoveRelations(removeFilter);
    }
    
    private static IEnumerable<string> FindRelatedIds<TDatabaseRelation>(
        this DatabaseContext context,
        Expression<Func<TDatabaseRelation, bool>> whereExpression,
        Expression<Func<TDatabaseRelation, string>> idSelectorExpression)
            where TDatabaseRelation : class
    {
        return context
            .Set<TDatabaseRelation>()
            .Where(whereExpression)
            .Select(idSelectorExpression)
            .AsNoTracking()
            .AsEnumerable();
    }

    private static void RemoveRelations<TDatabaseRelation>(
        this DatabaseContext context,
        Expression<Func<TDatabaseRelation, bool>> whereExpression)
            where TDatabaseRelation : class
    {
        var items = context
            .Set<TDatabaseRelation>()
            .Where(whereExpression);

        context.RemoveRange(items);
    }
}
