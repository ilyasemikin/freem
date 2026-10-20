using Freem.Domain.Activities;
using Freem.Domain.Events;
using Freem.Domain.Records.Models;
using Freem.Domain.Tags;

namespace Freem.Domain.RunningRecords.Events;

public sealed class RunningRecordUpdatedEvent : EntityEvent
{
    public RunningRecordUpdatedEvent(EventId id) : base(id)
    {
    }

    public override EventType Type => EventTypes.RunningRecords.Updated;

    public override EntityRef EntityRef => new(EntityType.RunningRecord, RunningRecordId.ToString());

    public required RunningRecordId RunningRecordId { get; init; }
    public RecordName? Name { get; init; }
    public RecordDescription? Description { get; init; }
    public required DateTimeOffset StartAt { get; init; }
    public required IReadOnlyList<ActivityId> ActivityIds { get; init; }
    public required IReadOnlyList<TagId> TagIds { get; init; }
}
