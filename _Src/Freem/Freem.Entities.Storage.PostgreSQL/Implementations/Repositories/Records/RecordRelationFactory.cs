using Freem.Entities.Identifiers;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Records;

internal static class RecordRelationFactory
{
    public static IEnumerable<RecordCategoryRelationEntity> CreateDatabaseRecordCategoryRelations(this Record entity)
    {
        return entity.Categories.Identifiers.Select(id => CreateDatabaseRecordCategoryRelation(entity.Id, id.Value));
    }

    public static RecordCategoryRelationEntity CreateDatabaseRecordCategoryRelation(RecordIdentifier recordId, string categoryIdString)
    {
        return new RecordCategoryRelationEntity
        {
            RecordId = recordId.Value,
            CategoryId = categoryIdString,
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