using System.Text.Json;
using Freem.Domain.Entities.DataTransfer.Export;
using Freem.Domain.DataTransfer.Formats;
using Freem.Domain.UseCases.Implementations.DataTransfer.Formats;

namespace Freem.Domain.UseCases.Implementations.DataTransfer.Export;

public sealed class NativeJsonExportWriter : IExportWriter
{
    private const int FlushEveryItems = 256;

    public FormatDescription Format => NativeJsonFormat.Description;

    public async Task WriteAsync(
        IAsyncEnumerable<ExportItem> items,
        Stream output,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(items);
        ArgumentNullException.ThrowIfNull(output);

        await using var writer = new Utf8JsonWriter(output);

        writer.WriteStartObject();
        writer.WriteString(NativeJsonFormat.Properties.Format, NativeJsonFormat.FormatMarker);
        writer.WriteNumber(NativeJsonFormat.Properties.Version, NativeJsonFormat.CurrentVersion);

        var section = Section.None;
        var count = 0;
        await foreach (var item in items.WithCancellation(cancellationToken))
        {
            var itemSection = GetSection(item);
            if (itemSection < section)
                throw new InvalidOperationException("Export items must be ordered: tags, activities, records");

            section = SwitchSection(writer, section, itemSection);
            WriteItem(writer, item);

            count++;
            if (count % FlushEveryItems == 0)
                await writer.FlushAsync(cancellationToken);
        }

        SwitchSection(writer, section, Section.Done);
        writer.WriteEndObject();

        await writer.FlushAsync(cancellationToken);
    }

    private static Section GetSection(ExportItem item)
    {
        return item switch
        {
            ExportTag => Section.Tags,
            ExportActivity => Section.Activities,
            ExportRecord => Section.Records,
            _ => throw new ArgumentOutOfRangeException(nameof(item), item.GetType(), null)
        };
    }

    private static Section SwitchSection(Utf8JsonWriter writer, Section current, Section target)
    {
        if (current == target)
            return current;

        if (current != Section.None)
            writer.WriteEndArray();

        var next = current + 1;
        while (next < target)
        {
            WriteEmptySection(writer, next);
            next++;
        }

        if (target != Section.Done)
            writer.WriteStartArray(GetSectionName(target));

        return target;
    }

    private static void WriteEmptySection(Utf8JsonWriter writer, Section section)
    {
        writer.WriteStartArray(GetSectionName(section));
        writer.WriteEndArray();
    }

    private static string GetSectionName(Section section)
    {
        return section switch
        {
            Section.Tags => NativeJsonFormat.Properties.Tags,
            Section.Activities => NativeJsonFormat.Properties.Activities,
            Section.Records => NativeJsonFormat.Properties.Records,
            _ => throw new ArgumentOutOfRangeException(nameof(section), section, null)
        };
    }

    private static void WriteItem(Utf8JsonWriter writer, ExportItem item)
    {
        switch (item)
        {
            case ExportTag tag:
                WriteTag(writer, tag);
                break;
            case ExportActivity activity:
                WriteActivity(writer, activity);
                break;
            case ExportRecord record:
                WriteRecord(writer, record);
                break;
        }
    }

    private static void WriteTag(Utf8JsonWriter writer, ExportTag tag)
    {
        writer.WriteStartObject();
        writer.WriteString(NativeJsonFormat.Properties.Id, tag.Id.ToString());
        writer.WriteString(NativeJsonFormat.Properties.Name, tag.Name.ToString());
        writer.WriteEndObject();
    }

    private static void WriteActivity(Utf8JsonWriter writer, ExportActivity activity)
    {
        writer.WriteStartObject();
        writer.WriteString(NativeJsonFormat.Properties.Id, activity.Id.ToString());
        writer.WriteString(NativeJsonFormat.Properties.Name, activity.Name.ToString());
        writer.WriteString(NativeJsonFormat.Properties.Color, activity.Color.ToString());
        writer.WriteString(NativeJsonFormat.Properties.Status, activity.Status.ToString());
        WriteIds(writer, NativeJsonFormat.Properties.TagIds, activity.TagIds.Select(id => id.ToString()));
        writer.WriteEndObject();
    }

    private static void WriteRecord(Utf8JsonWriter writer, ExportRecord record)
    {
        writer.WriteStartObject();
        writer.WriteString(NativeJsonFormat.Properties.Id, record.Id.ToString());
        if (record.Name is not null)
            writer.WriteString(NativeJsonFormat.Properties.Name, record.Name.ToString());
        if (record.Description is not null)
            writer.WriteString(NativeJsonFormat.Properties.Description, record.Description.ToString());
        WriteIds(writer, NativeJsonFormat.Properties.ActivityIds, record.ActivityIds.Select(id => id.ToString()));
        WriteIds(writer, NativeJsonFormat.Properties.TagIds, record.TagIds.Select(id => id.ToString()));

        writer.WriteStartArray(NativeJsonFormat.Properties.Periods);
        foreach (var period in record.Periods)
        {
            writer.WriteStartObject();
            writer.WriteString(NativeJsonFormat.Properties.Id, period.Id.ToString());
            writer.WriteString(NativeJsonFormat.Properties.StartAt, period.Period.StartAt);
            writer.WriteString(NativeJsonFormat.Properties.EndAt, period.Period.EndAt);
            writer.WriteString(NativeJsonFormat.Properties.TimeZoneOffset, period.TimeZoneOffset.ToString());
            if (period.Description is not null)
                writer.WriteString(NativeJsonFormat.Properties.Description, period.Description.ToString());
            writer.WriteEndObject();
        }

        writer.WriteEndArray();
        writer.WriteEndObject();
    }

    private static void WriteIds(Utf8JsonWriter writer, string name, IEnumerable<string> ids)
    {
        writer.WriteStartArray(name);
        foreach (var id in ids)
            writer.WriteStringValue(id);

        writer.WriteEndArray();
    }

    private enum Section
    {
        None = 0,
        Tags = 1,
        Activities = 2,
        Records = 3,
        Done = 4
    }
}
