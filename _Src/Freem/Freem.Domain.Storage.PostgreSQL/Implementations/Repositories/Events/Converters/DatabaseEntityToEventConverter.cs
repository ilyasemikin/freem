using System.Text.Json;
using Freem.Domain.Entities.Events;
using Freem.Domain.Events;
using Freem.Domain.Storage.PostgreSQL.Database.Entities;
using Freem.Domain.Users;
using Utilities.Naming.Abstractions;

namespace Freem.Domain.Storage.PostgreSQL.Implementations.Repositories.Events.Converters;

internal sealed class DatabaseEntityToEventConverter
{
    public Event Convert(EventEntity entity)
    {
        var id = EventId.Parse(entity.Id, null);
        var userId = UserId.Parse(entity.UserId, null);
        var type = EventType.Parse(entity.EventType, null);
        var entityType = EntityTypeNames.Parse(entity.EntityType);
        var entityRef = new EntityRef(entityType, entity.EntityRef);

        var values = JsonSerializer.Deserialize<Dictionary<string, string?>>(entity.Payload)
                     ?? throw new FormatException($"Invalid payload of event [{entity.Id}]");
        var payload = new EventPayload(values, INamingPolicy.CamelCase);

        return new Event(id)
        {
            Type = type,
            UserId = userId,
            OccurredAt = entity.OccurredAt,
            EntityRef = entityRef,
            Payload = payload
        };
    }
}
