using Freem.Entities.Abstractions.Events;
using Freem.Entities.Abstractions.Events.Identifiers;
using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.Storage.Abstractions.Models;
using Freem.Entities.Storage.Abstractions.Models.Filters;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.Storage.Abstractions.Repositories;

public interface IEventsRepository
{
    Task CreateAsync<TEvent>(TEvent entity, CancellationToken cancellationToken = default)
        where TEvent : class, IEntityEvent<IEntityIdentifier, UserIdentifier>;
    
    Task<SearchEntityResult<TEvent>> FindByIdAsync<TEvent>(EventIdentifier id, CancellationToken cancellationToken = default)
        where TEvent : class, IEntityEvent<IEntityIdentifier, UserIdentifier>;

    Task<SearchEntitiesAsyncResult<IEntityEvent<IEntityIdentifier, UserIdentifier>>> FindAfterAsync(
        EventsAfterTimeFilter filter,
        CancellationToken cancellationToken = default);
}