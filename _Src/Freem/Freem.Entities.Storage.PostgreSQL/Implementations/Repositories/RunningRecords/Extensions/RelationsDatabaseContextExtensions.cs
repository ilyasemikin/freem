using Freem.Domain.Activities.Identifiers;
using Freem.Domain.Entities.RunningRecords;
using Freem.Domain.Tags.Identifiers;
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
        await context.UpdateRelatedEntitiesAsync<ActivityIdentifier, RunningRecordActivityRelationEntity>(
            entity.Activities,
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
        await context.UpdateRelatedEntitiesAsync<TagIdentifier, RunningRecordTagRelationEntity>(
            entity.Tags,
            e => e.RunningRecordUserId == entity.Id,
            e => e.TagId,
            ids => e => ids.Contains(e.TagId) && e.RunningRecordUserId == entity.UserId,
            id => RunningRecordRelationFactory.CreateDatabaseRunningRecordTagRelation(entity.Id, id),
            cancellationToken);
    }
}