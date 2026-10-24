using Freem.Domain.Entities.Events;
using Freem.Domain.Events;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Domain.Storage.Repositories.Events;

public interface IEventRepository
{
    Task<Result<Error>> CreateAsync(
        EventEnvelope envelope,
        CancellationToken cancellationToken = default);
    Task<Result<Error>> CreateManyAsync(
        IReadOnlyList<EventEnvelope> envelopes,
        CancellationToken cancellationToken = default);

    Task<Result<Event, Error>> FindByIdAsync(
        EventId id,
        CancellationToken cancellationToken = default);

    Task<Result<AsyncBatch<Event>, Error>> FindAsync(
        EventByUserFilter filter,
        CancellationToken cancellationToken = default);
}
