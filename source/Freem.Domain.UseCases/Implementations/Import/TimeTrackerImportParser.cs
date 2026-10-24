using System.Globalization;
using System.Text;
using Freem.Domain.Activities.Models;
using Freem.Domain.Entities.Import;
using Freem.Domain.Formats;
using Freem.Domain.Import;
using Freem.Domain.Records.Models;
using Freem.Domain.UseCases.Implementations.Formats;
using Freem.Time;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Domain.UseCases.Implementations.Import;

public sealed class TimeTrackerImportParser : IImportParser
{
    private const string DateTimeFormat = "d MMM yyyy H:mm";

    public FormatDescription Format => TimeTrackerFormat.Description;

    public async Task<Result<ImportParseResult, Error>> ParseAsync(
        Stream input,
        ImportParseOptions options,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(input);
        ArgumentNullException.ThrowIfNull(options);

        using var reader = new StreamReader(input, Encoding.UTF8, true, leaveOpen: true);

        var header = await reader.ReadLineAsync(cancellationToken);
        if (header is null)
            return CreateError("File is empty");
        if (!IsHeaderValid(header))
            return CreateError("File header does not match the Time Tracker export");

        var state = new ParseState();
        var lineNumber = 1;
        while (await reader.ReadLineAsync(cancellationToken) is { } line)
        {
            lineNumber++;
            if (string.IsNullOrWhiteSpace(line))
                continue;

            state.RowsInFile++;
            ParseLine(line, lineNumber, options, state);
        }

        var result = new ImportParseResult(
            [],
            state.Activities.Values.ToArray(),
            state.Records,
            state.RowsInFile,
            state.Warnings);
        return result;
    }

    private static bool IsHeaderValid(string header)
    {
        var names = header
            .Split(',')
            .Select(name => name.Trim())
            .ToArray();
        return names.SequenceEqual(TimeTrackerFormat.Columns.Header, StringComparer.Ordinal);
    }

    private static void ParseLine(string line, int lineNumber, ImportParseOptions options, ParseState state)
    {
        var parts = line.Split(',');
        var minimalCount = TimeTrackerFormat.Columns.LeadingCount + TimeTrackerFormat.Columns.TrailingCount;
        if (parts.Length < minimalCount)
        {
            state.AddWarning(lineNumber, "Row has too few columns and is skipped");
            return;
        }

        var rawId = parts[TimeTrackerFormat.Columns.Id].Trim();
        if (rawId.Length == 0 || !ImportRecordId.TryParse(rawId, null, out var recordId))
        {
            state.AddWarning(lineNumber, "Row has no ID and is skipped");
            return;
        }

        if (!state.RecordIds.Add(rawId))
        {
            state.AddWarning(lineNumber, $"Row ID \"{rawId}\" is repeated and the row is skipped");
            return;
        }

        if (!TryParseTime(parts, lineNumber, options, state, out var period))
            return;

        if (!TryResolveActivity(parts, lineNumber, state, out var activityId))
            return;

        var name = ParseNotes(parts, lineNumber, state);
        ImportActivityId[] activityIds = [activityId];
        ImportTagId[] tagIds = [];
        var recordPeriod = new ImportRecordPeriod(period, options.TimeZoneOffset);
        var record = new ImportRecord(recordId, name, null, activityIds, tagIds, [recordPeriod]);
        state.Records.Add(record);
    }

    private static bool TryParseTime(
        string[] parts,
        int lineNumber,
        ImportParseOptions options,
        ParseState state,
        out DateTimePeriod period)
    {
        period = null!;

        var startText = $"{parts[TimeTrackerFormat.Columns.StartDate].Trim()} {parts[TimeTrackerFormat.Columns.StartTime].Trim()}";
        var endText = $"{parts[TimeTrackerFormat.Columns.EndDate].Trim()} {parts[TimeTrackerFormat.Columns.EndTime].Trim()}";
        if (!TryParseDateTime(startText, out var start) || !TryParseDateTime(endText, out var end))
        {
            state.AddWarning(lineNumber, "Row has an invalid start or end time and is skipped");
            return false;
        }

        var offset = options.TimeZoneOffset.Value;
        var startAt = new DateTimeOffset(start, offset);
        var endAt = new DateTimeOffset(end, offset);
        if (endAt < startAt)
        {
            state.AddWarning(lineNumber, "Row ends before it starts and is skipped");
            return false;
        }

        period = new DateTimePeriod(startAt, endAt);
        return true;
    }

    private static bool TryParseDateTime(string text, out DateTime value)
    {
        return DateTime.TryParseExact(
            text,
            DateTimeFormat,
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out value);
    }

    private static bool TryResolveActivity(
        string[] parts,
        int lineNumber,
        ParseState state,
        out ImportActivityId activityId)
    {
        activityId = null!;

        var rawId = parts[TimeTrackerFormat.Columns.ActivityId].Trim();
        var rawName = parts[TimeTrackerFormat.Columns.ActivityName].Trim();
        if (rawId.Length == 0 || !ImportActivityId.TryParse(rawId, null, out var parsedId))
        {
            state.AddWarning(lineNumber, "Row has no activity ID and is skipped");
            return false;
        }

        if (!ActivityName.TryParse(rawName, null, out var activityName))
        {
            state.AddWarning(lineNumber, $"Activity name of row is empty or longer than {ActivityName.MaxLength} characters and the row is skipped");
            return false;
        }

        if (state.Activities.TryGetValue(rawId, out var existing))
        {
            if (existing.Name != activityName)
                state.AddWarning(lineNumber, $"Activity ID \"{rawId}\" has another name \"{rawName}\", the first name is kept");
        }
        else
        {
            IReadOnlyList<ImportTagId> tagIds = [];
            state.Activities.Add(rawId, new ImportActivity(parsedId, activityName, null, ActivityStatus.Active, tagIds));
        }

        activityId = parsedId;
        return true;
    }

    private static RecordName? ParseNotes(string[] parts, int lineNumber, ParseState state)
    {
        var leading = TimeTrackerFormat.Columns.Notes;
        var count = parts.Length - TimeTrackerFormat.Columns.LeadingCount - TimeTrackerFormat.Columns.TrailingCount;
        var notes = string.Join(',', parts.Skip(leading).Take(count)).Trim();
        if (notes.Length == 0)
            return null;

        if (notes.Length > RecordName.MaxLength)
        {
            notes = Truncate(notes, RecordName.MaxLength);
            state.AddWarning(lineNumber, $"Notes are longer than {RecordName.MaxLength} characters and are truncated");
        }

        return RecordName.Parse(notes, null);
    }

    private static string Truncate(string text, int maxLength)
    {
        var length = maxLength;
        if (char.IsHighSurrogate(text[length - 1]))
            length--;

        return text[..length].TrimEnd();
    }

    private static Error CreateError(string message)
    {
        return DomainErrorCatalog.Factories.CreateInvalidRequest(message);
    }

    private sealed class ParseState
    {
        public int RowsInFile { get; set; }
        public HashSet<string> RecordIds { get; } = [];
        public Dictionary<string, ImportActivity> Activities { get; } = [];
        public List<ImportRecord> Records { get; } = [];
        public List<ImportParseWarning> Warnings { get; } = [];

        public void AddWarning(int lineNumber, string message)
        {
            var warning = new ImportParseWarning(lineNumber, message);
            Warnings.Add(warning);
        }
    }
}
