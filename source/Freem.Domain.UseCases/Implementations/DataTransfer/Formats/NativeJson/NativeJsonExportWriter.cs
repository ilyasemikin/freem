using System.Runtime.CompilerServices;
using Freem.DataTransfer.NativeJson;
using Freem.DataTransfer.NativeJson.Models;
using Freem.Domain.DataTransfer.Formats;
using Freem.Domain.Entities.DataTransfer.Export;
using Freem.Domain.Records.Models;
using Freem.Domain.UseCases.Implementations.DataTransfer.Export;

namespace Freem.Domain.UseCases.Implementations.DataTransfer.Formats.NativeJson;

public sealed class NativeJsonExportWriter : IExportWriter
{
    private readonly NativeJsonWriter _writer = new();

    public FormatDescription Format => NativeJsonFormat.Description;

    public async Task WriteAsync(
        IAsyncEnumerable<ExportItem> items,
        Stream output,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(items);
        ArgumentNullException.ThrowIfNull(output);

        var nativeItems = MapAsync(items, cancellationToken);
        await _writer.WriteAsync(nativeItems, output, cancellationToken);
    }

    private static async IAsyncEnumerable<NativeJsonItem> MapAsync(
        IAsyncEnumerable<ExportItem> items,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await foreach (var item in items.WithCancellation(cancellationToken))
            yield return Map(item);
    }

    private static NativeJsonItem Map(ExportItem item)
    {
        return item switch
        {
            ExportTag tag => MapTag(tag),
            ExportActivity activity => MapActivity(activity),
            ExportRecord record => MapRecord(record),
            _ => throw new ArgumentOutOfRangeException(nameof(item), item.GetType(), null)
        };
    }

    private static NativeJsonTag MapTag(ExportTag tag)
    {
        var id = tag.Id.ToString();
        var name = tag.Name.ToString();
        return new NativeJsonTag
        {
            Id = id,
            Name = name
        };
    }

    private static NativeJsonActivity MapActivity(ExportActivity activity)
    {
        var id = activity.Id.ToString();
        var name = activity.Name.ToString();
        var color = activity.Color.ToString();
        var status = activity.Status.ToString();
        var tagIds = activity.TagIds.Select(tagId => tagId.ToString()).ToArray();
        return new NativeJsonActivity
        {
            Id = id,
            Name = name,
            Color = color,
            Status = status,
            TagIds = tagIds
        };
    }

    private static NativeJsonRecord MapRecord(ExportRecord record)
    {
        var id = record.Id.ToString();
        var name = record.Name?.ToString();
        var description = record.Description?.ToString();
        var activityIds = record.ActivityIds.Select(activityId => activityId.ToString()).ToArray();
        var tagIds = record.TagIds.Select(tagId => tagId.ToString()).ToArray();
        var periods = record.Periods.Select(MapPeriod).ToArray();
        return new NativeJsonRecord
        {
            Id = id,
            Name = name,
            Description = description,
            ActivityIds = activityIds,
            TagIds = tagIds,
            Periods = periods
        };
    }

    private static NativeJsonPeriod MapPeriod(RecordPeriodSnapshot period)
    {
        var id = period.Id.ToString();
        var timeZoneOffset = period.TimeZoneOffset.ToString();
        var description = period.Description?.ToString();
        return new NativeJsonPeriod
        {
            Id = id,
            StartAt = period.Period.StartAt,
            EndAt = period.Period.EndAt,
            TimeZoneOffset = timeZoneOffset,
            Description = description
        };
    }
}
