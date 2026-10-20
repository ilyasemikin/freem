using Freem.Domain;
using Freem.Domain.Events;
using Freem.Domain.Storage.Repositories.Events;
using Freem.Domain.Users;
using Freem.Domain.Events.Producer.Abstractions;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Domain.Events.Producer.Implementations;

public sealed class EventProducer
{
    private readonly IEventRepository _repository;
    private readonly IEventPublisher _publisher;
    
    public delegate IEntityEvent<Id, UserId> EventFactory(EventId eventId);

    public EventProducer(
        IEventRepository repository, 
        IEventPublisher publisher)
    {
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(publisher);
        
        _repository = repository;
        _publisher = publisher;
    }

    public async Task<Result<Error>> PublishAsync(EventFactory factory, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(factory);
        
        var eventId = EventId.Generate();
        var @event = factory(eventId);

        var result = await _repository.CreateAsync(@event, cancellationToken);
        if (!result.Success)
            return result;

        await _publisher.PublishAsync(@event, cancellationToken);
        return result;
    }
}