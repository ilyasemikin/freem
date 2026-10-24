using Freem.Domain.Events;

namespace Freem.Domain.Activities.Events;

public sealed class ActivityArchivedEvent : EntityEvent
{
    public ActivityArchivedEvent(EventId id) : base(id)
    {
    }

    public override EventType Type => EventTypes.Activities.Archived;

    public override EntityRef EntityRef => new(EntityType.Activity, ActivityId.ToString());

    public required ActivityId ActivityId { get; init; }
}
