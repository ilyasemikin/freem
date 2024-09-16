using Freem.Entities.Events;
using Freem.Entities.Identifiers;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Events;
using Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Events;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Tags;

internal static class TagEventMapper
{
    public static TagEventEntity MapToDatabaseEntity(this TagEvent @event)
    {
        var action = @event.Action.MapToDatabaseModel();
        return new TagEventEntity
        {
            Id = @event.EventId.Value,
            UserId = @event.UserId.Value,
            TagId = @event.TagId.Value,
            CreatedAt = @event.CreatedAt,
            Action = action
        };
    }
    
    public static TagEvent MapToDomainEntity(this TagEventEntity entity)
    {
        var id = new EventIdentifier(entity.Id);
        var userId = new UserIdentifier(entity.UserId);
        var tagId = new TagIdentifier(entity.TagId);
        var action = entity.Action.MapToDomainModel();
        return new TagEvent(id, userId, tagId, action, entity.CreatedAt);
    }
}