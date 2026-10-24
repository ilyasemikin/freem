using Freem.Domain.Events;

namespace Freem.Api.Public.Contracts.V1.Events;

/// <summary>Paging and filter query for listing events.</summary>
public sealed class EventListRequest
{
    /// <summary>Offset used when the query omits it.</summary>
    public const int DefaultOffset = 0;
    /// <summary>Limit used when the query omits it.</summary>
    public const int DefaultLimit = 100;

    /// <summary>Zero-based number of items to skip.</summary>
    public int Offset { get; }
    /// <summary>Maximum number of items to return. Must be positive.</summary>
    public int Limit { get; }

    /// <summary>Return only events about entities of this type.</summary>
    public EntityType? EntityType { get; }
    /// <summary>Return only events about the entity with this id.</summary>
    public string? EntityRef { get; }

    /// <summary>Return only events that occurred at or after this moment.</summary>
    public DateTimeOffset? From { get; }
    /// <summary>Return only events that occurred before or at this moment.</summary>
    public DateTimeOffset? To { get; }

    public EventListRequest(
        int offset,
        int limit,
        EntityType? entityType = null,
        string? entityRef = null,
        DateTimeOffset? from = null,
        DateTimeOffset? to = null)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(offset);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(limit);

        Offset = offset;
        Limit = limit;
        EntityType = entityType;
        EntityRef = entityRef;
        From = from;
        To = to;
    }
}
