using Freem.Domain.Users;

namespace Freem.Domain.Events;

public sealed class EventEnvelope
{
    public UserId UserId { get; }
    public DateTimeOffset OccurredAt { get; }
    public EntityEvent Event { get; }

    public EventEnvelope(UserId userId, DateTimeOffset occurredAt, EntityEvent @event)
    {
        ArgumentNullException.ThrowIfNull(userId);
        ArgumentNullException.ThrowIfNull(@event);

        UserId = userId;
        OccurredAt = occurredAt;
        Event = @event;
    }
}
