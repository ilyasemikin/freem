using System.Collections.Frozen;
using Freem.Entities.Abstractions.Events.Models;
using Freem.Entities.Activities.Events.Created;
using Freem.Entities.Activities.Events.Removed;
using Freem.Entities.Activities.Events.Updated;
using Freem.Entities.Records.Events.Created;
using Freem.Entities.Records.Events.Removed;
using Freem.Entities.Records.Events.Updated;

namespace Freem.Entities.Records.Events;

public static class RecordEventActions
{
    public const string Created = "created";
    public const string Updated = "updated";
    public const string Removed = "removed";

    public static IReadOnlyList<string> All { get; } = [Created, Updated, Removed];
}