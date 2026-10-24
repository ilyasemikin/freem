using Freem.Domain.Events;

namespace Freem.Domain.Activities.Events;

public sealed class ActivityUnarchivedEvent : EntityEvent
{
    public ActivityUnarchivedEvent(EventId id) : base(id)
    {
    }

    public override EventType Type => EventTypes.Activities.Unarchived;

    public override EntityRef EntityRef => new(EntityType.Activity, ActivityId.ToString());

    public required ActivityId ActivityId { get; init; }
}
