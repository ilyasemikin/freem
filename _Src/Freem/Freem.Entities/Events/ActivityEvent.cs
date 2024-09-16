using Freem.Entities.Abstractions;
using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.Identifiers;
using Freem.Enums.Exceptions;

namespace Freem.Entities.Events;

public sealed class ActivityEvent : IEventEntity<EventIdentifier, UserIdentifier>
{
    public EventIdentifier EventId { get; }
    public UserIdentifier UserId { get; }
    public ActivityIdentifier ActivityId { get; }
    public EventAction Action { get; }
    public DateTimeOffset CreatedAt { get; }

    IEntityIdentifier IEventEntity<EventIdentifier, UserIdentifier>.ChangedEntityId => ActivityId;

    public ActivityEvent(
        EventIdentifier eventId, 
        UserIdentifier userId,
        ActivityIdentifier activityId, 
        EventAction action,
        DateTimeOffset createdAt)
    {
        ArgumentNullException.ThrowIfNull(eventId);
        ArgumentNullException.ThrowIfNull(userId);
        ArgumentNullException.ThrowIfNull(activityId);
        InvalidEnumValueException<EventAction>.ThrowIfValueInvalid(action);

        EventId = eventId;
        UserId = userId;
        ActivityId = activityId;
        Action = action;
        CreatedAt = createdAt.UtcDateTime;
    }
}