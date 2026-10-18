using Freem.Entities.Abstractions.Events;
using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.Events.Producer.Abstractions;
using Freem.Entities.Users.Identifiers;

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
