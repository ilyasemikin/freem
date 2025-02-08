using Freem.Entities.Abstractions.Events;
using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.Events.Consumption.Implementations;
using Freem.Entities.Events.Production.Abstractions;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.Events.Local.Implementations;

public sealed class LocalEventPublisher : IEventPublisher
{
    private readonly EventConsumersExecutor _consumer;

    public LocalEventPublisher(EventConsumersExecutor consumer)
    {
        ArgumentNullException.ThrowIfNull(consumer);
        
        _consumer = consumer;
    }

    public Task PublishAsync(
        IEntityEvent<IEntityIdentifier, UserIdentifier> @event, 
        CancellationToken cancellationToken = default)
    {
        _ = Task.Run(async () => await _consumer.ExecuteAsync(@event, cancellationToken), cancellationToken);

        return Task.CompletedTask;
    }
}