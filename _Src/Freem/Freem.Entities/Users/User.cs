using Freem.Entities.Abstractions;
using Freem.Entities.Abstractions.Events.Identifiers;
using Freem.Entities.Users.Events.Registered;
using Freem.Entities.Users.Events.SettingsChanged;
using Freem.Entities.Users.Identifiers;
using Freem.Entities.Users.Models;

namespace Freem.Entities.Users;

public sealed class User : 
    IEntity<UserIdentifier>
{
    public const string EntityName = "user";
    
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
