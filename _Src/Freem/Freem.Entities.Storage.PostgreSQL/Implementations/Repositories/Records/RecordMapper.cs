using Freem.Entities.Identifiers;
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
            Id = entity.Id.Value,
            Name = entity.Name,
            Description = entity.Description,
            StartAt = entity.Period.StartAt,
            EndAt = entity.Period.EndAt,
            UserId = entity.UserId.Value,
        };
    }

    public static Record MapToDomainEntity(this RecordEntity entity)
    {
        var id = new RecordIdentifier(entity.Id);
        var userId = new UserIdentifier(entity.UserId);

        var activities = entity.Activities.MapToRelatedActivitiesCollection();
        var tags = entity.Tags.MapToRelatedTagsCollection();

        var period = new DateTimePeriod(entity.StartAt, entity.EndAt);

        var record = new Record(id, userId, activities, tags, period);
        if (entity.Name is not null)
            record.Name = entity.Name;
        if (entity.Description is not null)
            record.Description = entity.Description;

        return record;
    }
}
