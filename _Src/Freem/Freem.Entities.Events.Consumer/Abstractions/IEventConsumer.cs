using Freem.Entities.Abstractions.Events;
using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.Identifiers;

namespace Freem.Entities.Events.Consumer.Abstractions;

public interface IEventConsumer<in TEvent>
    where TEvent : IEntityEvent<IEntityIdentifier, UserIdentifier>
{
    Task ExecuteAsync(TEvent @event, CancellationToken cancellationToken = default);
}