namespace Freem.Entities;

public class User
{
    public string Id { get; }

    public User(string id)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);

        Id = id;
    }
}
