using Freem.Entities.Abstractions.Events;
using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.Events.Consumer.Implementations;
using Freem.Entities.Identifiers;

namespace Freem.Entities.Events.Consumer.Abstractions;

public interface IEventConsumerRunner
{
    Task RunAsync(
        IEntityEvent<IEntityIdentifier, UserIdentifier> @event,
        EventConsumerDescriptorsCollection.EventConsumerDescriptor descriptor, 
        CancellationToken cancellationToken = default);
}