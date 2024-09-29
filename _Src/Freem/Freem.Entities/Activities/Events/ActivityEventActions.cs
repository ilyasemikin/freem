using System.Collections.Frozen;
using Freem.Entities.Abstractions.Events.Models;
using Freem.Entities.Activities.Events.Created;
using Freem.Entities.Activities.Events.Removed;
using Freem.Entities.Activities.Events.Updated;

namespace Freem.Entities.Activities.Events;

public static class ActivityEventActions
{
    public const string Created = "created";
    public const string Updated = "updated";
    public const string Removed = "removed";

    public static IReadOnlyList<string> All { get; } = [Created, Updated, Removed];
}