using Freem.Entities.Records;
using Freem.Entities.Records.Identifiers;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Records;

internal static class RecordRelationFactory
{
    public static IEnumerable<RecordActivityRelationEntity> CreateDatabaseRecordActivityRelations(this Record entity)
    {
        return entity.Activities.Identifiers.Select(id => CreateDatabaseRecordActivityRelation(entity.Id, id));
    }

    public static RecordActivityRelationEntity CreateDatabaseRecordActivityRelation(RecordIdentifier recordId, string activityIdString)
    {
        return new RecordActivityRelationEntity
        {
            RecordId = recordId,
            ActivityId = activityIdString,
        };
    }

    public static IEnumerable<RecordTagRelationEntity> CreateDatabaseRecordTagRelations(this Record entity)
    {
        return entity.Tags.Identifiers.Select(id => CreateDatabaseRecordTagRelation(entity.Id, id));
    }

    public static RecordTagRelationEntity CreateDatabaseRecordTagRelation(RecordIdentifier recordId, string tagIdString)
    {
        return new RecordTagRelationEntity
        {
            RecordId = recordId,
            TagId = tagIdString
        };
    }
}