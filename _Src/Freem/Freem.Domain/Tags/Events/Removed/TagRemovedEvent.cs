using Freem.Domain.Events;
using Freem.Domain.Users;

namespace Freem.Domain.Tags.Events.Removed;

public sealed class TagRemovedEvent : EntityEvent<TagId, UserId>
{
    public TagRemovedEvent(EventId id, TagId entityId, UserId userId) 
        : base(id, entityId, userId, TagEventActions.Removed)
    {
    }
}