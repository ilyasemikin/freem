using Freem.DateTimePeriods;
using Freem.Entities.Abstractions;
using Freem.Entities.Collections;
using Freem.Entities.Constants;

namespace Freem.Entities;

public class Record : IEntity
{
    public const int MaxNameLength = LengthLimits.RecordMaxNameLength;
    public const int MaxDescriptionLength = LengthLimits.RecordMaxDescriptionLength;

    public const int RelatedCategoriesMinCount = 1;

    private string? _name;
    private string? _description;

    public string Id { get; }
    public string UserId { get; }
    public RelatedEntitiesCollection<Category> Categories { get; }
    public RelatedEntitiesCollection<Tag> Tags { get; }

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

    public DateTimePeriod Period { get; }

    public Record(
        string id,
        string userId,
        RelatedEntitiesCollection<Category> categories,
        RelatedEntitiesCollection<Tag> tags,
        DateTimePeriod timePeriod)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        ArgumentException.ThrowIfNullOrWhiteSpace(userId);
        ArgumentNullException.ThrowIfNull(categories);
        ArgumentNullException.ThrowIfNull(tags);
        ArgumentNullException.ThrowIfNull(timePeriod);

        Id = id;
        UserId = userId;
        
        Categories = categories;
        Tags = tags;
        
        Period = timePeriod;
    }

    public static RelatedEntitiesCollection<Category> CreateRelatedCategories(IEnumerable<Category> entities)
    {
        return CreateRelatedCategories(entities, []);
    }

    public static RelatedEntitiesCollection<Category> CreateRelatedCategories(IEnumerable<string> identifiers)
    {
        return CreateRelatedCategories([], identifiers);
    }

    public static RelatedEntitiesCollection<Category> CreateRelatedCategories(
        IEnumerable<Category> entities, 
        IEnumerable<string> identifiers)
    {
        return new RelatedEntitiesCollection<Category>(entities, identifiers, RelatedCategoriesMinCount);
    }

    public static RelatedEntitiesCollection<Tag> CreateRelatedTags(
        IEnumerable<Tag>? entities = null, 
        IEnumerable<string>? identifiers = null)
    {
        return new RelatedEntitiesCollection<Tag>(entities ?? [], identifiers ?? []);
    }
}
