using Freem.Entities.Identifiers;
using Freem.Entities.Storage.PostgreSQL.Database.Entities;
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

    public static RunningRecord MapToDomainEntity(this RunningRecordEntity entity)
    {
        var userId = new UserIdentifier(entity.UserId);

        var categories = entity.Categories.MapToRelatedCategoriesCollection();
        var tags = entity.Tags.MapToRelatedTagsCollection();
        
        return new RunningRecord(userId, categories, tags, entity.StartAt);
    }
}
