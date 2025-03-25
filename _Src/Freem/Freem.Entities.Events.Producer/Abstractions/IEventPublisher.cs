using Freem.Entities.Abstractions.Events;
using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.Events.Producer.Abstractions;

public interface IEventPublisher
{
    Task PublishAsync(
        IEntityEvent<IEntityIdentifier, UserIdentifier> @event,
        CancellationToken cancellationToken = default);
}
