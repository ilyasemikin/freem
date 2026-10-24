using Freem.Domain.Events;

namespace Freem.Domain.Records.Events;

public sealed class RecordRemovedEvent : EntityEvent
{
    public RecordRemovedEvent(EventId id) : base(id)
    {
    }

    public override EventType Type => EventTypes.Records.Removed;

    public override EntityRef EntityRef => new(EntityType.Record, RecordId.ToString());

    public required RecordId RecordId { get; init; }
}
