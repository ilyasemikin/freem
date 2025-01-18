using Freem.Entities.Abstractions.Events;
using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.Bus.Events.Abstractions;
using Freem.Entities.Bus.Events.Implementations;
using Freem.Entities.Users.Identifiers;
using Microsoft.Extensions.DependencyInjection;

namespace Freem.Entities.Bus.Events.DependencyInjection.Microsoft;

public sealed class EventConsumersBuilder
{
    private readonly IList<Type> _consumers;
    private readonly IServiceCollection _services;
    
    internal EventConsumersBuilder(IServiceCollection services)
    {
        _consumers = new List<Type>();
        
        _services = services;
    }

    public EventConsumersBuilder WithConsumer<TConsumer>(ServiceLifetime lifetime = ServiceLifetime.Singleton)
        where TConsumer : IEventConsumer<IEntityEvent<IEntityIdentifier, UserIdentifier>>
    {
        var descriptor = new ServiceDescriptor(typeof(TConsumer), typeof(TConsumer), lifetime);
        _services.Add(descriptor);

        _consumers.Add(typeof(TConsumer));

        return this;
    }

    internal EventConsumerTypesCollection Create()
    {
        return new EventConsumerTypesCollection(_consumers);
    }
}