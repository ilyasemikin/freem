using System.Text.Json;
using Freem.Domain.Events;
using Freem.Domain.Storage.PostgreSQL.Database.Entities;
using Freem.Domain.Storage.Serialization;

namespace Freem.Domain.Storage.PostgreSQL.Implementations.Repositories.Events.Converters;

internal sealed class EventEnvelopeToDatabaseEntityConverter
{
    public EventEntity Convert(EventEnvelope envelope)
    {
        var @event = envelope.Event;

        var payload = EventPayloadSerializer.Serialize(@event);
        var values = payload.ToReadOnlyDictionary();

        return new EventEntity
        {
            Id = @event.Id.ToString(),
            UserId = envelope.UserId.ToString(),
            EventType = @event.Type.ToString(),
            EntityType = EntityTypeNames.ToName(@event.EntityRef.Type),
            EntityRef = @event.EntityRef.Value,
            OccurredAt = envelope.OccurredAt,
            Payload = JsonSerializer.Serialize(values)
        };
    }
}
