using Freem.Domain.Events;
using Freem.Domain.Users;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Domain.Storage.Repositories.Events;

public interface IEventRepository
{
    Task<Result<Error>> CreateAsync(
        IEntityEvent<Id, UserId> entity,
        CancellationToken cancellationToken = default);

    Task<Result<IEntityEvent<Id, UserId>, Error>> FindByIdAsync(
        EventId id,
        CancellationToken cancellationToken = default);

    Task<Result<AsyncBatch<IEntityEvent<Id, UserId>>, Error>> FindAsync(
        EventAfterTimeFilter filter,
        CancellationToken cancellationToken = default);
}