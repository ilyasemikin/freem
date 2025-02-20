using Freem.Converters.Abstractions;
using Freem.Entities.Abstractions.Events;
using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.Storage.PostgreSQL.Database.Entities;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Events.Converters;

internal class EventEntityToDatabaseEntityConverter 
    : IConverter<IEntityEvent<IEntityIdentifier, UserIdentifier>, EventEntity>
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