using Freem.Entities.Abstractions;
using Freem.Entities.Abstractions.Relations.Collection;
using Freem.Entities.Identifiers;
using Freem.Entities.Relations.Collections;
using Freem.Enums.Exceptions;

namespace Freem.Entities;

public class Category : IEntity<CategoryIdentifier>, IEntityRelation<Tag, TagIdentifier>
{
    public const int MaxNameLength = 128;

    private string _name = string.Empty;
    private CategoryStatus _status;

    public CategoryIdentifier Id { get; }
    public UserIdentifier UserId { get; }
    public RelatedTagsCollection Tags { get; }

    IReadOnlyRelatedEntitiesCollection<Tag, TagIdentifier> IEntityRelation<Tag, TagIdentifier>.RelatedEntities => Tags;

    public string Name
    {
        get => _name;
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            if (value.Length > MaxNameLength)
                throw new ArgumentException($"Length cannot be more than {MaxNameLength}", nameof(value));

            _name = value;
        }
    }

    public CategoryStatus Status
    {
        get => _status;
        set
        {
            InvalidEnumValueException<CategoryStatus>.ThrowIfValueInvalid(value);

            _status = value;
        }
    }

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
