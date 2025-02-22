using Freem.Entities.Abstractions.Events;
using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.Events.Consumer.Abstractions;
using Freem.Entities.Identifiers;

namespace Freem.Entities.Events.Consumer.Implementations;

public sealed class EventConsumersExecutor
{
    private readonly EventConsumerDescriptorsCollection _collection;
    private readonly IEventConsumerRunner _runner;

    public EventConsumersExecutor(
        EventConsumerDescriptorsCollection collection, 
        IEventConsumerRunner runner)
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(runner);
        
        _collection = collection;
        _runner = runner;
        _runner = runner;
    }

    public async Task ExecuteAsync(
        IEntityEvent<IEntityIdentifier, UserIdentifier> @event, 
        CancellationToken cancellationToken = default)
    {
        var eventType = @event.GetType();
        if (!_collection.TryGet(eventType, out var descriptors))
            return;

        foreach (var descriptor in descriptors)
            await _runner.RunAsync(@event, descriptor, cancellationToken);
    }
}