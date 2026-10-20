using Freem.Domain;
using Freem.Domain.Events;
using Freem.Domain.Users;

namespace Freem.Domain.Events.Producer.Abstractions;

public interface IEventPublisher
{
    Task PublishAsync(
        IEntityEvent<Id, UserId> @event,
        CancellationToken cancellationToken = default);
}
