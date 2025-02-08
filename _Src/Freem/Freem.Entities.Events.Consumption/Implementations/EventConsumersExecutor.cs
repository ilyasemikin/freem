using Freem.Entities.Abstractions.Events;
using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.Events.Consumption.Abstractions;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.Events.Consumption.Implementations;

public sealed class EventConsumersExecutor
{
    private readonly EventConsumerDescriptorsCollection _collection;
    private readonly IEventConsumerResolver _resolver;

    public EventConsumersExecutor(
        EventConsumerDescriptorsCollection collection, 
        IEventConsumerResolver resolver)
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(resolver);
        
        _collection = collection;
        _resolver = resolver;
    }

    public async Task ExecuteAsync(
        IEntityEvent<IEntityIdentifier, UserIdentifier> @event, 
        CancellationToken cancellationToken = default)
    {
        var eventType = @event.GetType();
        if (!_collection.TryGet(eventType, out var descriptors))
            return;

        foreach (var descriptor in descriptors)
        {
            var consumer = _resolver.Resolve(descriptor);
            
            await descriptor.CallExecuteAsync(consumer, @event, cancellationToken);
        }
    }
}