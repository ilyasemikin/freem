using Freem.Domain.Events;

namespace Freem.Domain.Users.Events.SettingsChanged;

public sealed class UserSettingsChanged : EntityEvent<UserId, UserId>
{
    public UserSettingsChanged(EventId id, UserId userId) 
        : base(id, userId, userId, UserEventActions.SettingsChanged)
    {
    }
}