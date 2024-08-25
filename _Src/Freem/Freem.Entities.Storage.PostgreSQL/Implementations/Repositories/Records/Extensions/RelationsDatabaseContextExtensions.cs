using Freem.Entities.Identifiers;
using Freem.Entities.Storage.PostgreSQL.Database;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations;
using Freem.Entities.Storage.PostgreSQL.Implementations.Extensions;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Records.Extensions;

internal static class RelationsDatabaseContextExtensions
{
    public static async Task UpdateRelatedCategoriesAsync(
        this DatabaseContext context,
        Record entity,
        CancellationToken cancellationToken = default)
    {
        await context.UpdateRelatedEntitiesAsync<Category, CategoryIdentifier, RecordCategoryRelationEntity>(
            entity,
            e => e.RecordId == entity.Id.Value,
            e => e.RecordId,
            ids => e => ids.Contains(e.CategoryId),
            id => RecordRelationFactory.CreateDatabaseRecordCategoryRelation(entity.Id, id),
            cancellationToken);
    }
    
    public static async Task UpdateRelatedTagsAsync(
        this DatabaseContext context,
        Record entity,
        CancellationToken cancellationToken = default)
    {
        await context.UpdateRelatedEntitiesAsync<Tag, TagIdentifier, RecordTagRelationEntity>(
            entity,
            e => e.RecordId == entity.Id.Value,
            e => e.RecordId,
            ids => e => ids.Contains(e.TagId),
            id => RecordRelationFactory.CreateDatabaseRecordTagRelation(entity.Id, id),
            cancellationToken);
    }
}