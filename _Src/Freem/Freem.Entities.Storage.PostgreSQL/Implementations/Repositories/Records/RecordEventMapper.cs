using Freem.Entities.Events;
using Freem.Entities.Identifiers;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Events;
using Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Events;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Records;

internal static class RecordEventMapper
{
    public static RecordEventEntity MapToDatabaseEntity(this RecordEvent @event)
    {
        var action = @event.Action.MapToDatabaseModel();
        return new RecordEventEntity
        {
            Id = @event.EventId.Value,
            UserId = @event.UserId.Value,
            RecordId = @event.RecordId.Value,
            Action = action,
            CreatedAt = @event.CreatedAt
        };
    }
    
    public static RecordEvent MapToDomainEntity(this RecordEventEntity entity)
    {
        var id = new EventIdentifier(entity.Id);
        var userId = new UserIdentifier(entity.UserId);
        var recordId = new RecordIdentifier(entity.RecordId);
        var action = entity.Action.MapToDomainModel();
        return new RecordEvent(id, userId, recordId, action, entity.CreatedAt);
    }
}