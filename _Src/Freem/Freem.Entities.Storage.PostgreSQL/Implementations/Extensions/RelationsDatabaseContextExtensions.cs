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

        var removeFilter = removeFilterFactory(idsToRemove);
        await context.RemoveRelationsAsync(removeFilter, cancellationToken);

        var dbRelationEntities = idsToAdd.Select(databaseRelationEntityFactory);
        await context.AddRangeAsync(dbRelationEntities, cancellationToken);
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

    private static async Task RemoveRelationsAsync<TDatabaseRelation>(
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
