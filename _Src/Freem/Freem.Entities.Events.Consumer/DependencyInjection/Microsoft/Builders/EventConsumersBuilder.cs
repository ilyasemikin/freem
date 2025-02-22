using Freem.Entities.Abstractions.Events;
using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.Events.Consumer.Abstractions;
using Freem.Entities.Events.Consumer.Implementations;
using Freem.Entities.Identifiers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Freem.Entities.Events.Consumer.DependencyInjection.Microsoft.Builders;

public sealed class EventConsumersBuilder
{
    private readonly IServiceCollection _services;
    private readonly EventConsumerDescriptorsCollection.Builder _collectionBuilder;
    
    public EventConsumersBuilder(IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);
        
        _services = services;
        _collectionBuilder = new EventConsumerDescriptorsCollection.Builder();
    }

    public EventConsumersBuilder AddConsumer<TEvent, TEventConsumer>(ServiceLifetime lifetime = ServiceLifetime.Transient)
        where TEventConsumer : IEventConsumer<TEvent>
        where TEvent : IEntityEvent<IEntityIdentifier, UserIdentifier>
    {
        if (!_collectionBuilder.TryAdd<TEvent, TEventConsumer>())
            throw new InvalidOperationException($"EventConsumer \"{typeof(TEvent)}\" has already been registered.");
        
        var consumerType = typeof(TEventConsumer);
        var descriptor = new ServiceDescriptor(consumerType, consumerType, lifetime);
        _services.TryAdd(descriptor);
        
        return this;
    }

    public EventConsumerDescriptorsCollection BuildTypesCollection()
    {
        return _collectionBuilder.Build();
    }
}