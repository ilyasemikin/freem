using Freem.Entities.Activities;
using Freem.Entities.Activities.Identifiers;
using Freem.Entities.RunningRecords;
using Freem.Entities.Storage.PostgreSQL.Database;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations;
using Freem.Entities.Storage.PostgreSQL.Implementations.Extensions;
using Freem.Entities.Tags;
using Freem.Entities.Tags.Identifiers;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.RunningRecords.Extensions;

internal static class RelationsDatabaseContextExtensions
{
    public static async Task UpdateRelatedActivitiesAsync(
        this DatabaseContext context,
        RunningRecord entity,
        CancellationToken cancellationToken = default)
    {
        await context.UpdateRelatedEntitiesAsync<Activity, ActivityIdentifier, RunningRecordActivityRelationEntity>(
            entity,
            e => e.RunningRecordUserId == entity.Id,
            e => e.ActivityId,
            ids => e => ids.Contains(e.ActivityId) && e.RunningRecordUserId == entity.UserId,
            id => RunningRecordRelationFactory.CreateDatabaseRunningRecordActivityRelation(entity.Id, id),
            cancellationToken);
    }

    public static async Task UpdateRelatedTagsAsync(
        this DatabaseContext context,
        RunningRecord entity,
        CancellationToken cancellationToken = default)
    {
        await context.UpdateRelatedEntitiesAsync<Tag, TagIdentifier, RunningRecordTagRelationEntity>(
            entity,
            e => e.RunningRecordUserId == entity.Id,
            e => e.TagId,
            ids => e => ids.Contains(e.TagId) && e.RunningRecordUserId == entity.UserId,
            id => RunningRecordRelationFactory.CreateDatabaseRunningRecordTagRelation(entity.Id, id),
            cancellationToken);
    }
}