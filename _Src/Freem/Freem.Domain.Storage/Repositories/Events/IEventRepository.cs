using Freem.Domain.Events;
using Freem.Domain.Storage.Models;
using Freem.Domain.Users;

namespace Freem.Domain.Storage.Repositories.Events;

public interface IEventRepository
{
    Task CreateAsync(
        IEntityEvent<Id, UserId> entity,
        CancellationToken cancellationToken = default);

    Task<SearchEntityResult<IEntityEvent<Id, UserId>>> FindByIdAsync(
        EventId id,
        CancellationToken cancellationToken = default);

    Task<SearchEntitiesAsyncResult<IEntityEvent<Id, UserId>>> FindAsync(
        EventAfterTimeFilter filter,
        CancellationToken cancellationToken = default);
}