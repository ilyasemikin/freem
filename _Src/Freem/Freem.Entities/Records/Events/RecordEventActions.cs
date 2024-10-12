namespace Freem.Entities.Records.Events;

public static class RecordEventActions
{
    public const string Created = "created";
    public const string Updated = "updated";
    public const string Removed = "removed";

    public static IReadOnlyList<string> All { get; } = [Created, Updated, Removed];
}