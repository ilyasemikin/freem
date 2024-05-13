namespace Freem.Entities;

public class User
{
    public string Id { get; }
    public string Nickname { get; }

    public User(string id, string nickname)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        ArgumentException.ThrowIfNullOrWhiteSpace(nickname);

        Id = id;
        Nickname = nickname;
    }
}
