using System.Collections.Frozen;
using Freem.Entities.Abstractions.Events.Models;
using Freem.Entities.Tags.Events.Created;
using Freem.Entities.Tags.Events.Removed;
using Freem.Entities.Tags.Events.Updated;

namespace Freem.Entities.Tags.Events;

public static class TagEventActions
{
    public const string Created = "created";
    public const string Updated = "updated";
    public const string Removed = "removed";

    public static IReadOnlyList<string> All { get; } = [Created, Updated, Removed];
}