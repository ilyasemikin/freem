using Freem.Entities.Identifiers;
using Freem.Entities.Storage.PostgreSQL.Database;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations;
using Freem.Entities.Storage.PostgreSQL.Implementations.Extensions;

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
            e => e.RunningRecordUserId == entity.Id.Value,
            e => e.ActivityId,
            ids => e => ids.Contains(e.ActivityId),
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
            e => e.RunningRecordUserId == entity.Id.Value,
            e => e.TagId,
            ids => e => ids.Contains(e.TagId),
            id => RunningRecordRelationFactory.CreateDatabaseRunningRecordTagRelation(entity.Id, id),
            cancellationToken);
    }
}