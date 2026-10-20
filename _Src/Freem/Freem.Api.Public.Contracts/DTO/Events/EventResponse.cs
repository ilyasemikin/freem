using Freem.Domain.Events;

namespace Freem.Api.Public.Contracts.DTO.Events;

public sealed class EventResponse
{
    public EventId Id { get; }
    public EventType EventType { get; }
    public EntityRef EntityRef { get; }
    public DateTimeOffset OccurredAt { get; }
    public EventPayload Payload { get; }

    public EventResponse(
        EventId id,
        EventType eventType,
        EntityRef entityRef,
        DateTimeOffset occurredAt,
        EventPayload payload)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(eventType);
        ArgumentNullException.ThrowIfNull(entityRef);
        ArgumentNullException.ThrowIfNull(payload);

        Id = id;
        EventType = eventType;
        EntityRef = entityRef;
        OccurredAt = occurredAt;
        Payload = payload;
    }
}
