using Freem.Domain;
using Freem.Domain.Events;
using Freem.Domain.Users;
using Freem.Entities.Storage.PostgreSQL.Database.Entities;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Events.Converters;

internal class EventEntityToDatabaseEntityConverter
{
    private readonly EntityIdNameProvider _nameProvider;

    public EventEntityToDatabaseEntityConverter(EntityIdNameProvider nameProvider)
    {
        ArgumentNullException.ThrowIfNull(nameProvider);
        
        _nameProvider = nameProvider;
    }

    public EventEntity Convert(IEntityEvent<Id, UserId> entity)
    {
        return new EventEntity
        {
            Id = entity.Id.ToString(),
            EntityId = entity.EntityId.ToString(),
            EntityName = _nameProvider.Get(entity.EntityId),
            UserId = entity.UserEntityId.ToString(),
            Action = entity.Action.ToString()
        };
    }
}