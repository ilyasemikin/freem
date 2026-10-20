using Freem.Domain.Entities.Records;
using Freem.Domain.Records;
using Freem.Domain.Records.Models;
using Freem.Domain.Users;
using Freem.Domain.Storage.PostgreSQL.Database.Entities;
using Freem.Domain.Storage.PostgreSQL.Implementations.Mappers;
using Freem.Time.Models;

namespace Freem.Domain.Storage.PostgreSQL.Implementations.Repositories.Records;

internal static class RecordMapper
{
    public static RecordEntity MapToDatabaseEntity(this Record entity)
    {
        var id = entity.Id.ToString();
        var periods = entity.Periods
            .Select(p => p.MapToDatabaseEntity(id))
            .ToList();
        return new RecordEntity
        {
            Id = id,
            Name = entity.Name?.ToString(),
            Description = entity.Description?.ToString(),
            UserId = entity.UserId.ToString(),
            Periods = periods
        };
    }

    public static RecordPeriodEntity MapToDatabaseEntity(this RecordPeriod entity, string recordId)
    {
        return new RecordPeriodEntity
        {
            Id = entity.Id.ToString(),
            RecordId = recordId,
            Description = entity.Description?.ToString(),
            StartAt = entity.Period.StartAt,
            EndAt = entity.Period.EndAt
        };
    }

    public static RecordPeriod MapToDomainEntity(this RecordPeriodEntity entity)
    {
        var id = RecordPeriodId.Parse(entity.Id, null);
        var period = new DateTimePeriod(entity.StartAt, entity.EndAt);
        var description = entity.Description is null
            ? null
            : RecordPeriodDescription.Parse(entity.Description, null);
        return new RecordPeriod(id, period, description);
    }

    public static Record MapToDomainEntity(this RecordEntity entity)
    {
        var id = RecordId.Parse(entity.Id, null);
        var userId = UserId.Parse(entity.UserId, null);

        var activities = entity.Activities.MapToRelatedActivitiesCollection();
        var tags = entity.Tags.MapToRelatedTagsCollection();

        var recordPeriods = (entity.Periods ?? [])
            .Select(p => p.MapToDomainEntity());
        var periods = new RecordPeriodsCollection(recordPeriods);

        var record = new Record(id, userId, activities, tags, periods);
        if (entity.Name is not null)
            record.Name = RecordName.Parse(entity.Name, null);
        if (entity.Description is not null)
            record.Description = RecordDescription.Parse(entity.Description, null);

        return record;
    }
}
