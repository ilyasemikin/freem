namespace Freem.Entities.Activities.Events;

public static class ActivityEventActions
{
    public const string Created = "created";
    public const string Updated = "updated";
    public const string Removed = "removed";
    public const string Archived = "archived";
    public const string Unarchived = "unarchived";

    public static IReadOnlyList<string> All { get; } = [Created, Updated, Removed, Archived, Unarchived];
}