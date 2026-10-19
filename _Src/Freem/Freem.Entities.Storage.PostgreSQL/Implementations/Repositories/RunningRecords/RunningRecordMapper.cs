using Freem.Domain.Entities.RunningRecords;
using Freem.Domain.Records.Models;
using Freem.Domain.Users;
using Freem.Entities.Storage.PostgreSQL.Database.Entities;
using Freem.Entities.Storage.PostgreSQL.Implementations.Mappers;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.RunningRecords;

internal static class RunningRecordMapper
{
    public static RunningRecordEntity MapToDatabaseEntity(this RunningRecord entity)
    {
        return new RunningRecordEntity
        {
            UserId = entity.UserId.ToString(),
            Name = entity.Name?.ToString(),
            Description = entity.Description?.ToString(),
            StartAt = entity.StartAt
        };
    }

    public static RunningRecord MapToDomainEntity(this RunningRecordEntity entity)
    {
        var userId = UserId.Parse(entity.UserId, null);

        var activities = entity.Activities.MapToRelatedActivitiesCollection();
        var tags = entity.Tags.MapToRelatedTagsCollection();
        
        var record = new RunningRecord(userId, activities, tags, entity.StartAt);
        if (entity.Name is not null)
            record.Name = RecordName.Parse(entity.Name, null);
        if (entity.Description is not null)
            record.Description = RecordDescription.Parse(entity.Description, null);

        return record;
    }
}
