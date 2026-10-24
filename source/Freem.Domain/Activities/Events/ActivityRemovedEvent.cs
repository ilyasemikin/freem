using Freem.Domain.Events;

namespace Freem.Domain.Activities.Events;

public sealed class ActivityRemovedEvent : EntityEvent
{
    public ActivityRemovedEvent(EventId id) : base(id)
    {
    }

    public override EventType Type => EventTypes.Activities.Removed;

    public override EntityRef EntityRef => new(EntityType.Activity, ActivityId.ToString());

    public required ActivityId ActivityId { get; init; }
}
