using Freem.Entities.Abstractions.Events;
using Freem.Entities.Abstractions.Events.Identifiers;
using Freem.Entities.Abstractions.Events.Models;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.Users.Events.SignedIn;

public sealed class UserSignedInEvent : EntityEvent<UserIdentifier, UserIdentifier>
{
    public UserSignedInEvent(EventIdentifier id, UserIdentifier entityId) 
        : base(id, entityId, entityId, UserEventActions.SignedIn)
    {
    }
}