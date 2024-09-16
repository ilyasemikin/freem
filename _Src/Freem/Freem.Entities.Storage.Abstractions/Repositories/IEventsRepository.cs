using Freem.Entities.Abstractions;
using Freem.Entities.Identifiers;
using Freem.Entities.Storage.Abstractions.Base;
using Freem.Entities.Storage.Abstractions.Models;
using Freem.Entities.Storage.Abstractions.Models.Filters;

namespace Freem.Entities.Storage.Abstractions.Repositories;

public interface IEventsRepository : 
    ISearchByIdRepository<IEventEntity<EventIdentifier, UserIdentifier>, EventIdentifier>
{
    Task<SearchEntitiesAsyncResult<IEventEntity<EventIdentifier, UserIdentifier>>> FindAfterAsync(
        EventsAfterTimeFilter filter,
        CancellationToken cancellationToken = default);
}