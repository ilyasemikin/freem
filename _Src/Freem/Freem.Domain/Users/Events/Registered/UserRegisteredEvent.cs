using Freem.Domain.Events;

namespace Freem.Domain.Users.Events.Registered;

public sealed class UserRegisteredEvent : EntityEvent<UserId, UserId>
{
    public UserRegisteredEvent(EventId id, UserId entityId)
        : base(id, entityId, entityId, UserEventActions.Registered)
    {
    }
}