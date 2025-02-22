using Freem.Entities.Abstractions.Events;
using Freem.Entities.Abstractions.Events.Identifiers;
using Freem.Entities.Identifiers;

namespace Freem.Entities.Users.Events.SettingsChanged;

public sealed class UserSettingsChanged : EntityEvent<UserIdentifier, UserIdentifier>
{
    public UserSettingsChanged(EventIdentifier id, UserIdentifier userId) 
        : base(id, userId, userId, UserEventActions.SettingsChanged)
    {
    }
}