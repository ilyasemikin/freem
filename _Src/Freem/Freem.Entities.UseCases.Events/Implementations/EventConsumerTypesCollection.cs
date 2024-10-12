using Freem.Reflection.Extensions;

namespace Freem.Entities.UseCases.Events.Implementations;

internal sealed class EventConsumerTypesCollection
{
    private readonly IReadOnlyDictionary<Type, Type> _consumers;
    
    public EventConsumerTypesCollection(IEnumerable<Type> consumers)
    {
        _consumers = BuildConsumerDictionary(consumers);
    }

    public IEnumerable<Type> Get(Type @event)
    {
        if (_consumers.TryGetValue(@event, out var consumer))
            yield return consumer;
    }

    private static IReadOnlyDictionary<Type, Type> BuildConsumerDictionary(IEnumerable<Type> consumers)
    {
        var result = new Dictionary<Type, Type>();

        foreach (var consumer in consumers)
        {
            var @interface = consumer.GetRequiredInterface("IEventConsumer`1");
            var argument = @interface.GetRequiredGenericArgument(0);
            result.Add(argument, consumer);
        }

        return result;
    }
}