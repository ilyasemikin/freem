using Freem.Entities.Abstractions.Events;
using Freem.Entities.Abstractions.Events.Identifiers;
using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.UseCases.Events.Abstractions;

internal interface IEventProducer
{
    delegate IEntityEvent<IEntityIdentifier, UserIdentifier> EventFactory(EventIdentifier eventId);
    
    Task PublishAsync(
        EventFactory factory, 
        CancellationToken cancellationToken = default);
}