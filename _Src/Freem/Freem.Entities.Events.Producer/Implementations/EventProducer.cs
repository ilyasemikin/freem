using Freem.Domain;
using Freem.Domain.Events;
using Freem.Domain.Storage.Repositories.Events;
using Freem.Domain.Users;
using Freem.Entities.Events.Producer.Abstractions;

namespace Freem.Entities.Events.Producer.Implementations;

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

    public async Task PublishAsync(EventFactory factory, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(factory);
        
        var eventId = EventId.Generate();
        var @event = factory(eventId);

        await _repository.CreateAsync(@event, cancellationToken);
        await _publisher.PublishAsync(@event, cancellationToken);
    }
}