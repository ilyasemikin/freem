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
    public RelatedEntitiesCollection<Tag> Tags { get; }

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

    public DateTimeOffset CreatedAt { get; }

    public Category(
        string id,
        string name,
        string userId,
        RelatedEntitiesCollection<Tag> relatedTags,
        DateTimeOffset createdAt,
        CategoryStatus status = CategoryStatus.Active)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        ArgumentException.ThrowIfNullOrWhiteSpace(userId);
        ArgumentNullException.ThrowIfNull(relatedTags);

        Id = id;
        UserId = userId;
        Name = name;
        Tags = relatedTags;
        Status = status;
        CreatedAt = createdAt.UtcDateTime;
    }

    public static RelatedEntitiesCollection<Tag> CreateRelatedTags(
        IEnumerable<Tag>? entities = null, 
        IEnumerable<string>? identifiers = null)
    {
        return new RelatedEntitiesCollection<Tag>(entities ?? [], identifiers ?? []);
    }
}
