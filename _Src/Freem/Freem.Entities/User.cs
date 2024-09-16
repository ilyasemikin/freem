using Freem.Clones;
using Freem.Entities.Abstractions;
using Freem.Entities.Identifiers;

namespace Freem.Entities;

public class User : 
    IEntity<UserIdentifier>, 
    ICloneable<User>
{
    public UserIdentifier Id { get; }
    public string Nickname { get; }

    public User(UserIdentifier id, string nickname)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(nickname);

        Id = id;
        Nickname = nickname;
    }

    public User Clone()
    {
        return new User(Id, Nickname);
    }
}
