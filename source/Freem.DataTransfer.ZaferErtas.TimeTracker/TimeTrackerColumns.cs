namespace Freem.DataTransfer.ZaferErtas.TimeTracker;

public static class TimeTrackerColumns
{
    public const int Id = 0;
    public const int StartDate = 2;
    public const int StartTime = 3;
    public const int EndDate = 4;
    public const int EndTime = 5;
    public const int ActivityId = 8;
    public const int ActivityName = 9;
    public const int Notes = 10;

    public const int LeadingCount = 10;
    public const int TrailingCount = 2;

    public static IReadOnlyList<string> Header { get; } =
    [
        "ID",
        "Time item ID",
        "Start Date",
        "Start Time",
        "End Date",
        "End Time",
        "Duration",
        "Duration in milliseconds",
        "Activity ID",
        "Activity Name",
        "Notes",
        "Category ID",
        "Category Name"
    ];
}
