using Freem.Entities.Abstractions;
using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.Identifiers;
using Freem.Enums.Exceptions;

namespace Freem.Entities.Events;

public sealed class CategoryEvent : IEventEntity<EventIdentifier, UserIdentifier>
{
    public EventIdentifier EventId { get; }
    public UserIdentifier UserId { get; }
    public CategoryIdentifier CategoryId { get; }
    public EventAction Action { get; }
    public DateTimeOffset CreatedAt { get; }

    IEntityIdentifier IEventEntity<EventIdentifier, UserIdentifier>.ChangedEntityId => CategoryId;

    public CategoryEvent(
        EventIdentifier eventId, 
        UserIdentifier userId,
        CategoryIdentifier categoryId, 
        EventAction action,
        DateTimeOffset createdAt)
    {
        ArgumentNullException.ThrowIfNull(eventId);
        ArgumentNullException.ThrowIfNull(userId);
        ArgumentNullException.ThrowIfNull(categoryId);
        InvalidEnumValueException<EventAction>.ThrowIfValueInvalid(action);

        EventId = eventId;
        UserId = userId;
        CategoryId = categoryId;
        Action = action;
        CreatedAt = createdAt.UtcDateTime;
    }
}