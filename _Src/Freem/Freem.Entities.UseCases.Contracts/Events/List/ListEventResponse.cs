using Freem.Domain;
using Freem.Domain.Events;
using Freem.Domain.Users;
using Freem.Entities.UseCases.Contracts.Filter;

namespace Freem.Entities.UseCases.Contracts.Events.List;

public sealed class ListEventResponse
{
    public IReadOnlyList<IEntityEvent<Id, UserId>> Events { get; }
    public TotalCount TotalCount { get; }

    public ListEventResponse(IReadOnlyList<IEntityEvent<Id, UserId>> events, TotalCount totalCount)
    {
        ArgumentNullException.ThrowIfNull(events);
        ArgumentNullException.ThrowIfNull(totalCount);

        Events = events;
        TotalCount = totalCount;
    }
}