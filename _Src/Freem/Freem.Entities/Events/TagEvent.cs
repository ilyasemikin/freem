using Freem.Entities.Abstractions;
using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.Identifiers;
using Freem.Enums.Exceptions;

namespace Freem.Entities.Events;

public class TagEvent : IEventEntity<EventIdentifier, UserIdentifier>
{
    public EventIdentifier EventId { get; }
    public UserIdentifier UserId { get; }
    public TagIdentifier TagId { get; }
    public EventAction Action { get; }
    public DateTimeOffset CreatedAt { get; }

    IEntityIdentifier IEventEntity<EventIdentifier, UserIdentifier>.ChangedEntityId => TagId;

    public TagEvent(
        EventIdentifier eventId, 
        UserIdentifier userId,
        TagIdentifier tagId, 
        EventAction action,
        DateTimeOffset createdAt)
    {
        ArgumentNullException.ThrowIfNull(eventId);
        ArgumentNullException.ThrowIfNull(userId);
        ArgumentNullException.ThrowIfNull(tagId);
        InvalidEnumValueException<EventAction>.ThrowIfValueInvalid(action);
        
        EventId = eventId;
        UserId = userId;
        TagId = tagId;
        Action = action;
        CreatedAt = createdAt.UtcDateTime;
    }
}