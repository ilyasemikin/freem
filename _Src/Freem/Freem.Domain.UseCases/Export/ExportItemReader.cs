using System.Runtime.CompilerServices;
using Freem.Domain.Activities;
using Freem.Domain.Entities.Activities;
using Freem.Domain.Entities.Records;
using Freem.Domain.Entities.Tags;
using Freem.Domain.Entities.Export;
using Freem.Domain.Records.Models;
using Freem.Domain.Storage.Models.Sorting;
using Freem.Domain.Storage.Repositories.Activities;
using Freem.Domain.Storage.Repositories.Records;
using Freem.Domain.Storage.Repositories.Tags;
using Freem.Domain.Users;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.Sorting;

namespace Freem.Domain.UseCases.Export;

public sealed class ExportItemReader
{
    private const int DefaultBatchSize = 500;

    private readonly ITagRepository _tags;
    private readonly IActivityRepository _activities;
    private readonly IRecordRepository _records;
    private readonly int _batchSize;

    public ExportItemReader(
        ITagRepository tags,
        IActivityRepository activities,
        IRecordRepository records)
        : this(tags, activities, records, DefaultBatchSize)
    {
    }

    public ExportItemReader(
        ITagRepository tags,
        IActivityRepository activities,
        IRecordRepository records,
        int batchSize)
    {
        ArgumentNullException.ThrowIfNull(tags);
        ArgumentNullException.ThrowIfNull(activities);
        ArgumentNullException.ThrowIfNull(records);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(batchSize);

        _tags = tags;
        _activities = activities;
        _records = records;
        _batchSize = batchSize;
    }

    public async IAsyncEnumerable<ExportItem> ReadAsync(
        UserId userId,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(userId);

        await foreach (var tag in ReadTagsAsync(userId, cancellationToken))
            yield return tag;

        await foreach (var activity in ReadActivitiesAsync(userId, cancellationToken))
            yield return activity;

        await foreach (var record in ReadRecordsAsync(userId, cancellationToken))
            yield return record;
    }

    private async IAsyncEnumerable<ExportItem> ReadTagsAsync(
        UserId userId,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var offset = 0;
        while (true)
        {
            var filter = new TagByUserFilter(userId)
            {
                Limit = _batchSize,
                Offset = offset
            };

            var result = await _tags.FindAsync(filter, cancellationToken);
            if (!result.Success)
                throw new ExportReadException(result.Error);

            var batch = await result.Value.ToArrayAsync(cancellationToken);
            foreach (var tag in batch)
                yield return MapTag(tag);

            if (batch.Length < _batchSize)
                yield break;

            offset += batch.Length;
        }
    }

    private async IAsyncEnumerable<ExportItem> ReadActivitiesAsync(
        UserId userId,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var sorting = new SortOptions<ActivitySortField>(ActivitySortField.Id, SortDirection.Ascending);
        var offset = 0;
        while (true)
        {
            var filter = new ActivityByUserFilter(userId)
            {
                Limit = _batchSize,
                Offset = offset,
                Sorting = sorting
            };

            var result = await _activities.FindAsync(filter, cancellationToken);
            if (!result.Success)
                throw new ExportReadException(result.Error);

            var batch = await result.Value.ToArrayAsync(cancellationToken);
            foreach (var activity in batch)
                yield return MapActivity(activity);

            if (batch.Length < _batchSize)
                yield break;

            offset += batch.Length;
        }
    }

    private async IAsyncEnumerable<ExportItem> ReadRecordsAsync(
        UserId userId,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var offset = 0;
        while (true)
        {
            var filter = new RecordByUserFilter(userId)
            {
                Limit = _batchSize,
                Offset = offset
            };

            var result = await _records.FindAsync(filter, cancellationToken);
            if (!result.Success)
                throw new ExportReadException(result.Error);

            var batch = await result.Value.ToArrayAsync(cancellationToken);
            foreach (var record in batch)
                yield return MapRecord(record);

            if (batch.Length < _batchSize)
                yield break;

            offset += batch.Length;
        }
    }

    public static ExportTag MapTag(Tag tag)
    {
        ArgumentNullException.ThrowIfNull(tag);

        return new ExportTag(tag.Id, tag.Name);
    }

    public static ExportActivity MapActivity(Activity activity)
    {
        ArgumentNullException.ThrowIfNull(activity);

        var tagIds = activity.Tags.Ids.ToArray();
        return new ExportActivity(activity.Id, activity.Name, activity.Color, activity.Status, tagIds);
    }

    public static ExportRecord MapRecord(Record record)
    {
        ArgumentNullException.ThrowIfNull(record);

        var activityIds = record.Activities.Ids.ToArray();
        var tagIds = record.Tags.Ids.ToArray();
        var periods = record.Periods
            .Select(MapPeriod)
            .ToArray();
        return new ExportRecord(record.Id, record.Name, record.Description, activityIds, tagIds, periods);
    }

    private static RecordPeriodSnapshot MapPeriod(RecordPeriod period)
    {
        return new RecordPeriodSnapshot(period.Id, period.Period, period.TimeZoneOffset, period.Description);
    }
}
