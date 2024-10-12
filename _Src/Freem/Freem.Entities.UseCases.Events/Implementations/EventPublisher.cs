using Freem.Entities.Abstractions.Events;
using Freem.Entities.Abstractions.Events.Identifiers;
using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.Storage.Abstractions.Repositories;
using Freem.Entities.UseCases.Events.Abstractions;
using Freem.Entities.Users.Identifiers;
using Freem.Identifiers.Abstractions.Generators;

namespace Freem.Entities.UseCases.Events.Implementations;

internal sealed class EventPublisher : IEventPublisher
{
    private readonly IIdentifierGenerator<EventIdentifier> _identifierGenerator;
    private readonly IEventsRepository _repository;
    private readonly IEventConsumersResolver _eventConsumersResolver;

    public EventPublisher(
        IIdentifierGenerator<EventIdentifier> identifierGenerator, 
        IEventsRepository repository, 
        IEventConsumersResolver eventConsumersResolver)
    {
        _identifierGenerator = identifierGenerator;
        _repository = repository;
        _eventConsumersResolver = eventConsumersResolver;
    }

    public async Task PublishAsync(
        IEntityEvent<IEntityIdentifier, UserIdentifier> @event, 
        CancellationToken cancellationToken = default)
    {
        await _repository.CreateAsync(@event, cancellationToken);

        var consumers = _eventConsumersResolver.Resolve(@event);
        foreach (var consumer in consumers)
            _ = Task.Run(async () => await consumer.ExecuteAsync(@event, cancellationToken), cancellationToken);
    }

    public async Task PublishAsync(
        IEventPublisher.EventFactory factory, 
        CancellationToken cancellationToken = default)
    {
        var eventId = _identifierGenerator.Generate();
        var @event = factory(eventId);
        
        await PublishAsync(@event, cancellationToken);
    }
}