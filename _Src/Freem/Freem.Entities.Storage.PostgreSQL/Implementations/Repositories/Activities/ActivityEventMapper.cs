using Freem.Entities.Events;
using Freem.Entities.Identifiers;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Events;
using Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Events;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Activities;

internal static class ActivityEventMapper
{
    public static ActivityEventEntity MapToDatabaseEntity(this ActivityEvent @event)
    {
        var action = @event.Action.MapToDatabaseModel();
        return new ActivityEventEntity
        {
            Id = @event.EventId.Value,
            ActivityId = @event.ActivityId.Value,
            UserId = @event.UserId.Value,
            Action = action,
            CreatedAt = @event.CreatedAt
        };
    }
    
    public static ActivityEvent MapToDomainEntity(ActivityEventEntity entity)
    {
        var id = new EventIdentifier(entity.Id);
        var userId = new UserIdentifier(entity.UserId);
        var activityId = new ActivityIdentifier(entity.ActivityId);
        var action = entity.Action.MapToDomainModel();
        return new ActivityEvent(id, userId, activityId, action, entity.CreatedAt);
    }
}