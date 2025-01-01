using Freem.Entities.Abstractions.Events;
using Freem.Entities.Abstractions.Events.Identifiers;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.Users.Events.PasswordCredentialsChanged;

public sealed class UserPasswordCredentialsChangedEvent : EntityEvent<UserIdentifier, UserIdentifier>
{
    public UserPasswordCredentialsChangedEvent(EventIdentifier id, UserIdentifier entityId)
        : base(id, entityId, entityId, UserEventActions.LoginCredentialsChanged)
    {
    }
}