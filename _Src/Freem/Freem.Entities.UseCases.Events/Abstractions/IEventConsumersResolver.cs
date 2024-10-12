using Freem.Entities.Abstractions.Events;
using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.UseCases.Events.Abstractions;

internal interface IEventConsumersResolver
{
    IEnumerable<IEventConsumer<IEntityEvent<IEntityIdentifier, UserIdentifier>>> Resolve(
        IEntityEvent<IEntityIdentifier, UserIdentifier> @event);
}