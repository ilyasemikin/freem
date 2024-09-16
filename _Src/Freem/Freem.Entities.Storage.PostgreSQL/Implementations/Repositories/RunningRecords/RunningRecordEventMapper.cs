using Freem.Entities.Events;
using Freem.Entities.Identifiers;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Events;
using Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Events;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.RunningRecords;

internal static class RunningRecordEventMapper
{
    public static RunningRecordEventEntity MapToDatabaseEntity(this RunningRecordEvent @event)
    {
        var action = @event.Action.MapToDatabaseModel();
        return new RunningRecordEventEntity
        {
            Id = @event.EventId.Value,
            UserId = @event.UserId.Value,
            CreatedAt = @event.CreatedAt,
            Action = action
        };
    }
    
    public static RunningRecordEvent MapToDomainEntity(RunningRecordEventEntity entity)
    {
        var id = new EventIdentifier(entity.Id);
        var userId = new UserIdentifier(entity.UserId);
        var action = entity.Action.MapToDomainModel();
        return new RunningRecordEvent(id, userId, action, entity.CreatedAt);
    }
}