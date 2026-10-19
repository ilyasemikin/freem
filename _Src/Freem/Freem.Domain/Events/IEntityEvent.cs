using Freem.Domain.Events.Identifiers;
using Freem.Domain.Events.Models;
using Freem.Domain.Identifiers;

namespace Freem.Domain.Events;

public interface IEntityEvent<out TEntityIdentifier, out TUserEntityIdentifier> 
    : IEntity<EventIdentifier>
    where TEntityIdentifier : IEntityIdentifier
    where TUserEntityIdentifier : IEntityIdentifier
{
    TEntityIdentifier EntityId { get; }
    TUserEntityIdentifier UserEntityId { get; }

    EventAction Action { get; }
}