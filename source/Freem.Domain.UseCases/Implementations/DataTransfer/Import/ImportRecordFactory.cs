using Freem.Domain.Entities.DataTransfer.Import;
using Freem.Domain.Entities.Records;
using Freem.Domain.Records;
using Freem.Domain.Relations.Collections;
using Freem.Domain.Users;

namespace Freem.Domain.UseCases.Implementations.DataTransfer.Import;

/// <summary>
/// Creates records from the rows of an import.
/// </summary>
public static class ImportRecordFactory
{
    /// <summary>
    /// Returns null when the row breaks record invariants (non-positive duration, overlapping periods,
    /// too many activities or tags): such rows are skipped.
    /// </summary>
    public static Record? TryCreate(ImportRow row, UserId userId, ImportMappings mappings)
    {
        ArgumentNullException.ThrowIfNull(row);
        ArgumentNullException.ThrowIfNull(userId);
        ArgumentNullException.ThrowIfNull(mappings);

        var importRecord = row.Record;
        var activityIds = importRecord.ActivityIds
            .Select(mappings.GetActivity)
            .Distinct()
            .ToArray();
        var tagIds = importRecord.TagIds
            .Select(mappings.GetTag)
            .Distinct()
            .ToArray();
        if (activityIds.Length < RelatedActivityIdsCollection.MinActivitiesCount ||
            activityIds.Length > RelatedActivityIdsCollection.MaxActivitiesCount ||
            tagIds.Length > RelatedTagIdsCollection.MaxTagsCount)
            return null;

        if (importRecord.Periods.Any(p => p.Period.Duration <= TimeSpan.Zero))
            return null;

        var periods = importRecord.Periods
            .Select(CreatePeriod)
            .ToArray();
        if (!RecordPeriodsCollection.TryCreate(periods, out var collection))
            return null;

        var id = RecordId.Generate();
        var activities = new RelatedActivityIdsCollection(activityIds);
        var tags = new RelatedTagIdsCollection(tagIds);
        return new Record(id, userId, activities, tags, collection)
        {
            Name = importRecord.Name,
            Description = importRecord.Description
        };
    }

    private static RecordPeriod CreatePeriod(ImportRecordPeriod importPeriod)
    {
        var id = RecordPeriodId.Generate();
        return new RecordPeriod(id, importPeriod.Period, importPeriod.TimeZoneOffset, importPeriod.Description);
    }
}
