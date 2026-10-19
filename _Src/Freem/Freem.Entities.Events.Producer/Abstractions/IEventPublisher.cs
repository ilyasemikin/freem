using Freem.Domain.Events;
using Freem.Domain.Identifiers;
using Freem.Domain.Users.Identifiers;

namespace Freem.Entities.Events.Producer.Abstractions;

public interface IEventPublisher
{
    Task PublishAsync(
        IEntityEvent<IEntityIdentifier, UserIdentifier> @event,
        CancellationToken cancellationToken = default);
}
