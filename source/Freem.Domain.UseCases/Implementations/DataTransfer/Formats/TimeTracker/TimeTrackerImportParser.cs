using Freem.DataTransfer.ZaferErtas.TimeTracker;
using Freem.DataTransfer.ZaferErtas.TimeTracker.Models;
using Freem.Domain.Activities.Models;
using Freem.Domain.Entities.DataTransfer.Import;
using Freem.Domain.DataTransfer.Formats;
using Freem.Domain.DataTransfer.Import;
using Freem.Domain.Records.Models;
using Freem.Domain.UseCases.Implementations.DataTransfer.Import;
using Freem.Time;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Domain.UseCases.Implementations.DataTransfer.Formats.TimeTracker;

public sealed class TimeTrackerImportParser : IImportParser
{
    private readonly TimeTrackerReader _reader = new();

    public FormatDescription Format => TimeTrackerFormat.Description;

    public async Task<Result<ImportParseResult, Error>> ParseAsync(
        Stream input,
        ImportParseOptions options,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(input);
        ArgumentNullException.ThrowIfNull(options);

        var fileResult = await _reader.ReadAsync(input, cancellationToken);
        if (!fileResult.Success)
            return DomainErrorCatalog.Factories.CreateInvalidRequest(fileResult.Error.Message);

        var file = fileResult.Value;
        var state = new ParseState();
        foreach (var issue in file.Issues)
            state.AddWarning(issue.LineNumber, issue.Message);

        foreach (var row in file.Rows)
            ParseRow(row, options, state);

        var warnings = state.Warnings
            .OrderBy(warning => warning.LineNumber)
            .ToArray();
        var result = new ImportParseResult(
            [],
            state.Activities.Values.ToArray(),
            state.Records,
            file.RowsInFile,
            warnings);
        return result;
    }

    private static void ParseRow(TimeTrackerRow row, ImportParseOptions options, ParseState state)
    {
        if (!ImportRecordId.TryParse(row.Id, null, out var recordId))
        {
            state.AddWarning(row.LineNumber, "Row has no ID and is skipped");
            return;
        }

        if (!TryCreatePeriod(row, options, state, out var period))
            return;

        if (!TryResolveActivity(row, state, out var activityId))
            return;

        var name = ParseNotes(row, state);
        ImportActivityId[] activityIds = [activityId];
        ImportTagId[] tagIds = [];
        var recordPeriod = new ImportRecordPeriod(period, options.TimeZoneOffset);
        var record = new ImportRecord(recordId, name, null, activityIds, tagIds, [recordPeriod]);
        state.Records.Add(record);
    }

    private static bool TryCreatePeriod(
        TimeTrackerRow row,
        ImportParseOptions options,
        ParseState state,
        out DateTimePeriod period)
    {
        period = null!;

        var offset = options.TimeZoneOffset.Value;
        var startAt = new DateTimeOffset(row.Start, offset);
        var endAt = new DateTimeOffset(row.End, offset);
        if (endAt < startAt)
        {
            state.AddWarning(row.LineNumber, "Row ends before it starts and is skipped");
            return false;
        }

        period = new DateTimePeriod(startAt, endAt);
        return true;
    }

    private static bool TryResolveActivity(
        TimeTrackerRow row,
        ParseState state,
        out ImportActivityId activityId)
    {
        activityId = null!;

        if (row.ActivityId.Length == 0 || !ImportActivityId.TryParse(row.ActivityId, null, out var parsedId))
        {
            state.AddWarning(row.LineNumber, "Row has no activity ID and is skipped");
            return false;
        }

        if (!ActivityName.TryParse(row.ActivityName, null, out var activityName))
        {
            state.AddWarning(row.LineNumber, $"Activity name of row is empty or longer than {ActivityName.MaxLength} characters and the row is skipped");
            return false;
        }

        if (state.Activities.TryGetValue(row.ActivityId, out var existing))
        {
            if (existing.Name != activityName)
                state.AddWarning(row.LineNumber, $"Activity ID \"{row.ActivityId}\" has another name \"{row.ActivityName}\", the first name is kept");
        }
        else
        {
            IReadOnlyList<ImportTagId> tagIds = [];
            state.Activities.Add(row.ActivityId, new ImportActivity(parsedId, activityName, null, ActivityStatus.Active, tagIds));
        }

        activityId = parsedId;
        return true;
    }

    private static RecordName? ParseNotes(TimeTrackerRow row, ParseState state)
    {
        var notes = row.Notes;
        if (notes.Length == 0)
            return null;

        if (notes.Length > RecordName.MaxLength)
        {
            notes = Truncate(notes, RecordName.MaxLength);
            state.AddWarning(row.LineNumber, $"Notes are longer than {RecordName.MaxLength} characters and are truncated");
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

    private sealed class ParseState
    {
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
