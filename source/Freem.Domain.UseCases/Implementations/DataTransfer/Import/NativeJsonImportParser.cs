using System.Text.Json;
using Freem.Domain.Activities.Models;
using Freem.Domain.Entities.DataTransfer.Import;
using Freem.Domain.DataTransfer.Formats;
using Freem.Domain.DataTransfer.Import;
using Freem.Domain.Records.Models;
using Freem.Domain.Tags.Models;
using Freem.Domain.UseCases.Implementations.DataTransfer.Formats;
using Freem.Domain.UseCases.Implementations.DataTransfer.Import.NativeJson;
using Freem.Domain.Users.Models;
using Freem.Time;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Domain.UseCases.Implementations.DataTransfer.Import;

public sealed class NativeJsonImportParser : IImportParser
{
    private static readonly JsonSerializerOptions SerializerOptions = CreateSerializerOptions();

    public FormatDescription Format => NativeJsonFormat.Description;

    public async Task<Result<ImportParseResult, Error>> ParseAsync(
        Stream input,
        ImportParseOptions options,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(input);
        ArgumentNullException.ThrowIfNull(options);

        NativeJsonFile? file;
        try
        {
            file = await JsonSerializer.DeserializeAsync<NativeJsonFile>(input, SerializerOptions, cancellationToken);
        }
        catch (JsonException exception)
        {
            return CreateError($"File is not a valid JSON: {exception.Message}");
        }

        if (file is null)
            return CreateError("File is empty");

        try
        {
            return Map(file);
        }
        catch (NativeJsonParseException exception)
        {
            return CreateError(exception.Message);
        }
    }

    private static ImportParseResult Map(NativeJsonFile file)
    {
        if (file.Format != NativeJsonFormat.FormatMarker)
            throw new NativeJsonParseException($"Unexpected format \"{file.Format}\", expected \"{NativeJsonFormat.FormatMarker}\"");
        if (file.Version != NativeJsonFormat.CurrentVersion)
            throw new NativeJsonParseException($"Unsupported version \"{file.Version}\", supported version is {NativeJsonFormat.CurrentVersion}");

        var tags = (file.Tags ?? []).Select(MapTag).ToArray();
        var tagIds = CreateIdSet(tags.Select(tag => tag.Id), "tag");

        var activities = (file.Activities ?? [])
            .Select(activity => MapActivity(activity, tagIds))
            .ToArray();
        var activityIds = CreateIdSet(activities.Select(activity => activity.Id), "activity");

        var records = (file.Records ?? [])
            .Select(record => MapRecord(record, activityIds, tagIds))
            .ToArray();
        CreateIdSet(records.Select(record => record.Id), "record");

        return new ImportParseResult(tags, activities, records, records.Length, []);
    }

    private static ImportTag MapTag(NativeJsonTag tag)
    {
        var id = ParseId(tag.Id, "tag id", ImportTagId.Parse);
        var name = Parse(tag.Name, "tag name", TagName.Parse);
        return new ImportTag(id, name);
    }

    private static ImportActivity MapActivity(NativeJsonActivity activity, HashSet<ImportTagId> tagIds)
    {
        var id = ParseId(activity.Id, "activity id", ImportActivityId.Parse);
        var name = Parse(activity.Name, "activity name", ActivityName.Parse);
        var status = Parse(activity.Status, "activity status", ActivityStatus.Parse);
        var color = activity.Color is null
            ? null
            : Parse(activity.Color, "activity color", ActivityColor.Parse);
        var activityTagIds = ResolveIds(activity.TagIds, tagIds, "tag", ImportTagId.Parse, $"activity \"{id}\"");
        return new ImportActivity(id, name, color, status, activityTagIds);
    }

    private static ImportRecord MapRecord(
        NativeJsonRecord record,
        HashSet<ImportActivityId> activityIds,
        HashSet<ImportTagId> tagIds)
    {
        var id = ParseId(record.Id, "record id", ImportRecordId.Parse);
        var name = record.Name is null
            ? null
            : Parse(record.Name, "record name", RecordName.Parse);
        var description = record.Description is null
            ? null
            : Parse(record.Description, "record description", RecordDescription.Parse);
        var recordActivityIds = ResolveIds(record.ActivityIds, activityIds, "activity", ImportActivityId.Parse, $"record \"{id}\"");
        var recordTagIds = ResolveIds(record.TagIds, tagIds, "tag", ImportTagId.Parse, $"record \"{id}\"");

        if (record.Periods is null || record.Periods.Count == 0)
            throw new NativeJsonParseException($"Record \"{id}\" has no periods");

        var periods = record.Periods
            .Select(period => MapPeriod(period, id))
            .ToArray();
        return new ImportRecord(id, name, description, recordActivityIds, recordTagIds, periods);
    }

    private static ImportRecordPeriod MapPeriod(NativeJsonPeriod period, ImportRecordId recordId)
    {
        if (period.StartAt is null || period.EndAt is null)
            throw new NativeJsonParseException($"Period of record \"{recordId}\" has no start or end");
        if (period.EndAt <= period.StartAt)
            throw new NativeJsonParseException($"Period of record \"{recordId}\" must have positive duration");

        var dateTimePeriod = new DateTimePeriod(period.StartAt.Value, period.EndAt.Value);
        var offset = Parse(period.TimeZoneOffset, "time zone offset", TimeZoneOffset.Parse);
        var description = period.Description is null
            ? null
            : Parse(period.Description, "period description", RecordPeriodDescription.Parse);
        return new ImportRecordPeriod(dateTimePeriod, offset, description);
    }

    private static T ParseId<T>(string? id, string description, Func<string, IFormatProvider?, T> parse)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new NativeJsonParseException($"Some {description} is missing");

        return Parse(id, description, parse);
    }

    private static HashSet<T> CreateIdSet<T>(IEnumerable<T> ids, string entity)
    {
        var result = new HashSet<T>();
        foreach (var id in ids)
        {
            if (!result.Add(id))
                throw new NativeJsonParseException($"Duplicate {entity} id \"{id}\"");
        }

        return result;
    }

    private static T[] ResolveIds<T>(
        IReadOnlyList<string>? ids,
        HashSet<T> known,
        string entity,
        Func<string, IFormatProvider?, T> parse,
        string owner)
    {
        if (ids is null)
            return [];

        var result = new List<T>();
        foreach (var value in ids)
        {
            var id = Parse(value, $"{entity} id", parse);
            if (!known.Contains(id))
                throw new NativeJsonParseException($"The {owner} refers to unknown {entity} \"{id}\"");

            result.Add(id);
        }

        return result.ToArray();
    }

    private static T Parse<T>(string? value, string description, Func<string, IFormatProvider?, T> parse)
    {
        if (value is null)
            throw new NativeJsonParseException($"Value of {description} is missing");

        try
        {
            return parse(value, null);
        }
        catch (FormatException)
        {
            throw new NativeJsonParseException($"Invalid value of {description}: \"{value}\"");
        }
    }

    private static Error CreateError(string message)
    {
        return DomainErrorCatalog.Factories.CreateInvalidRequest(message);
    }

    private static JsonSerializerOptions CreateSerializerOptions()
    {
        return new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        };
    }

    private sealed class NativeJsonParseException : Exception
    {
        public NativeJsonParseException(string message)
            : base(message)
        {
        }
    }
}
