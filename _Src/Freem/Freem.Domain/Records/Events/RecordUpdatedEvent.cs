using Freem.Domain.Activities;
using Freem.Domain.Events;
using Freem.Domain.Records.Models;
using Freem.Domain.Tags;
using Freem.Time.Models;

namespace Freem.Domain.Records.Events;

public sealed class RecordUpdatedEvent : EntityEvent
{
    public RecordUpdatedEvent(EventId id) : base(id)
    {
    }

    public override EventType Type => EventTypes.Records.Updated;

    public override EntityRef EntityRef => new(EntityType.Record, RecordId.ToString());

    public required RecordId RecordId { get; init; }
    public RecordName? Name { get; init; }
    public RecordDescription? Description { get; init; }
    public required DateTimePeriod Period { get; init; }
    public required IReadOnlyList<ActivityId> ActivityIds { get; init; }
    public required IReadOnlyList<TagId> TagIds { get; init; }
}
