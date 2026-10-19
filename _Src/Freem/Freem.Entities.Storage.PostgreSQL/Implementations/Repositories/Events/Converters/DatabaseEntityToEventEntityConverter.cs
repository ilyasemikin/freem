using Freem.Domain;
using Freem.Domain.Events;
using Freem.Domain.Events.Identifiers;
using Freem.Domain.Identifiers;
using Freem.Domain.Users.Identifiers;
using Freem.Entities.Storage.PostgreSQL.Database.Entities;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Events.Converters;

internal sealed class DatabaseEntityToEventEntityConverter
{
    private readonly EntityIdentifierFactory _entityIdentifierFactory;
    private readonly EventsFactory _eventsFactory;

    public DatabaseEntityToEventEntityConverter(
        EntityIdentifierFactory entityIdentifierFactory, 
        EventsFactory eventsFactory)
    {
        ArgumentNullException.ThrowIfNull(entityIdentifierFactory);
        ArgumentNullException.ThrowIfNull(eventsFactory);
        
        _entityIdentifierFactory = entityIdentifierFactory;
        _eventsFactory = eventsFactory;
    }

    public IEntityEvent<IEntityIdentifier, UserIdentifier> Convert(EventEntity entity)
    {
        var id = new EventIdentifier(entity.Id);
        var entityId = _entityIdentifierFactory.Create(entity.EntityName, entity.EntityId);
        var userId = new UserIdentifier(entity.UserId);
        return _eventsFactory.Create(id, entityId, userId, entity.Action);
    }
}