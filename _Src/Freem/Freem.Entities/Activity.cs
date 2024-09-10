using Freem.Entities.Abstractions;
using Freem.Entities.Abstractions.Relations.Collection;
using Freem.Entities.Identifiers;
using Freem.Entities.Relations.Collections;
using Freem.Enums.Exceptions;

namespace Freem.Entities;

public class Activity : IEntity<ActivityIdentifier>, IEntityRelation<Tag, TagIdentifier>
{
    public const int MaxNameLength = 128;

    private string _name = string.Empty;
    private ActivityStatus _status;

    public ActivityIdentifier Id { get; }
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

    public ActivityStatus Status
    {
        get => _status;
        set
        {
            InvalidEnumValueException<ActivityStatus>.ThrowIfValueInvalid(value);

            _status = value;
        }
    }

    public Activity(
        ActivityIdentifier id,
        UserIdentifier userId,
        RelatedTagsCollection relatedTags,
        ActivityStatus status)
    {
        ArgumentNullException.ThrowIfNull(relatedTags);

        Id = id;
        UserId = userId;
        Tags = relatedTags;
        Status = status;
    }
}
