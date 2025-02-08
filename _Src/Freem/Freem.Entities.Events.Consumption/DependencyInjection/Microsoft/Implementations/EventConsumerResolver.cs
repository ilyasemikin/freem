using Freem.Entities.Events.Consumption.Abstractions;
using Freem.Entities.Events.Consumption.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace Freem.Entities.Events.Consumption.DependencyInjection.Microsoft.Implementations;

public sealed class EventConsumerResolver : IEventConsumerResolver
{
    private readonly IServiceProvider _provider;

    public EventConsumerResolver(IServiceProvider provider)
    {
        ArgumentNullException.ThrowIfNull(provider);
        
        _provider = provider;
    }

    public object Resolve(EventConsumerDescriptorsCollection.EventConsumerDescriptor descriptor)
    {
        return _provider.GetRequiredService(descriptor.ConsumerType);
    }
}