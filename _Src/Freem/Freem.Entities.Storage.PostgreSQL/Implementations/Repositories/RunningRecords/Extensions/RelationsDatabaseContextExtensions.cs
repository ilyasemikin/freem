using Freem.Entities.Identifiers;
using Freem.Entities.Storage.PostgreSQL.Database;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations;
using Freem.Entities.Storage.PostgreSQL.Implementations.Extensions;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.RunningRecords.Extensions;

internal static class RelationsDatabaseContextExtensions
{
    public static async Task UpdateRelatedCategoriesAsync(
        this DatabaseContext context,
        RunningRecord entity,
        CancellationToken cancellationToken = default)
    {
        await context.UpdateRelatedEntitiesAsync<Category, CategoryIdentifier, RunningRecordCategoryRelationEntity>(
            entity,
            e => e.RunningRecordUserId == entity.Id.Value,
            e => e.CategoryId,
            ids => e => ids.Contains(e.CategoryId),
            id => RunningRecordRelationFactory.CreateDatabaseRunningRecordCategoryRelation(entity.Id, id),
            cancellationToken);
    }

    public static async Task UpdateRelatedTagsAsync(
        this DatabaseContext context,
        RunningRecord entity,
        CancellationToken cancellationToken = default)
    {
        await context.UpdateRelatedEntitiesAsync<Tag, TagIdentifier, RunningRecordTagRelationEntity>(
            entity,
            e => e.RunningRecordUserId == entity.Id.Value,
            e => e.TagId,
            ids => e => ids.Contains(e.TagId),
            id => RunningRecordRelationFactory.CreateDatabaseRunningRecordTagRelation(entity.Id, id),
            cancellationToken);
    }
}