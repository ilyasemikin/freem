using Freem.Domain.Events;

namespace Freem.Domain.RunningRecords.Events;

public sealed class RunningRecordRemovedEvent : EntityEvent
{
    public RunningRecordRemovedEvent(EventId id) : base(id)
    {
    }

    public override EventType Type => EventTypes.RunningRecords.Removed;

    public override EntityRef EntityRef => new(EntityType.RunningRecord, RunningRecordId.ToString());

    public required RunningRecordId RunningRecordId { get; init; }
}
