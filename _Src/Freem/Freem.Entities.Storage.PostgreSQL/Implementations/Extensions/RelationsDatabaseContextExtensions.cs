using System.Linq.Expressions;
using Freem.Collections.Extensions;
using Freem.Entities.Abstractions;
using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.Abstractions.Relations;
using Freem.Entities.Storage.PostgreSQL.Database;
using Microsoft.EntityFrameworkCore;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Extensions;

internal static class RelationsDatabaseContextExtensions
{
    public static async Task UpdateRelatedEntitiesAsync<TRelatedEntity, TRelatedEntityIdentifier, TDatabaseRelation>(
        this DatabaseContext context,
        IEntityRelation<TRelatedEntity, TRelatedEntityIdentifier> entity,
        Expression<Func<TDatabaseRelation, bool>> whereExpression,
        Expression<Func<TDatabaseRelation, string>> idSelectorExpression,
        Func<IEnumerable<string>, Expression<Func<TDatabaseRelation, bool>>> removeFilterFactory,
        Func<string, TDatabaseRelation> databaseRelationEntityFactory,
        CancellationToken cancellationToken = default)
            where TRelatedEntity : IEntity<TRelatedEntityIdentifier>
            where TRelatedEntityIdentifier : IEntityIdentifier
            where TDatabaseRelation : class
    {
        var existedIds = context.FindRelatedIds(whereExpression, idSelectorExpression);

        var ids = entity.RelatedEntities.Identifiers.Select(id => id.ToString()!);
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
