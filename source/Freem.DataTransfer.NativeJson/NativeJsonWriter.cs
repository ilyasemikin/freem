using System.Text.Json;
using Freem.DataTransfer.NativeJson.Models;

namespace Freem.DataTransfer.NativeJson;

/// <summary>
/// Writes items as a native JSON file without buffering them. Items must be ordered by section:
/// tags, activities, records. Properties with null values are omitted.
/// </summary>
public sealed class NativeJsonWriter
{
    private const int FlushEveryItems = 256;

    public async Task WriteAsync(
        IAsyncEnumerable<NativeJsonItem> items,
        Stream output,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(items);
        ArgumentNullException.ThrowIfNull(output);

        await using var writer = new Utf8JsonWriter(output);

        writer.WriteStartObject();
        writer.WriteString(NativeJsonSchema.Properties.Format, NativeJsonSchema.FormatMarker);
        writer.WriteNumber(NativeJsonSchema.Properties.Version, NativeJsonSchema.CurrentVersion);

        var section = Section.None;
        var count = 0;
        await foreach (var item in items.WithCancellation(cancellationToken))
        {
            var itemSection = GetSection(item);
            if (itemSection < section)
                throw new InvalidOperationException("Items must be ordered: tags, activities, records");

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

    private static Section GetSection(NativeJsonItem item)
    {
        return item switch
        {
            NativeJsonTag => Section.Tags,
            NativeJsonActivity => Section.Activities,
            NativeJsonRecord => Section.Records,
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
            Section.Tags => NativeJsonSchema.Properties.Tags,
            Section.Activities => NativeJsonSchema.Properties.Activities,
            Section.Records => NativeJsonSchema.Properties.Records,
            _ => throw new ArgumentOutOfRangeException(nameof(section), section, null)
        };
    }

    private static void WriteItem(Utf8JsonWriter writer, NativeJsonItem item)
    {
        switch (item)
        {
            case NativeJsonTag tag:
                WriteTag(writer, tag);
                break;
            case NativeJsonActivity activity:
                WriteActivity(writer, activity);
                break;
            case NativeJsonRecord record:
                WriteRecord(writer, record);
                break;
        }
    }

    private static void WriteTag(Utf8JsonWriter writer, NativeJsonTag tag)
    {
        writer.WriteStartObject();
        WriteString(writer, NativeJsonSchema.Properties.Id, tag.Id);
        WriteString(writer, NativeJsonSchema.Properties.Name, tag.Name);
        writer.WriteEndObject();
    }

    private static void WriteActivity(Utf8JsonWriter writer, NativeJsonActivity activity)
    {
        writer.WriteStartObject();
        WriteString(writer, NativeJsonSchema.Properties.Id, activity.Id);
        WriteString(writer, NativeJsonSchema.Properties.Name, activity.Name);
        WriteString(writer, NativeJsonSchema.Properties.Color, activity.Color);
        WriteString(writer, NativeJsonSchema.Properties.Status, activity.Status);
        WriteStrings(writer, NativeJsonSchema.Properties.TagIds, activity.TagIds);
        writer.WriteEndObject();
    }

    private static void WriteRecord(Utf8JsonWriter writer, NativeJsonRecord record)
    {
        writer.WriteStartObject();
        WriteString(writer, NativeJsonSchema.Properties.Id, record.Id);
        WriteString(writer, NativeJsonSchema.Properties.Name, record.Name);
        WriteString(writer, NativeJsonSchema.Properties.Description, record.Description);
        WriteStrings(writer, NativeJsonSchema.Properties.ActivityIds, record.ActivityIds);
        WriteStrings(writer, NativeJsonSchema.Properties.TagIds, record.TagIds);

        if (record.Periods is not null)
        {
            writer.WriteStartArray(NativeJsonSchema.Properties.Periods);
            foreach (var period in record.Periods)
                WritePeriod(writer, period);

            writer.WriteEndArray();
        }

        writer.WriteEndObject();
    }

    private static void WritePeriod(Utf8JsonWriter writer, NativeJsonPeriod period)
    {
        writer.WriteStartObject();
        WriteString(writer, NativeJsonSchema.Properties.Id, period.Id);
        if (period.StartAt is not null)
            writer.WriteString(NativeJsonSchema.Properties.StartAt, period.StartAt.Value);
        if (period.EndAt is not null)
            writer.WriteString(NativeJsonSchema.Properties.EndAt, period.EndAt.Value);
        WriteString(writer, NativeJsonSchema.Properties.TimeZoneOffset, period.TimeZoneOffset);
        WriteString(writer, NativeJsonSchema.Properties.Description, period.Description);
        writer.WriteEndObject();
    }

    private static void WriteString(Utf8JsonWriter writer, string name, string? value)
    {
        if (value is not null)
            writer.WriteString(name, value);
    }

    private static void WriteStrings(Utf8JsonWriter writer, string name, IReadOnlyList<string>? values)
    {
        if (values is null)
            return;

        writer.WriteStartArray(name);
        foreach (var value in values)
            writer.WriteStringValue(value);

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
