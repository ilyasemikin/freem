using Freem.Domain.Events.Models;

namespace Freem.Domain.Events;

public interface IEntityEvent<out TEntityId, out TUserEntityId> 
    : IEntity<EventId>
    where TEntityId : Id
    where TUserEntityId : Id
{
    TEntityId EntityId { get; }
    TUserEntityId UserEntityId { get; }

    EventAction Action { get; }
}