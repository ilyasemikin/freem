using Freem.Entities.Identifiers;
using Freem.Entities.RunningRecords;
using Freem.Entities.Storage.PostgreSQL.Database.Entities;
using Freem.Entities.Storage.PostgreSQL.Implementations.Mappers;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.RunningRecords;

internal static class RunningRecordMapper
{
    public static RunningRecordEntity MapToDatabaseEntity(this RunningRecord entity)
    {
        return new RunningRecordEntity
        {
            UserId = entity.UserId,
            Name = entity.Name,
            Description = entity.Description,
            StartAt = entity.StartAt
        };
    }

    public static RunningRecord MapToDomainEntity(this RunningRecordEntity entity)
    {
        var userId = new UserIdentifier(entity.UserId);

        var activities = entity.Activities.MapToRelatedActivitiesCollection();
        var tags = entity.Tags.MapToRelatedTagsCollection();
        
        var record = new RunningRecord(userId, activities, tags, entity.StartAt);
        if (entity.Name is not null)
            record.Name = entity.Name;
        if (entity.Description is not null)
            record.Description = entity.Description;

        return record;
    }
}
