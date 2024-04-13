using Freem.Entities.Constants;

namespace Freem.Entities;

public class Tag
{
    public const int MaxNameLength = LengthLimits.TagMaxNameLength;

    private string _name = string.Empty;

    public string Id { get; }
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

    public Tag(string id, string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);

        Id = id;
        Name = name;
    }
}
