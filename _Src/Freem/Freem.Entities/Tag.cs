namespace Freem.Entities;

public class Tag
{
    public string Id { get; }
    public string Name { get; }

    public Tag(string id, string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        Id = id;
        Name = name;
    }
}
