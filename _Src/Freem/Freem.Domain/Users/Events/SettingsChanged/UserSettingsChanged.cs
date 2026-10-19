using Freem.Domain.Events;
using Freem.Domain.Events.Identifiers;
using Freem.Domain.Users.Identifiers;

namespace Freem.Domain.Users.Events.SettingsChanged;

public sealed class UserSettingsChanged : EntityEvent<UserIdentifier, UserIdentifier>
{
    public UserSettingsChanged(EventIdentifier id, UserIdentifier userId) 
        : base(id, userId, userId, UserEventActions.SettingsChanged)
    {
    }
}