using Freem.Domain.Entities.RunningRecords;
using Freem.Domain.RunningRecords;
using Freem.Domain.Storage.PostgreSQL.Database.Entities.Relations;

namespace Freem.Domain.Storage.PostgreSQL.Implementations.Repositories.RunningRecords;

internal static class RunningRecordRelationFactory
{
    public static IEnumerable<RunningRecordActivityRelationEntity> CreateDatabaseRunningRecordActivityRelations(this RunningRecord entity)
    {
        return entity.Activities.Ids
            .Select(id => id.ToString())
            .Select(activityId => CreateDatabaseRunningRecordActivityRelation(entity.Id, activityId));
    }

    public static RunningRecordActivityRelationEntity CreateDatabaseRunningRecordActivityRelation(RunningRecordId runningRecordId, string activityIdString)
    {
        return new RunningRecordActivityRelationEntity
        {
            RunningRecordUserId = runningRecordId.ToString(),
            ActivityId = activityIdString
        };
    }

    public static IEnumerable<RunningRecordTagRelationEntity> CreateDatabaseRunningRecordTagRelations(this RunningRecord entity)
    {
        return entity.Tags.Ids
            .Select(id => id.ToString())
            .Select(tagId => CreateDatabaseRunningRecordTagRelation(entity.Id, tagId));
    }

    public static RunningRecordTagRelationEntity CreateDatabaseRunningRecordTagRelation(RunningRecordId runningRecordId, string tagIdString)
    {
        return new RunningRecordTagRelationEntity
        {
            RunningRecordUserId = runningRecordId.ToString(),
            TagId = tagIdString
        };
    }
}