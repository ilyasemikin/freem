using Freem.Domain.Events.Identifiers;
using Freem.Domain.Events.Models;
using Freem.Domain.Identifiers;

namespace Freem.Domain.Events;

public abstract class EntityEvent<TEntityIdentifier, TUserEntityIdentifier> 
    : IEntityEvent<TEntityIdentifier, TUserEntityIdentifier>
    where TEntityIdentifier : IEntityIdentifier
    where TUserEntityIdentifier : IEntityIdentifier
{
    public EventIdentifier Id { get; }
    public TEntityIdentifier EntityId { get; }
    public TUserEntityIdentifier UserEntityId { get; }
    
    public EventAction Action { get; }

    protected EntityEvent(
        EventIdentifier id, 
        TEntityIdentifier entityId, 
        TUserEntityIdentifier userId, 
        EventAction action)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(entityId);
        ArgumentNullException.ThrowIfNull(userId);
        ArgumentNullException.ThrowIfNull(action);

        Id = id;
        EntityId = entityId;
        UserEntityId = userId;
        Action = action;
    }
}