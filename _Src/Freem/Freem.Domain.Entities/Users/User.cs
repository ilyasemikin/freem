using Freem.Domain.Events.Identifiers;
using Freem.Domain.Users.Events.Registered;
using Freem.Domain.Users.Events.SettingsChanged;
using Freem.Domain.Users.Identifiers;
using Freem.Domain.Users.Models;

namespace Freem.Domain.Entities.Users;

public sealed class User : 
    IEntity<UserIdentifier>
{
    public UserIdentifier Id { get; }
    public Nickname Nickname { get; }

    public UserSettings Settings { get; set; }
    
    public User(UserIdentifier id, Nickname nickname)
    {
        ArgumentNullException.ThrowIfNull(nickname);

        Id = id;
        Nickname = nickname;
        Settings = UserSettings.Default;
    }

    public UserRegisteredEvent BuildRegisteredEvent(EventIdentifier eventId)
    {
        return new UserRegisteredEvent(eventId, Id);
    }

    public UserSettingsChanged BuildSettingsChangedEvent(EventIdentifier eventId)
    {
        return new UserSettingsChanged(eventId, Id);
    }
}
