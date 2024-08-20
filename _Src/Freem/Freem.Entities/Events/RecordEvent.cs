using Freem.Entities.Abstractions;
using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.Identifiers;
using Freem.Enums.Exceptions;

namespace Freem.Entities.Events;

public sealed class RecordEvent : IEventEntity<EventIdentifier, UserIdentifier>
{
    public EventIdentifier EventId { get; }
    public UserIdentifier UserId { get; }
    public RecordIdentifier RecordId { get; }
    public EventAction Action { get; }
    public DateTimeOffset CreatedAt { get; }

    IEntityIdentifier IEventEntity<EventIdentifier, UserIdentifier>.ChangedEntityId => RecordId;

    public RecordEvent(
        EventIdentifier eventId, 
        UserIdentifier userId,
        RecordIdentifier recordId, 
        EventAction action,
        DateTimeOffset createdAt)
    {
        ArgumentNullException.ThrowIfNull(eventId);
        ArgumentNullException.ThrowIfNull(recordId);
        ArgumentNullException.ThrowIfNull(userId);
        InvalidEnumValueException<EventAction>.ThrowIfValueInvalid(action);
        
        EventId = eventId;
        UserId = userId;
        RecordId = recordId;
        Action = action;
        CreatedAt = createdAt.UtcDateTime;
    }
}