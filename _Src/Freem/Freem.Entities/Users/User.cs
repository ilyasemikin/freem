using Freem.Clones;
using Freem.Entities.Abstractions;
using Freem.Entities.Users.Identifiers;
using Freem.Entities.Users.Models;

namespace Freem.Entities.Users;

public sealed class User : 
    IEntity<UserIdentifier>, 
    ICloneable<User>
{
    public UserIdentifier Id { get; }
    public Nickname Nickname { get; }

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
}
