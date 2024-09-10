using Freem.Entities.Identifiers;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.RunningRecords;

internal static class RunningRecordRelationFactory
{
    public static IEnumerable<RunningRecordActivityRelationEntity> CreateDatabaseRunningRecordActivityRelations(this RunningRecord entity)
    {
        return entity.Activities.Identifiers.Select(activityId => CreateDatabaseRunningRecordActivityRelation(entity.Id, activityId.Value));
    }

    public static RunningRecordActivityRelationEntity CreateDatabaseRunningRecordActivityRelation(UserIdentifier runningRecordUserId, string activityIdString)
    {
        return new RunningRecordActivityRelationEntity
        {
            RunningRecordUserId = runningRecordUserId.Value,
            ActivityId = activityIdString
        };
    }

    public static IEnumerable<RunningRecordTagRelationEntity> CreateDatabaseRunningRecordTagRelations(this RunningRecord entity)
    {
        return entity.Tags.Identifiers.Select(tagId => CreateDatabaseRunningRecordTagRelation(entity.Id, tagId.Value));
    }

    public static RunningRecordTagRelationEntity CreateDatabaseRunningRecordTagRelation(UserIdentifier runningRecordUserId, string tagIdString)
    {
        return new RunningRecordTagRelationEntity
        {
            RunningRecordUserId = runningRecordUserId.Value,
            TagId = tagIdString
        };
    }
}