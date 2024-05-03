using Freem.Collections.Identifiers;
using Freem.Entities.Constants;
using Freem.Entities.Helpers;

namespace Freem.Entities;

public class Category
{
    public const int MaxNameLength = LengthLimits.CategoryMaxNameLength;

    private string _name = string.Empty;

    public string Id { get; }
    public string UserId { get; }
    public IdentifiersCollection TagIds { get; }

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
        IEnumerable<string>? tagIds,
        DateTimeOffset createdAt,
        CategoryStatus status = CategoryStatus.Active)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        ArgumentException.ThrowIfNullOrWhiteSpace(userId);

        Id = id;
        UserId = userId;
        Name = name;
        TagIds = new IdentifiersCollection(tagIds, IdentifiersCheckerStrategies.TagIdsCheckerStrategy);
        Status = status;
        CreatedAt = createdAt.UtcDateTime;
    }
}
