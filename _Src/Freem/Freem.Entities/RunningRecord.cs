using Freem.Entities.Abstractions;
using Freem.Entities.Abstractions.Relations.Collection;
using Freem.Entities.Identifiers;
using Freem.Entities.Relations.Collections;

namespace Freem.Entities;

public class RunningRecord : 
    IEntity<UserIdentifier>,
    IEntityRelation<Category, CategoryIdentifier>, 
    IEntityRelation<Tag, TagIdentifier>
{
    public const int MaxNameLength = Record.MaxNameLength;
    public const int MaxDescriptionLength = Record.MaxDescriptionLength;

    private string? _name;
    private string? _description;

    public UserIdentifier Id => UserId;
    public UserIdentifier UserId { get; }
    public RelatedCategoriesCollection Categories { get; }
    public RelatedTagsCollection Tags { get; }
    
    IReadOnlyRelatedEntitiesCollection<Category, CategoryIdentifier> IEntityRelation<Category, CategoryIdentifier>.RelatedEntities => Categories;
    IReadOnlyRelatedEntitiesCollection<Tag, TagIdentifier> IEntityRelation<Tag, TagIdentifier>.RelatedEntities => Tags;

    public string? Name
    {
        get => _name;
        set
        {
            if (value?.Length > MaxNameLength)
                throw new ArgumentException($"Length cannot be more than {MaxNameLength}", nameof(value));

            _name = value;
        }
    }

    public string? Description
    {
        get => _description;
        set
        {
            if (value?.Length > MaxDescriptionLength)
                throw new ArgumentException($"Length cannot be more than {MaxDescriptionLength}", nameof(value));

            _description = value;
        }
    }

    public DateTimeOffset StartAt { get; }

    public RunningRecord(
        UserIdentifier userId,
        RelatedCategoriesCollection categories,
        RelatedTagsCollection tags,
        DateTimeOffset startAt)
    {
        ArgumentNullException.ThrowIfNull(categories);
        ArgumentNullException.ThrowIfNull(tags);

        UserId = userId;
        Categories = categories;
        Tags = tags;

        StartAt = startAt;
    }
}
