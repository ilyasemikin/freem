using Freem.Entities.Abstractions.Events.Identifiers;
using Freem.Entities.Abstractions.Events.Models;
using Freem.Entities.Abstractions.Identifiers;

namespace Freem.Entities.Abstractions.Events;

public interface IEntityEvent<out TEntityIdentifier, out TUserEntityIdentifier> 
    : IEntity<EventIdentifier>
    where TEntityIdentifier : IEntityIdentifier
    where TUserEntityIdentifier : IEntityIdentifier
{
    TEntityIdentifier EntityId { get; }
    TUserEntityIdentifier UserEntityId { get; }

    EventAction Action { get; }
}