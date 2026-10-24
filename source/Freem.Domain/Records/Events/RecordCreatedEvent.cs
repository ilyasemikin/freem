using Freem.Domain.Activities;
using Freem.Domain.Events;
using Freem.Domain.Records.Models;
using Freem.Domain.Tags;

namespace Freem.Domain.Records.Events;

public sealed class RecordCreatedEvent : EntityEvent
{
    public RecordCreatedEvent(EventId id) : base(id)
    {
    }

    public override EventType Type => EventTypes.Records.Created;

    public override EntityRef EntityRef => new(EntityType.Record, RecordId.ToString());

    public required RecordId RecordId { get; init; }
    public RecordName? Name { get; init; }
    public RecordDescription? Description { get; init; }
    public required IReadOnlyList<RecordPeriodSnapshot> Periods { get; init; }
    public required IReadOnlyList<ActivityId> ActivityIds { get; init; }
    public required IReadOnlyList<TagId> TagIds { get; init; }
}
