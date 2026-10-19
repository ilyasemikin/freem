using Freem.Domain.Events;
using Freem.Domain.Users;

namespace Freem.Domain.Tags.Events.Created;

public sealed class TagCreatedEvent : EntityEvent<TagId, UserId>
{
    public TagCreatedEvent(EventId id, TagId entityId, UserId userId) 
        : base(id, entityId, userId, TagEventActions.Created)
    {
    }
}