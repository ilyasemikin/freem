namespace Freem.DataTransfer.ZaferErtas.TimeTracker.Models;

public sealed class TimeTrackerRow
{
    public int LineNumber { get; }
    public string Id { get; }
    public DateTime Start { get; }
    public DateTime End { get; }
    public string ActivityId { get; }
    public string ActivityName { get; }
    public string Notes { get; }

    public TimeTrackerRow(
        int lineNumber,
        string id,
        DateTime start,
        DateTime end,
        string activityId,
        string activityName,
        string notes)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(lineNumber);
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(activityId);
        ArgumentNullException.ThrowIfNull(activityName);
        ArgumentNullException.ThrowIfNull(notes);

        LineNumber = lineNumber;
        Id = id;
        Start = start;
        End = end;
        ActivityId = activityId;
        ActivityName = activityName;
        Notes = notes;
    }
}
