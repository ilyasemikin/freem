using Freem.Domain.Events;

namespace Freem.Api.Public.Contracts.DTO.Events;

public sealed class ListEventRequest
{
    public const int DefaultOffset = 0;
    public const int DefaultLimit = 100;

    public int Offset { get; }
    public int Limit { get; }

    public EntityType? EntityType { get; }
    public string? EntityRef { get; }

    public DateTimeOffset? From { get; }
    public DateTimeOffset? To { get; }

    public ListEventRequest(
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
