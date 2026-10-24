using Freem.Domain.Entities.Records;
using Freem.Domain.Records;
using Freem.Domain.Storage.PostgreSQL.Database.Entities.Relations;

namespace Freem.Domain.Storage.PostgreSQL.Implementations.Repositories.Records;

internal static class RecordRelationFactory
{
    public static IEnumerable<RecordActivityRelationEntity> CreateDatabaseRecordActivityRelations(this Record entity)
    {
        return entity.Activities.Ids
            .Select(id => id.ToString())
            .Select(activityId => CreateDatabaseRecordActivityRelation(entity.Id, activityId));
    }

    public static RecordActivityRelationEntity CreateDatabaseRecordActivityRelation(RecordId recordId, string activityIdString)
    {
        return new RecordActivityRelationEntity
        {
            RecordId = recordId.ToString(),
            ActivityId = activityIdString,
        };
    }

    public static IEnumerable<RecordTagRelationEntity> CreateDatabaseRecordTagRelations(this Record entity)
    {
        return entity.Tags.Ids
            .Select(id => id.ToString())
            .Select(tagId => CreateDatabaseRecordTagRelation(entity.Id, tagId));
    }

    public static RecordTagRelationEntity CreateDatabaseRecordTagRelation(RecordId recordId, string tagIdString)
    {
        return new RecordTagRelationEntity
        {
            RecordId = recordId.ToString(),
            TagId = tagIdString
        };
    }
}