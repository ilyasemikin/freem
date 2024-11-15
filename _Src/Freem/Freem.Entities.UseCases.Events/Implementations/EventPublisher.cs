using Freem.Entities.Abstractions.Events;
using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.Storage.Abstractions.Repositories;
using Freem.Entities.UseCases.Events.Abstractions;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.UseCases.Events.Implementations;

internal sealed class EventPublisher : IEventPublisher
{
    private readonly IEventsRepository _repository;
    private readonly IEventConsumersResolver _eventConsumersResolver;

    public EventPublisher(
        IEventsRepository repository, 
        IEventConsumersResolver eventConsumersResolver)
    {
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
}