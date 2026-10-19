using Freem.Domain.Events.Models;

namespace Freem.Domain.Events;

public abstract class EntityEvent<TEntityId, TUserEntityId> 
    : IEntityEvent<TEntityId, TUserEntityId>
    where TEntityId : Id
    where TUserEntityId : Id
{
    public EventId Id { get; }
    public TEntityId EntityId { get; }
    public TUserEntityId UserEntityId { get; }
    
    public EventAction Action { get; }

    protected EntityEvent(
        EventId id, 
        TEntityId entityId, 
        TUserEntityId userId, 
        string action)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(entityId);
        ArgumentNullException.ThrowIfNull(userId);

        Id = id;
        EntityId = entityId;
        UserEntityId = userId;
        Action = EventAction.Parse(action, null);
    }
}