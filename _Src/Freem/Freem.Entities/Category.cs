using Freem.Entities.Abstractions;
using Freem.Entities.Collections;
using Freem.Entities.Constants;

namespace Freem.Entities;

public class Category : IEntity
{
    public const int MaxNameLength = LengthLimits.CategoryMaxNameLength;

    private string _name = string.Empty;

    public string Id { get; }
    public string UserId { get; }
    public RelatedTagsCollection Tags { get; }

    public string Name
    {
        get => _name;
        set
        {
            ArgumentException.ThrowIfNullOrEmpty(value);
            if (value.Length > MaxNameLength)
                throw new ArgumentException($"Length cannot be more than {MaxNameLength}", nameof(value));

            _name = value;
        }
    }

    public CategoryStatus Status { get; }

    public Category(
        string id,
        string userId,
        RelatedTagsCollection relatedTags,
        CategoryStatus status)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        ArgumentException.ThrowIfNullOrWhiteSpace(userId);
        ArgumentNullException.ThrowIfNull(relatedTags);

        Id = id;
        UserId = userId;
        Tags = relatedTags;
        Status = status;
    }
}
