using Freem.Entities.Abstractions.Events;
using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.Events.Consumer.Abstractions;
using Freem.Entities.Events.Consumer.Implementations;
using Freem.Entities.Users.Identifiers;
using Microsoft.Extensions.DependencyInjection;

namespace Freem.Entities.Events.Consumer.DependencyInjection.Microsoft.Implementations;

public sealed class EventConsumerRunner : IEventConsumerRunner
{
    private readonly IServiceProvider _services;

    public EventConsumerRunner(IServiceProvider services)
    {
        ArgumentNullException.ThrowIfNull(services);
        
        _services = services;
    }

    public async Task RunAsync(
        IEntityEvent<IEntityIdentifier, UserIdentifier> @event, 
        EventConsumerDescriptorsCollection.EventConsumerDescriptor descriptor, 
        CancellationToken cancellationToken = default)
    {
        await using var scope = _services.CreateAsyncScope();

        var consumer = scope.ServiceProvider.GetRequiredService(descriptor.ConsumerType);
        await descriptor.CallExecuteAsync(consumer, @event, cancellationToken);
    }
}