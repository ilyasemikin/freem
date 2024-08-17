using Freem.Entities.Abstractions;
using Freem.Entities.Identifiers;
using Freem.Entities.Relations.Collections;

namespace Freem.Entities;

public class Category : IEntity<CategoryIdentifier>
{
    public const int MaxNameLength = 128;

    private string _name = string.Empty;

    public CategoryIdentifier Id { get; }
    public UserIdentifier UserId { get; }
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
        CategoryIdentifier id,
        UserIdentifier userId,
        RelatedTagsCollection relatedTags,
        CategoryStatus status)
    {
        ArgumentNullException.ThrowIfNull(relatedTags);

        Id = id;
        UserId = userId;
        Tags = relatedTags;
        Status = status;
    }
}
