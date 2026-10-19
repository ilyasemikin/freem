using Freem.Domain;
using Freem.Domain.Events;
using Freem.Domain.Identifiers;
using Freem.Domain.Users.Identifiers;
using Freem.Entities.Storage.PostgreSQL.Database.Entities;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Events.Converters;

internal class EventEntityToDatabaseEntityConverter
{
    private readonly EntityIdentifierNameProvider _nameProvider;

    public EventEntityToDatabaseEntityConverter(EntityIdentifierNameProvider nameProvider)
    {
        ArgumentNullException.ThrowIfNull(nameProvider);
        
        _nameProvider = nameProvider;
    }

    public EventEntity Convert(IEntityEvent<IEntityIdentifier, UserIdentifier> entity)
    {
        return new EventEntity
        {
            Id = entity.Id,
            EntityId = entity.EntityId.ToString(),
            EntityName = _nameProvider.Get(entity.EntityId),
            UserId = entity.UserEntityId,
            Action = entity.Action
        };
    }
}