using Freem.Domain.Activities;
using Freem.Domain.Entities.Records;
using Freem.Domain.Tags;
using Freem.Entities.Storage.PostgreSQL.Database;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations;
using Freem.Entities.Storage.PostgreSQL.Implementations.Extensions;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Records.Extensions;

internal static class RelationsDatabaseContextExtensions
{
    public static async Task UpdateRelatedActivitiesAsync(
        this DatabaseContext context,
        Record entity,
        CancellationToken cancellationToken = default)
    {
        var entityId = entity.Id.ToString();
        await context.UpdateRelatedEntitiesAsync<ActivityId, RecordActivityRelationEntity>(
            entity.Activities,
            e => e.RecordId == entityId,
            e => e.ActivityId,
            ids => e => ids.Contains(e.ActivityId) && e.RecordId == entityId,
            id => RecordRelationFactory.CreateDatabaseRecordActivityRelation(entity.Id, id),
            cancellationToken);
    }
    
    public static async Task UpdateRelatedTagsAsync(
        this DatabaseContext context,
        Record entity,
        CancellationToken cancellationToken = default)
    {
        var entityId = entity.Id.ToString();
        await context.UpdateRelatedEntitiesAsync<TagId, RecordTagRelationEntity>(
            entity.Tags,
            e => e.RecordId == entityId,
            e => e.TagId,
            ids => e => ids.Contains(e.TagId) && e.RecordId == entityId,
            id => RecordRelationFactory.CreateDatabaseRecordTagRelation(entity.Id, id),
            cancellationToken);
    }
}