namespace Freem.DataTransfer.ZaferErtas.TimeTracker.Models;

public sealed class TimeTrackerLineIssue
{
    public int LineNumber { get; }
    public string Message { get; }

    public TimeTrackerLineIssue(int lineNumber, string message)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(lineNumber);
        ArgumentException.ThrowIfNullOrWhiteSpace(message);

        LineNumber = lineNumber;
        Message = message;
    }
}
