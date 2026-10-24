using Freem.Api.Public.Contracts.V1.Events.Dtos;

namespace Freem.Api.Public.Contracts.V1.Events;

/// <summary>Response containing a page of events.</summary>
public sealed class EventListResponse
{
    /// <summary>Total number of events matching the query, regardless of paging.</summary>
    public int TotalCount { get; }

    /// <summary>The requested page of events.</summary>
    public IReadOnlyList<EventDto> Events { get; }

    public EventListResponse(int totalCount, IReadOnlyList<EventDto> events)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(totalCount);
        ArgumentNullException.ThrowIfNull(events);

        TotalCount = totalCount;
        Events = events;
    }
}
