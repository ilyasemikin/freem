using Freem.Entities.Abstractions;
using Freem.Entities.Identifiers;
using Freem.Entities.Relations.Collections;
using Freem.Time.Models;

namespace Freem.Entities;

public class Record : IEntity<RecordIdentifier>
{
    public const int MaxNameLength = 128;
    public const int MaxDescriptionLength = 1024;

    private string? _name;
    private string? _description;

    public RecordIdentifier Id { get; }
    public UserIdentifier UserId { get; }
    public RelatedCategoriesCollection Categories { get; }
    public RelatedTagsCollection Tags { get; }

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

    public DateTimePeriod Period { get; set; }

    public Record(
        RecordIdentifier id,
        UserIdentifier userId,
        RelatedCategoriesCollection categories,
        RelatedTagsCollection tags,
        DateTimePeriod period)
    {
        ArgumentNullException.ThrowIfNull(categories);
        ArgumentNullException.ThrowIfNull(tags);
        ArgumentNullException.ThrowIfNull(period);

        Id = id;
        UserId = userId;
        
        Categories = categories;
        Tags = tags;
        
        Period = period;
    }
}
