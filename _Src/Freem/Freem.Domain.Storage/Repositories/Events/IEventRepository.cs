using Freem.Domain.Events;
using Freem.Domain.Events.Identifiers;
using Freem.Domain.Identifiers;
using Freem.Domain.Storage.Models;
using Freem.Domain.Users.Identifiers;

namespace Freem.Domain.Storage.Repositories.Events;

public interface IEventRepository
{
    Task CreateAsync(
        IEntityEvent<IEntityIdentifier, UserIdentifier> entity,
        CancellationToken cancellationToken = default);

    Task<SearchEntityResult<IEntityEvent<IEntityIdentifier, UserIdentifier>>> FindByIdAsync(
        EventIdentifier id,
        CancellationToken cancellationToken = default);

    Task<SearchEntitiesAsyncResult<IEntityEvent<IEntityIdentifier, UserIdentifier>>> FindAsync(
        EventAfterTimeFilter filter,
        CancellationToken cancellationToken = default);
}