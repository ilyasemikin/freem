namespace Freem.Domain.Events;

public interface IEvent
{
    EventId Id { get; }
    EventType Type { get; }
}
