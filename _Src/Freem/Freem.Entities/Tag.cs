using Freem.Entities.Abstractions;
using Freem.Entities.Constants;

namespace Freem.Entities;

public class Tag : IEntity
{
    public const int MaxNameLength = LengthLimits.TagMaxNameLength;

    private string _name = string.Empty;

    public string Id { get; }
    public string UserId { get; }

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

    public Tag(string id, string userId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        ArgumentException.ThrowIfNullOrEmpty(userId);

        Id = id;
        UserId = userId;
    }
}
