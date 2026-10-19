using System.Text.Json;
using System.Text.Json.Serialization;
using Freem.Domain;
using Freem.Domain.Events;
using Freem.Domain.Events.Models;
using Freem.Domain.Users;

namespace Freem.Entities.Serialization.Json.Events;

public sealed class EventJsonConverter : JsonConverter<IEntityEvent<Id, UserId>>
{
    private const string EntityPropertyName = "entity";
    private const string ValuePropertyName = "value";

    private readonly EventsFactory _eventsFactory;
    private readonly EntityIdFactory _entityIdFactory;
    private readonly EntityIdNameProvider _nameProvider;
    
    public EventJsonConverter()
    {
        _eventsFactory = EventsFactory.Instance;
        _entityIdFactory = EntityIdFactory.Instance;
        _nameProvider = EntityIdNameProvider.Instance;
    }
    
    public override IEntityEvent<Id, UserId>? Read(
        ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var document = JsonDocument.ParseValue(ref reader);

        if (!document.RootElement.TryGetProperty(EntityPropertyName, out var entityElement) || entityElement.ValueKind != JsonValueKind.String)
            throw new JsonException();
        if (!document.RootElement.TryGetProperty(ValuePropertyName, out var valueElement) || valueElement.ValueKind != JsonValueKind.Object)
            throw new JsonException();

        var entity = entityElement.GetString()!;
        
        if (!valueElement.TryGetProperty(nameof(IEntityEvent<Id, UserId>.Id), out var idElement) || idElement.ValueKind != JsonValueKind.String)
            throw new JsonException();
        if (!valueElement.TryGetProperty(nameof(IEntityEvent<Id, UserId>.EntityId), out var entityIdElement) || entityIdElement.ValueKind != JsonValueKind.String)
            throw new JsonException();
        if (!valueElement.TryGetProperty(nameof(IEntityEvent<Id, UserId>.UserEntityId), out var userEntityIdElement) || userEntityIdElement.ValueKind != JsonValueKind.String)
            throw new JsonException();
        if (!valueElement.TryGetProperty(nameof(IEntityEvent<Id, UserId>.Action), out var actionElement) || actionElement.ValueKind != JsonValueKind.String)
            throw new JsonException();
        
        var idValue = idElement.GetString()!;
        var entityIdValue = entityIdElement.GetString()!;
        var userEntityIdValue = userEntityIdElement.GetString()!;
        var actionValue = actionElement.GetString()!;

        var id = EventId.Parse(idValue, null);
        var entityId = _entityIdFactory.Create(entity, entityIdValue);
        var userId = UserId.Parse(userEntityIdValue, null);
        var action = EventAction.Parse(actionValue, null);
        
        return _eventsFactory.Create(id, entityId, userId, action);
    }

    public override void Write(
        Utf8JsonWriter writer, IEntityEvent<Id, UserId> value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        var entity = _nameProvider.Get(value.EntityId);
        writer.WriteString(EntityPropertyName, entity);

        writer.WritePropertyName(ValuePropertyName);
        
        var id = value.Id.ToString();
        var entityId = value.EntityId.ToString();
        var userEntityId = value.UserEntityId.ToString();
        var action = value.Action.ToString();

        writer.WriteStartObject();
        writer.WriteString(nameof(IEntityEvent<Id, UserId>.Id), id);
        writer.WriteString(nameof(IEntityEvent<Id, UserId>.EntityId), entityId);
        writer.WriteString(nameof(IEntityEvent<Id, UserId>.UserEntityId), userEntityId);
        writer.WriteString(nameof(IEntityEvent<Id, UserId>.Action), action);
        writer.WriteEndObject();
        
        writer.WriteEndObject();
    }
}