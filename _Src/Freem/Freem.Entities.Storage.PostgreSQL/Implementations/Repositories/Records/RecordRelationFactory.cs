using Freem.Entities.Identifiers;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Records;

internal static class RecordRelationFactory
{
    public static IEnumerable<RecordActivityRelationEntity> CreateDatabaseRecordActivityRelations(this Record entity)
    {
        return entity.Activities.Identifiers.Select(id => CreateDatabaseRecordActivityRelation(entity.Id, id.Value));
    }

    public static RecordActivityRelationEntity CreateDatabaseRecordActivityRelation(RecordIdentifier recordId, string activityIdString)
    {
        return new RecordActivityRelationEntity
        {
            RecordId = recordId.Value,
            ActivityId = activityIdString,
        };
    }

    public static IEnumerable<RecordTagRelationEntity> CreateDatabaseRecordTagRelations(this Record entity)
    {
        return entity.Tags.Identifiers.Select(id => CreateDatabaseRecordTagRelation(entity.Id, id.Value));
    }

    public static RecordTagRelationEntity CreateDatabaseRecordTagRelation(RecordIdentifier recordId, string tagIdString)
    {
        return new RecordTagRelationEntity
        {
            RecordId = recordId.Value,
            TagId = tagIdString
        };
    }
}