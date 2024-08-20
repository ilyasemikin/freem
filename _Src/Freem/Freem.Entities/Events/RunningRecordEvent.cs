using Freem.Entities.Abstractions;
using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.Identifiers;
using Freem.Enums.Exceptions;

namespace Freem.Entities.Events;

public sealed class RunningRecordEvent : IEventEntity<EventIdentifier, UserIdentifier>
{
    public EventIdentifier EventId { get; }
    public UserIdentifier UserId { get; }
    public EventAction Action { get; }
    public DateTimeOffset CreatedAt { get; }

    IEntityIdentifier IEventEntity<EventIdentifier, UserIdentifier>.ChangedEntityId => UserId;

    public RunningRecordEvent(
        EventIdentifier eventId, 
        UserIdentifier userId, 
        EventAction action,
        DateTimeOffset createdAt)
    {
        ArgumentNullException.ThrowIfNull(eventId);
        ArgumentNullException.ThrowIfNull(userId);
        InvalidEnumValueException<EventAction>.ThrowIfValueInvalid(action);

        EventId = eventId;
        UserId = userId;
        Action = action;
        CreatedAt = createdAt.UtcDateTime;
    }
}