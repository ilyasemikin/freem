using System.Linq.Expressions;
using Freem.Domain.Identifiers;
using Freem.Domain.Relations.Collections;
using Freem.Entities.Storage.PostgreSQL.Database;
using Microsoft.EntityFrameworkCore;
using Utilities.Collections;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Extensions;

internal static class RelationsDatabaseContextExtensions
{
    public static async Task UpdateRelatedEntitiesAsync<TRelatedEntityIdentifier, TDatabaseRelation>(
        this DatabaseContext context,
        IReadOnlyRelatedEntitiesIdentifiersCollection<TRelatedEntityIdentifier> relatedIdentifiers,
        Expression<Func<TDatabaseRelation, bool>> whereExpression,
        Expression<Func<TDatabaseRelation, string>> idSelectorExpression,
        Func<IEnumerable<string>, Expression<Func<TDatabaseRelation, bool>>> removeFilterFactory,
        Func<string, TDatabaseRelation> databaseRelationEntityFactory,
        CancellationToken cancellationToken = default)
            where TRelatedEntityIdentifier : IEntityIdentifier
            where TDatabaseRelation : class
    {
        var existedIds = context.FindRelatedIds(whereExpression, idSelectorExpression);

        var ids = relatedIdentifiers.Identifiers.Select(id => id.ToString()!);
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
