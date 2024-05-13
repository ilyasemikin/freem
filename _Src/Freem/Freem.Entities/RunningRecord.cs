using Freem.Entities.Collections;
using Freem.Entities.Constants;

namespace Freem.Entities;

public class RunningRecord
{
    public const int MaxNameLength = LengthLimits.RecordMaxNameLength;
    public const int MaxDescriptionLength = LengthLimits.RecordMaxDescriptionLength;

    private string? _name;
    private string? _description;

    public string UserId { get; }
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

    public DateTimeOffset StartAt { get; }

    public RunningRecord(
        string userId,
        RelatedCategoriesCollection categories,
        RelatedTagsCollection tags,
        DateTimeOffset startAt)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userId);
        ArgumentNullException.ThrowIfNull(categories);
        ArgumentNullException.ThrowIfNull(tags);

        UserId = userId;
        Categories = categories;
        Tags = tags;

        StartAt = startAt;
    }
}
