using Freem.Entities.Abstractions.Events;
using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.Bus.Events.Abstractions;

public interface IEventConsumer<TEvent>
    where TEvent : IEntityEvent<IEntityIdentifier, UserIdentifier>
{
    Task ExecuteAsync(TEvent @event, CancellationToken cancellationToken = default);
}