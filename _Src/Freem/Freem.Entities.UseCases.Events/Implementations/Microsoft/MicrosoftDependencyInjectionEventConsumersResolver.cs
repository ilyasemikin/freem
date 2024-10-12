using Freem.Entities.Abstractions.Events;
using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.UseCases.Events.Abstractions;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.UseCases.Events.Implementations.Microsoft;

internal class MicrosoftDependencyInjectionEventConsumersResolver : IEventConsumersResolver
{
    private readonly EventConsumerTypesCollection _collection;
    private readonly IServiceProvider _provider;
    
    public MicrosoftDependencyInjectionEventConsumersResolver(EventConsumerTypesCollection collection, IServiceProvider provider)
    {
        _collection = collection;
        _provider = provider;
    }

    public IEnumerable<IEventConsumer<IEntityEvent<IEntityIdentifier, UserIdentifier>>> Resolve(
        IEntityEvent<IEntityIdentifier, UserIdentifier> @event)
    {
        var type = @event.GetType();
        var consumersTypes = _collection.Get(type);

        foreach (var consumerType in consumersTypes)
        {
            if (_provider.GetService(consumerType) is IEventConsumer<IEntityEvent<IEntityIdentifier, UserIdentifier>> consumer)
                yield return consumer;
        }
    }
}