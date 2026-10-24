using Freem.Domain.Events;

namespace Freem.Api.Public.Contracts.V1.Events.Dtos;

/// <summary>An entry of the event log: something that happened to one of the user's entities.</summary>
public sealed class EventDto
{
    /// <summary>Event id.</summary>
    public EventId Id { get; }
    /// <summary>What happened, e.g. creation or removal of an entity.</summary>
    public EventType EventType { get; }
    /// <summary>The entity the event is about.</summary>
    public EntityRef EntityRef { get; }
    /// <summary>When the event happened.</summary>
    public DateTimeOffset OccurredAt { get; }
    /// <summary>Snapshot of the entity data related to the event.</summary>
    public EventPayload Payload { get; }

    public EventDto(
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
