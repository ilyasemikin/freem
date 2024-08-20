using Freem.Entities.Events;
using Freem.Entities.Identifiers;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Events;
using Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Events;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Categories;

internal static class CategoryEventMapper
{
    public static CategoryEventEntity MapToDatabaseEntity(this CategoryEvent @event)
    {
        var action = @event.Action.MapToDatabaseModel();
        return new CategoryEventEntity
        {
            Id = @event.EventId.Value,
            CategoryId = @event.CategoryId.Value,
            UserId = @event.UserId.Value,
            Action = action,
            CreatedAt = @event.CreatedAt
        };
    }
    
    public static CategoryEvent MapToDomainEntity(CategoryEventEntity entity)
    {
        var id = new EventIdentifier(entity.Id);
        var userId = new UserIdentifier(entity.UserId);
        var categoryId = new CategoryIdentifier(entity.CategoryId);
        var action = entity.Action.MapToDomainModel();
        return new CategoryEvent(id, userId, categoryId, action, entity.CreatedAt);
    }
}