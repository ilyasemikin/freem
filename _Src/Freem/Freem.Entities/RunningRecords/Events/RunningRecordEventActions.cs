namespace Freem.Entities.RunningRecords.Events;

public static class RunningRecordEventActions
{
    public const string Started = "started";
    public const string Stopped = "stopped";
    public const string Updated = "updated";
    public const string Removed = "removed";

    public static IReadOnlyList<string> All { get; } = [Started, Stopped, Updated, Removed];
}