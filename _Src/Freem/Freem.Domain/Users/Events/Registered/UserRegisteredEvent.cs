using Freem.Domain.Events;
using Freem.Domain.Events.Identifiers;
using Freem.Domain.Users.Identifiers;

namespace Freem.Domain.Users.Events.Registered;

public sealed class UserRegisteredEvent : EntityEvent<UserIdentifier, UserIdentifier>
{
    public UserRegisteredEvent(EventIdentifier id, UserIdentifier entityId)
        : base(id, entityId, entityId, UserEventActions.Registered)
    {
    }
}