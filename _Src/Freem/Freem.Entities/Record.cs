using Freem.Collections.Identifiers;
using Freem.Entities.Constants;
using Freem.Entities.Helpers;

namespace Freem.Entities;

public class Record
{
    public const int MaxNameLength = LimitConstants.RecordMaxNameLength;
    public const int MaxDescriptionLength = LimitConstants.RecordMaxDescriptionLength;

    private string? _name;
    private string? _description;

    public string Id { get; }
    public string UserId { get; }
    public IdentifiersCollection CategoryIds { get; }
    public IdentifiersCollection TagsIds { get; }

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

    public TimePeriod TimePeriod { get; }

    public Record(
        string id,
        string userId,
        IEnumerable<string> categoryIds,
        IEnumerable<string>? tagsIds,
        TimePeriod timePeriod)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        ArgumentException.ThrowIfNullOrWhiteSpace(userId);
        ArgumentNullException.ThrowIfNull(categoryIds);
        ArgumentNullException.ThrowIfNull(timePeriod);

        Id = id;
        UserId = userId;
        
        CategoryIds = new IdentifiersCollection(categoryIds, IdentifiersCheckerStrategies.CategoryIdsCheckerStrategy);
        TagsIds = new IdentifiersCollection(tagsIds, IdentifiersCheckerStrategies.TagIdsCheckerStrategy);
        
        TimePeriod = timePeriod;
    }
}
