using Freem.Domain.Entities.Events;
using Freem.Domain.UseCases.Contracts.Filter;

namespace Freem.Domain.UseCases.Contracts.Events.List;

public sealed class ListEventResponse
{
    public IReadOnlyList<Event> Events { get; }
    public TotalCount TotalCount { get; }

    public ListEventResponse(IReadOnlyList<Event> events, TotalCount totalCount)
    {
        ArgumentNullException.ThrowIfNull(events);
        ArgumentNullException.ThrowIfNull(totalCount);

        Events = events;
        TotalCount = totalCount;
    }
}
