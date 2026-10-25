namespace Freem.DataTransfer.ZaferErtas.TimeTracker.Models;

public sealed class TimeTrackerFile
{
    public IReadOnlyList<TimeTrackerRow> Rows { get; }
    public IReadOnlyList<TimeTrackerLineIssue> Issues { get; }
    public int RowsInFile { get; }

    public TimeTrackerFile(
        IReadOnlyList<TimeTrackerRow> rows,
        IReadOnlyList<TimeTrackerLineIssue> issues,
        int rowsInFile)
    {
        ArgumentNullException.ThrowIfNull(rows);
        ArgumentNullException.ThrowIfNull(issues);
        ArgumentOutOfRangeException.ThrowIfLessThan(rowsInFile, rows.Count);

        Rows = rows;
        Issues = issues;
        RowsInFile = rowsInFile;
    }
}
