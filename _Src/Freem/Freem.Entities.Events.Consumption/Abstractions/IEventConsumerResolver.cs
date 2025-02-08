using Freem.Entities.Events.Consumption.Implementations;

namespace Freem.Entities.Events.Consumption.Abstractions;

public interface IEventConsumerResolver
{
    object Resolve(EventConsumerDescriptorsCollection.EventConsumerDescriptor descriptor);
}