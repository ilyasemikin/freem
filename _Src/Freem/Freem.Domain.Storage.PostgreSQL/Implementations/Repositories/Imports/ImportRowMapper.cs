using Freem.Domain.Entities.Import;
using Freem.Domain.Import;
using Freem.Domain.Records.Models;
using Freem.Domain.Storage.PostgreSQL.Database.Entities;
using Freem.Domain.Users.Models;
using Freem.Time.Models;

namespace Freem.Domain.Storage.PostgreSQL.Implementations.Repositories.Imports;

internal static class ImportRowMapper
{
    public static ImportRowEntity MapToDatabaseEntity(this ImportRow entity)
    {
        var id = entity.Id.ToString();
        var documentId = entity.DocumentId.ToString();
        var record = entity.Record;

        var periods = record.Periods
            .Select((p, position) => p.MapToDatabaseEntity(id, position))
            .ToList();
        var activities = record.ActivityIds
            .Select((a, position) => new ImportRowActivityEntity
            {
                RowId = id,
                ImportActivityId = a.ToString(),
                Position = position
            })
            .ToList();
        var tags = record.TagIds
            .Select((t, position) => new ImportRowTagEntity
            {
                RowId = id,
                ImportTagId = t.ToString(),
                Position = position
            })
            .ToList();

        return new ImportRowEntity
        {
            Id = id,
            DocumentId = documentId,
            ImportRecordId = record.Id.ToString(),
            Name = record.Name?.ToString(),
            Description = record.Description?.ToString(),
            FirstStartAt = record.Periods.Min(p => p.Period.StartAt),
            Periods = periods,
            Activities = activities,
            Tags = tags
        };
    }

    public static ImportRowPeriodEntity MapToDatabaseEntity(
        this ImportRecordPeriod entity,
        string rowId,
        int position)
    {
        return new ImportRowPeriodEntity
        {
            RowId = rowId,
            Position = position,
            Description = entity.Description?.ToString(),
            StartAt = entity.Period.StartAt,
            EndAt = entity.Period.EndAt,
            TimeZoneOffsetTicks = entity.TimeZoneOffset.Ticks
        };
    }

    public static ImportRecordPeriod MapToDomainEntity(this ImportRowPeriodEntity entity)
    {
        var period = new DateTimePeriod(entity.StartAt, entity.EndAt);
        var offsetValue = new TimeSpan(entity.TimeZoneOffsetTicks);
        var timeZoneOffset = new TimeZoneOffset(offsetValue);
        var description = entity.Description is null
            ? null
            : RecordPeriodDescription.Parse(entity.Description, null);
        return new ImportRecordPeriod(period, timeZoneOffset, description);
    }

    public static ImportRow MapToDomainEntity(this ImportRowEntity entity)
    {
        var id = ImportRowId.Parse(entity.Id, null);
        var documentId = ImportDocumentId.Parse(entity.DocumentId, null);

        var recordId = ImportRecordId.Parse(entity.ImportRecordId, null);
        var name = entity.Name is null
            ? null
            : RecordName.Parse(entity.Name, null);
        var description = entity.Description is null
            ? null
            : RecordDescription.Parse(entity.Description, null);
        var activityIds = (entity.Activities ?? [])
            .OrderBy(a => a.Position)
            .Select(a => ImportActivityId.Parse(a.ImportActivityId, null))
            .ToList();
        var tagIds = (entity.Tags ?? [])
            .OrderBy(t => t.Position)
            .Select(t => ImportTagId.Parse(t.ImportTagId, null))
            .ToList();
        var periods = (entity.Periods ?? [])
            .OrderBy(p => p.Position)
            .Select(p => p.MapToDomainEntity())
            .ToList();
        var record = new ImportRecord(recordId, name, description, activityIds, tagIds, periods);

        return new ImportRow(id, documentId, record);
    }
}
