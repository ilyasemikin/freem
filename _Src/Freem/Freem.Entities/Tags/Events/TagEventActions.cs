namespace Freem.Entities.Tags.Events;

public static class TagEventActions
{
    public const string Created = "created";
    public const string Updated = "updated";
    public const string Removed = "removed";

    public static IReadOnlyList<string> All { get; } = [Created, Updated, Removed];
}