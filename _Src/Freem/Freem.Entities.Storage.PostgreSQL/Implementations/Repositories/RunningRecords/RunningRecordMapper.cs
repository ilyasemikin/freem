using Freem.Entities.Identifiers;
using Freem.Entities.Storage.PostgreSQL.Database.Entities;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations;
using Freem.Entities.Storage.PostgreSQL.Implementations.Mappers;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.RunningRecords;

internal static class RunningRecordMapper
{
    public static RunningRecordEntity MapToDatabaseEntity(this RunningRecord entity)
    {
        return new RunningRecordEntity
        {
            UserId = entity.UserId.Value,
            Name = entity.Name,
            Description = entity.Description
        };
    }

    public static IEnumerable<RunningRecordCategoryRelationEntity> MapToRunningRecordCategoryRelations(this RunningRecord entity)
    {
        return entity.Categories.Identifiers.Select(categoryId => MapToRunningRecordCategoryRelation(entity.Id, categoryId.Value));
    }

    public static RunningRecordCategoryRelationEntity MapToRunningRecordCategoryRelation(UserIdentifier runningRecordUserId, string categoryIdString)
    {
        return new RunningRecordCategoryRelationEntity
        {
            RunningRecordUserId = runningRecordUserId.Value,
            CategoryId = categoryIdString
        };
    }

    public static IEnumerable<RunningRecordTagRelationEntity> MapToRunningRecordTagRelations(this RunningRecord entity)
    {
        return entity.Tags.Identifiers.Select(tagId => MapToRunningRecordTagRelation(entity.Id, tagId.Value));
    }

    public static RunningRecordTagRelationEntity MapToRunningRecordTagRelation(UserIdentifier runningRecordUserId, string tagIdString)
    {
        return new RunningRecordTagRelationEntity
        {
            RunningRecordUserId = runningRecordUserId.Value,
            TagId = tagIdString
        };
    }

    public static RunningRecord MapToDomainEntity(this RunningRecordEntity entity)
    {
        var userId = new UserIdentifier(entity.UserId);

        var categories = entity.Categories.MapToRelatedCategoriesCollection();
        var tags = entity.Tags.MapToRelatedTagsCollection();
        return new RunningRecord(userId, categories, tags, entity.StartAt);
    }
}
