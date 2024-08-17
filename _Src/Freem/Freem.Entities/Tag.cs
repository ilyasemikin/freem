using Freem.Entities.Abstractions;
using Freem.Entities.Identifiers;

namespace Freem.Entities;

public class Tag : IEntity<TagIdentifier>
{
    public const int MaxNameLength = 128;

    private string _name = string.Empty;

    public TagIdentifier Id { get; }
    public UserIdentifier UserId { get; }

    public string Name
    {
        get => _name;
        set
        {
            ArgumentException.ThrowIfNullOrEmpty(nameof(value));

            if (value.Length > MaxNameLength)
                throw new ArgumentException($"Length cannot be more than {MaxNameLength}", nameof(value));

            _name = value;
        }
    }

    public Tag(
        TagIdentifier id, 
        UserIdentifier userId, 
        string name)
    {
        Id = id;
        UserId = userId;
        Name = name;
    }
}
