using Freem.Domain.Activities;
using Freem.Domain.Entities.RunningRecords;
using Freem.Domain.Tags;
using Freem.Domain.Storage.PostgreSQL.Database;
using Freem.Domain.Storage.PostgreSQL.Database.Entities.Relations;
using Freem.Domain.Storage.PostgreSQL.Implementations.Extensions;

namespace Freem.Domain.Storage.PostgreSQL.Implementations.Repositories.RunningRecords.Extensions;

internal static class RelationsDatabaseContextExtensions
{
    public static async Task UpdateRelatedActivitiesAsync(
        this DatabaseContext context,
        RunningRecord entity,
        CancellationToken cancellationToken = default)
    {
        var entityId = entity.Id.ToString();
        var userId = entity.UserId.ToString();
        await context.UpdateRelatedEntitiesAsync<ActivityId, RunningRecordActivityRelationEntity>(
            entity.Activities,
            e => e.RunningRecordUserId == entityId,
            e => e.ActivityId,
            ids => e => ids.Contains(e.ActivityId) && e.RunningRecordUserId == userId,
            id => RunningRecordRelationFactory.CreateDatabaseRunningRecordActivityRelation(entity.Id, id),
            cancellationToken);
    }

    public static async Task UpdateRelatedTagsAsync(
        this DatabaseContext context,
        RunningRecord entity,
        CancellationToken cancellationToken = default)
    {
        var entityId = entity.Id.ToString();
        var userId = entity.UserId.ToString();
        await context.UpdateRelatedEntitiesAsync<TagId, RunningRecordTagRelationEntity>(
            entity.Tags,
            e => e.RunningRecordUserId == entityId,
            e => e.TagId,
            ids => e => ids.Contains(e.TagId) && e.RunningRecordUserId == userId,
            id => RunningRecordRelationFactory.CreateDatabaseRunningRecordTagRelation(entity.Id, id),
            cancellationToken);
    }
}