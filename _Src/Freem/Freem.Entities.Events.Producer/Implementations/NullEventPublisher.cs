using Freem.Domain;
using Freem.Domain.Events;
using Freem.Domain.Users;
using Freem.Entities.Events.Producer.Abstractions;

namespace Freem.Entities.Events.Producer.Implementations;

public sealed class NullEventPublisher : IEventPublisher
{
    public Task PublishAsync(
        IEntityEvent<Id, UserId> @event,
        CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
