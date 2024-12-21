using Freem.Clones;
using Freem.Entities.Abstractions;
using Freem.Entities.Abstractions.Events.Identifiers;
using Freem.Entities.Users.Events.PasswordCredentialsAdded;
using Freem.Entities.Users.Events.Registered;
using Freem.Entities.Users.Identifiers;
using Freem.Entities.Users.Models;

namespace Freem.Entities.Users;

public sealed class User : 
    IEntity<UserIdentifier>, 
    ICloneable<User>
{
    public UserIdentifier Id { get; }
    public Nickname Nickname { get; }

    public UserPasswordCredentials? PasswordCredentials { get; set; }
    
    public User(UserIdentifier id, Nickname nickname)
    {
        ArgumentNullException.ThrowIfNull(nickname);

        Id = id;
        Nickname = nickname;
    }

    public User Clone()
    {
        return new User(Id, Nickname);
    }

    public UserRegisteredEvent BuildRegisteredEvent(EventIdentifier eventId)
    {
        return new UserRegisteredEvent(eventId, Id);
    }

    public UserPasswordCredentialsChangedEvent BuildPasswordCredentialsChangedEvent(EventIdentifier eventId)
    {
        if (PasswordCredentials is null)
            throw new InvalidOperationException("PasswordCredentials not added");

        return new UserPasswordCredentialsChangedEvent(eventId, Id);
    }
}
