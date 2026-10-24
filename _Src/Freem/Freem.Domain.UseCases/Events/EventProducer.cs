using Freem.Domain.Events;
using Freem.Domain.Storage.Repositories.Events;
using Freem.Domain.Users;
using Utilities.Clock.Abstractions;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Domain.UseCases.Events;

internal sealed class EventProducer
{
    private readonly IEventRepository _repository;
    private readonly IClock _clock;

    public delegate EntityEvent EventFactory(EventId eventId);

    public EventProducer(IEventRepository repository, IClock clock)
    {
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(clock);

        _repository = repository;
        _clock = clock;
    }

    public async Task<Result<Error>> PublishAsync(
        UserId userId, EventFactory factory,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(userId);
        ArgumentNullException.ThrowIfNull(factory);

        var eventId = EventId.Generate();
        var @event = factory(eventId);

        var occurredAt = _clock.Get();
        var envelope = new EventEnvelope(userId, occurredAt, @event);

        return await _repository.CreateAsync(envelope, cancellationToken);
    }

    public async Task<Result<Error>> PublishManyAsync(
        UserId userId, IReadOnlyList<EventFactory> factories,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(userId);
        ArgumentNullException.ThrowIfNull(factories);

        var occurredAt = _clock.Get();
        var envelopes = new List<EventEnvelope>(factories.Count);
        foreach (var factory in factories)
        {
            var eventId = EventId.Generate();
            var @event = factory(eventId);
            var envelope = new EventEnvelope(userId, occurredAt, @event);
            envelopes.Add(envelope);
        }

        return await _repository.CreateManyAsync(envelopes, cancellationToken);
    }
}
