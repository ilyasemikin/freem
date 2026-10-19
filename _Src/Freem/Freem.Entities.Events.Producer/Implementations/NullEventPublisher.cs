using Freem.Domain.Events;
using Freem.Domain.Identifiers;
using Freem.Domain.Users.Identifiers;
using Freem.Entities.Events.Producer.Abstractions;

namespace Freem.Entities.Events.Producer.Implementations;

public sealed class NullEventPublisher : IEventPublisher
{
    public Task PublishAsync(
        IEntityEvent<IEntityIdentifier, UserIdentifier> @event,
        CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
