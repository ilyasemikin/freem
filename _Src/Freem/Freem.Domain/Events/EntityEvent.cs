namespace Freem.Domain.Events;

public abstract class EntityEvent : IEvent
{
    public EventId Id { get; }

    public abstract EventType Type { get; }
    public abstract EntityRef EntityRef { get; }

    protected EntityEvent(EventId id)
    {
        ArgumentNullException.ThrowIfNull(id);

        Id = id;
    }
}
