using Freem.Domain.Events;
using Freem.Domain.Users;

namespace Freem.Domain.Entities.Events;

public sealed class Event : IEvent
{
    public EventId Id { get; }

    public required EventType Type { get; init; }
    public required UserId UserId { get; init; }
    public required DateTimeOffset OccurredAt { get; init; }
    public required EntityRef EntityRef { get; init; }
    public required EventPayload Payload { get; init; }

    public Event(EventId id)
    {
        ArgumentNullException.ThrowIfNull(id);

        Id = id;
    }
}
