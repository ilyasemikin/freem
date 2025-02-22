using Freem.Entities.Abstractions.Events;
using Freem.Entities.Abstractions.Events.Identifiers;
using Freem.Entities.Identifiers;

namespace Freem.Entities.Users.Events.Registered;

public sealed class UserRegisteredEvent : EntityEvent<UserIdentifier, UserIdentifier>
{
    public UserRegisteredEvent(EventIdentifier id, UserIdentifier entityId)
        : base(id, entityId, entityId, UserEventActions.Registered)
    {
    }
}