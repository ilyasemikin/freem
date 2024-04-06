using Freem.Collections.Identifiers;
using Freem.Entities.Constants;
using Freem.Entities.Helpers;

namespace Freem.Entities;

public class RunningRecord
{
    public const int MaxNameLength = LimitConstants.RecordMaxNameLength;
    public const int MaxDescriptionLength = LimitConstants.RecordMaxDescriptionLength;

    private string? _name;
    private string? _description;

    public string UserId { get; }
    public IdentifiersCollection CategoryIds { get; }
    public IdentifiersCollection TagIds { get; }

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
        IEnumerable<string> categoryIds,
        DateTimeOffset startAt,
        IEnumerable<string>? tagIds = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userId);
        ArgumentNullException.ThrowIfNull(categoryIds);
        ArgumentNullException.ThrowIfNull(tagIds);

        UserId = userId;
        CategoryIds = new IdentifiersCollection(categoryIds, IdentifiersCheckerStrategies.CategoryIdsCheckerStrategy);
        TagIds = new IdentifiersCollection(tagIds, IdentifiersCheckerStrategies.TagIdsCheckerStrategy);

        StartAt = startAt;
    }
}
