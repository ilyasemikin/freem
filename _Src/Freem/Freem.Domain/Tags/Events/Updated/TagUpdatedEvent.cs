using Freem.Domain.Events;
using Freem.Domain.Users;

namespace Freem.Domain.Tags.Events.Updated;

public sealed class TagUpdatedEvent : EntityEvent<TagId, UserId>
{
    public TagUpdatedEvent(EventId id, TagId entityId, UserId userId) 
        : base(id, entityId, userId, TagEventActions.Updated)
    {
    }
}