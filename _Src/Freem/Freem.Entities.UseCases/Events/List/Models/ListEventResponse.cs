using System.Collections;
using Freem.Entities.Abstractions.Events;
using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.UseCases.Models.Filter;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.UseCases.Events.List.Models;

public sealed class ListEventResponse : IEnumerable<IEntityEvent<IEntityIdentifier, UserIdentifier>>
{
    public IReadOnlyList<IEntityEvent<IEntityIdentifier, UserIdentifier>> Events { get; }
    public TotalCount TotalCount { get; }
    
    public ListEventResponse(
        IReadOnlyList<IEntityEvent<IEntityIdentifier, UserIdentifier>> events, 
        TotalCount totalCount)
    {
        ArgumentNullException.ThrowIfNull(events);
        ArgumentNullException.ThrowIfNull(totalCount);

        Events = events;
        TotalCount = totalCount;
    }

    public ListEventResponse(
        IEnumerable<IEntityEvent<IEntityIdentifier, UserIdentifier>> events,
        TotalCount totalCount)
    {
        ArgumentNullException.ThrowIfNull(events);
        ArgumentNullException.ThrowIfNull(totalCount);

        Events = events.ToArray();
        TotalCount = totalCount;
    }

    public IEnumerator<IEntityEvent<IEntityIdentifier, UserIdentifier>> GetEnumerator()
    {
        return Events.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}