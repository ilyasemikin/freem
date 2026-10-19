using Freem.Domain.Entities.Records;
using Freem.Domain.Records;
using Freem.Domain.Records.Models;
using Freem.Domain.Users;
using Freem.Entities.Storage.PostgreSQL.Database.Entities;
using Freem.Entities.Storage.PostgreSQL.Implementations.Mappers;
using Freem.Time.Models;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Records;

internal static class RecordMapper
{
    public static RecordEntity MapToDatabaseEntity(this Record entity)
    {
        return new RecordEntity
        {
            Id = entity.Id.ToString(),
            Name = entity.Name?.ToString(),
            Description = entity.Description?.ToString(),
            StartAt = entity.Period.StartAt,
            EndAt = entity.Period.EndAt,
            UserId = entity.UserId.ToString(),
        };
    }

    public static Record MapToDomainEntity(this RecordEntity entity)
    {
        var id = RecordId.Parse(entity.Id, null);
        var userId = UserId.Parse(entity.UserId, null);

        var activities = entity.Activities.MapToRelatedActivitiesCollection();
        var tags = entity.Tags.MapToRelatedTagsCollection();

        var period = new DateTimePeriod(entity.StartAt, entity.EndAt);

        var record = new Record(id, userId, activities, tags, period);
        if (entity.Name is not null)
            record.Name = RecordName.Parse(entity.Name, null);
        if (entity.Description is not null)
            record.Description = RecordDescription.Parse(entity.Description, null);

        return record;
    }
}
