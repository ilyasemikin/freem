using Freem.Domain;
using Freem.Domain.Events;
using Freem.Domain.Events.Models;
using Freem.Domain.Users;
using Freem.Entities.Storage.PostgreSQL.Database.Entities;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Events.Converters;

internal sealed class DatabaseEntityToEventEntityConverter
{
    private readonly EntityIdFactory _entityIdFactory;
    private readonly EventsFactory _eventsFactory;

    public DatabaseEntityToEventEntityConverter(
        EntityIdFactory entityIdFactory, 
        EventsFactory eventsFactory)
    {
        ArgumentNullException.ThrowIfNull(entityIdFactory);
        ArgumentNullException.ThrowIfNull(eventsFactory);
        
        _entityIdFactory = entityIdFactory;
        _eventsFactory = eventsFactory;
    }

    public IEntityEvent<Id, UserId> Convert(EventEntity entity)
    {
        var id = EventId.Parse(entity.Id, null);
        var entityId = _entityIdFactory.Create(entity.EntityName, entity.EntityId);
        var userId = UserId.Parse(entity.UserId, null);
        var action = EventAction.Parse(entity.Action, null);
        return _eventsFactory.Create(id, entityId, userId, action);
    }
}