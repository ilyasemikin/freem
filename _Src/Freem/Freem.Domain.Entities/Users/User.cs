using Freem.Domain.Events;
using Freem.Domain.Users;
using Freem.Domain.Users.Events.Registered;
using Freem.Domain.Users.Events.SettingsChanged;
using Freem.Domain.Users.Models;

namespace Freem.Domain.Entities.Users;

public sealed class User : 
    IEntity<UserId>
{
    public UserId Id { get; }
    public Nickname Nickname { get; }

    public UserSettings Settings { get; set; }
    
    public User(UserId id, Nickname nickname)
    {
        ArgumentNullException.ThrowIfNull(nickname);

        Id = id;
        Nickname = nickname;
        Settings = UserSettings.Default;
    }

    public UserRegisteredEvent BuildRegisteredEvent(EventId eventId)
    {
        return new UserRegisteredEvent(eventId, Id);
    }

    public UserSettingsChanged BuildSettingsChangedEvent(EventId eventId)
    {
        return new UserSettingsChanged(eventId, Id);
    }
}
