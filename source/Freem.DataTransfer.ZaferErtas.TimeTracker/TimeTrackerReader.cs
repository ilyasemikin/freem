using System.Globalization;
using System.Text;
using Freem.DataTransfer.ZaferErtas.TimeTracker.Models;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.DataTransfer.ZaferErtas.TimeTracker;

/// <summary>
/// Reads a CSV export of the Android application Time Tracker by Zafer Ertas (zzz1zzz.tracktime).
/// Lines that cannot be read are skipped and reported as issues; the file fails only on a missing or wrong header.
/// </summary>
public sealed class TimeTrackerReader
{
    private const string DateTimeFormat = "d MMM yyyy H:mm";

    public async Task<Result<TimeTrackerFile, Error>> ReadAsync(
        Stream input,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(input);

        using var reader = new StreamReader(input, Encoding.UTF8, true, leaveOpen: true);

        var header = await reader.ReadLineAsync(cancellationToken);
        if (header is null)
            return TimeTrackerErrorCatalog.Factories.CreateEmptyFile();
        if (!IsHeaderValid(header))
            return TimeTrackerErrorCatalog.Factories.CreateInvalidHeader();

        var state = new ReadState();
        var lineNumber = 1;
        while (await reader.ReadLineAsync(cancellationToken) is { } line)
        {
            lineNumber++;
            if (string.IsNullOrWhiteSpace(line))
                continue;

            state.RowsInFile++;
            ReadLine(line, lineNumber, state);
        }

        return new TimeTrackerFile(state.Rows, state.Issues, state.RowsInFile);
    }

    private static bool IsHeaderValid(string header)
    {
        var names = header
            .Split(',')
            .Select(name => name.Trim())
            .ToArray();
        return names.SequenceEqual(TimeTrackerColumns.Header, StringComparer.Ordinal);
    }

    private static void ReadLine(string line, int lineNumber, ReadState state)
    {
        var parts = line.Split(',');
        var minimalCount = TimeTrackerColumns.LeadingCount + TimeTrackerColumns.TrailingCount;
        if (parts.Length < minimalCount)
        {
            state.AddIssue(lineNumber, "Row has too few columns and is skipped");
            return;
        }

        var id = parts[TimeTrackerColumns.Id].Trim();
        if (id.Length == 0)
        {
            state.AddIssue(lineNumber, "Row has no ID and is skipped");
            return;
        }

        if (!state.Ids.Add(id))
        {
            state.AddIssue(lineNumber, $"Row ID \"{id}\" is repeated and the row is skipped");
            return;
        }

        var startText = $"{parts[TimeTrackerColumns.StartDate].Trim()} {parts[TimeTrackerColumns.StartTime].Trim()}";
        var endText = $"{parts[TimeTrackerColumns.EndDate].Trim()} {parts[TimeTrackerColumns.EndTime].Trim()}";
        if (!TryParseDateTime(startText, out var start) || !TryParseDateTime(endText, out var end))
        {
            state.AddIssue(lineNumber, "Row has an invalid start or end time and is skipped");
            return;
        }

        var activityId = parts[TimeTrackerColumns.ActivityId].Trim();
        var activityName = parts[TimeTrackerColumns.ActivityName].Trim();
        var notes = ReadNotes(parts);
        var row = new TimeTrackerRow(lineNumber, id, start, end, activityId, activityName, notes);
        state.Rows.Add(row);
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

    // Notes may contain commas, so the column takes everything between the leading and trailing columns.
    private static string ReadNotes(string[] parts)
    {
        var count = parts.Length - TimeTrackerColumns.LeadingCount - TimeTrackerColumns.TrailingCount;
        return string.Join(',', parts.Skip(TimeTrackerColumns.Notes).Take(count)).Trim();
    }

    private sealed class ReadState
    {
        public int RowsInFile { get; set; }
        public HashSet<string> Ids { get; } = [];
        public List<TimeTrackerRow> Rows { get; } = [];
        public List<TimeTrackerLineIssue> Issues { get; } = [];

        public void AddIssue(int lineNumber, string message)
        {
            var issue = new TimeTrackerLineIssue(lineNumber, message);
            Issues.Add(issue);
        }
    }
}
