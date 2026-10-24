using Freem.Domain.Activities.Models;
using Freem.Domain.Events;
using Freem.Domain.Tags;

namespace Freem.Domain.Activities.Events;

public sealed class ActivityCreatedEvent : EntityEvent
{
    public ActivityCreatedEvent(EventId id) : base(id)
    {
    }

    public override EventType Type => EventTypes.Activities.Created;

    public override EntityRef EntityRef => new(EntityType.Activity, ActivityId.ToString());

    public required ActivityId ActivityId { get; init; }
    public required ActivityName Name { get; init; }
    public ActivityColor? Color { get; init; }
    public required ActivityStatus Status { get; init; }
    public required IReadOnlyList<TagId> TagIds { get; init; }
}
