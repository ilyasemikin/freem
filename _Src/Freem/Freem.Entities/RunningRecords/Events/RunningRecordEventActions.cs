using System.Collections.Frozen;
using Freem.Entities.Abstractions.Events.Models;
using Freem.Entities.Records.Events.Created;
using Freem.Entities.Records.Events.Removed;
using Freem.Entities.Records.Events.Updated;
using Freem.Entities.RunningRecords.Events.Started;
using Freem.Entities.RunningRecords.Events.Stopped;

namespace Freem.Entities.RunningRecords.Events;

public static class RunningRecordEventActions
{
    public const string Started = "started";
    public const string Stopped = "stopped";

    public static IReadOnlyList<string> All { get; } = [Started, Stopped];
}