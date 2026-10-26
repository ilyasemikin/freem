using Freem.Domain.DataTransfer.Formats;
using Freem.Domain.DataTransfer.Import;
using Freem.Domain.Entities.Records;
using Freem.Domain.Records;
using Freem.Domain.Records.Models;
using Freem.Domain.Records.Sources;
using Freem.Domain.Users;
using Freem.Domain.Users.Models;
using Freem.Domain.Storage.PostgreSQL.Database.Entities;
using Freem.Domain.Storage.PostgreSQL.Implementations.Mappers;
using Freem.Time;

namespace Freem.Domain.Storage.PostgreSQL.Implementations.Repositories.Records;

internal static class RecordMapper
{
    public static RecordEntity MapToDatabaseEntity(this Record entity)
    {
        var id = entity.Id.ToString();
        var periods = entity.Periods
            .Select(p => p.MapToDatabaseEntity(id))
            .ToList();
        var importSource = entity.Source as ImportRecordSource;
        return new RecordEntity
        {
            Id = id,
            Name = entity.Name?.ToString(),
            Description = entity.Description?.ToString(),
            UserId = entity.UserId.ToString(),
            Periods = periods,
            SourceType = entity.Source.Type.ToString("G"),
            SourceImportDocumentId = importSource?.DocumentId.ToString(),
            SourceImportRowId = importSource?.RowId.ToString(),
            SourceImportFormat = importSource?.Format.ToString(),
            SourceImportRecordId = importSource?.ImportRecordId.ToString()
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
            EndAt = entity.Period.EndAt,
            TimeZoneOffsetTicks = entity.TimeZoneOffset.Ticks
        };
    }

    public static RecordPeriod MapToDomainEntity(this RecordPeriodEntity entity)
    {
        var id = RecordPeriodId.Parse(entity.Id, null);
        var period = new DateTimePeriod(entity.StartAt, entity.EndAt);
        var offsetValue = new TimeSpan(entity.TimeZoneOffsetTicks);
        var timeZoneOffset = new TimeZoneOffset(offsetValue);
        var description = entity.Description is null
            ? null
            : RecordPeriodDescription.Parse(entity.Description, null);
        return new RecordPeriod(id, period, timeZoneOffset, description);
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
        var source = entity.MapToRecordSource();

        var record = new Record(id, userId, activities, tags, periods, source);
        if (entity.Name is not null)
            record.Name = RecordName.Parse(entity.Name, null);
        if (entity.Description is not null)
            record.Description = RecordDescription.Parse(entity.Description, null);

        return record;
    }

    private static RecordSource MapToRecordSource(this RecordEntity entity)
    {
        var type = Enum.Parse<RecordSourceType>(entity.SourceType);
        return type switch
        {
            RecordSourceType.Manual => ManualRecordSource.Instance,
            RecordSourceType.RunningRecord => RunningRecordSource.Instance,
            RecordSourceType.Import => MapToImportRecordSource(entity),
            _ => throw new ArgumentOutOfRangeException(nameof(entity), type, "Unknown record source type")
        };
    }

    private static ImportRecordSource MapToImportRecordSource(RecordEntity entity)
    {
        var documentId = ImportDocumentId.Parse(entity.SourceImportDocumentId!, null);
        var rowId = ImportRowId.Parse(entity.SourceImportRowId!, null);
        var format = FormatKey.Parse(entity.SourceImportFormat!, null);
        var importRecordId = ImportRecordId.Parse(entity.SourceImportRecordId!, null);
        return new ImportRecordSource(documentId, rowId, format, importRecordId);
    }
}
