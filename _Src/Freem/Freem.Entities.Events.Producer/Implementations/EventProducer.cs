using Freem.Domain.Events;
using Freem.Domain.Events.Identifiers;
using Freem.Domain.Identifiers;
using Freem.Domain.Storage.Repositories.Events;
using Freem.Domain.Users.Identifiers;
using Freem.Entities.Events.Producer.Abstractions;
using Freem.Identifiers.Abstractions.Generators;

namespace Freem.Entities.Events.Producer.Implementations;

public sealed class EventProducer
{
    private readonly IIdentifierGenerator<EventIdentifier> _identifierGenerator;
    private readonly IEventRepository _repository;
    private readonly IEventPublisher _publisher;
    
    public delegate IEntityEvent<IEntityIdentifier, UserIdentifier> EventFactory(EventIdentifier eventId);

    public EventProducer(
        IIdentifierGenerator<EventIdentifier> identifierGenerator, 
        IEventRepository repository, 
        IEventPublisher publisher)
    {
        ArgumentNullException.ThrowIfNull(identifierGenerator);
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(publisher);
        
        _identifierGenerator = identifierGenerator;
        _repository = repository;
        _publisher = publisher;
    }

    public async Task PublishAsync(EventFactory factory, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(factory);
        
        var eventId = _identifierGenerator.Generate();
        var @event = factory(eventId);

        await _repository.CreateAsync(@event, cancellationToken);
        await _publisher.PublishAsync(@event, cancellationToken);
    }
}