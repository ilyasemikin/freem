using Freem.Entities.Identifiers;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.RunningRecords;

internal static class RunningRecordRelationFactory
{
    public static IEnumerable<RunningRecordCategoryRelationEntity> CreateDatabaseRunningRecordCategoryRelations(this RunningRecord entity)
    {
        return entity.Categories.Identifiers.Select(categoryId => CreateDatabaseRunningRecordCategoryRelation(entity.Id, categoryId.Value));
    }

    public static RunningRecordCategoryRelationEntity CreateDatabaseRunningRecordCategoryRelation(UserIdentifier runningRecordUserId, string categoryIdString)
    {
        return new RunningRecordCategoryRelationEntity
        {
            RunningRecordUserId = runningRecordUserId.Value,
            CategoryId = categoryIdString
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