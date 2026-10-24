using Freem.Domain.Events;

namespace Freem.Domain.RunningRecords.Events;

public sealed class RunningRecordStoppedEvent : EntityEvent
{
    public RunningRecordStoppedEvent(EventId id) : base(id)
    {
    }

    public override EventType Type => EventTypes.RunningRecords.Stopped;

    public override EntityRef EntityRef => new(EntityType.RunningRecord, RunningRecordId.ToString());

    public required RunningRecordId RunningRecordId { get; init; }
}
