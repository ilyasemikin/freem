using System.Text.Json;
using System.Text.Json.Serialization;
using Freem.Entities.Abstractions.Events;
using Freem.Entities.Abstractions.Events.Identifiers;
using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.Events;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.Serialization.Json.Events;

public sealed class EventJsonConverter : JsonConverter<IEntityEvent<IEntityIdentifier, UserIdentifier>>
{
    private const string EntityPropertyName = "entity";
    private const string ValuePropertyName = "value";

    private readonly EventsFactory _eventsFactory;
    private readonly EntityIdentifierFactory _entityIdentifierFactory;
    private readonly EntityIdentifierNameProvider _nameProvider;
    
    public EventJsonConverter(
        EventsFactory eventsFactory, 
        EntityIdentifierFactory entityIdentifierFactory, 
        EntityIdentifierNameProvider nameProvider)
    {
        ArgumentNullException.ThrowIfNull(eventsFactory);
        ArgumentNullException.ThrowIfNull(entityIdentifierFactory);
        ArgumentNullException.ThrowIfNull(nameProvider);
        
        _eventsFactory = eventsFactory;
        _entityIdentifierFactory = entityIdentifierFactory;
        _nameProvider = nameProvider;
    }
    
    public override IEntityEvent<IEntityIdentifier, UserIdentifier>? Read(
        ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var document = JsonDocument.ParseValue(ref reader);

        if (!document.RootElement.TryGetProperty(EntityPropertyName, out var entityElement) || entityElement.ValueKind != JsonValueKind.String)
            throw new JsonException();
        if (!document.RootElement.TryGetProperty(ValuePropertyName, out var valueElement) || valueElement.ValueKind != JsonValueKind.Object)
            throw new JsonException();

        var entity = entityElement.GetString()!;
        
        if (!valueElement.TryGetProperty(nameof(IEntityEvent<IEntityIdentifier, UserIdentifier>.Id), out var idElement) || idElement.ValueKind != JsonValueKind.String)
            throw new JsonException();
        if (!valueElement.TryGetProperty(nameof(IEntityEvent<IEntityIdentifier, UserIdentifier>.EntityId), out var entityIdElement) || entityIdElement.ValueKind != JsonValueKind.String)
            throw new JsonException();
        if (!valueElement.TryGetProperty(nameof(IEntityEvent<IEntityIdentifier, UserIdentifier>.UserEntityId), out var userEntityIdElement) || userEntityIdElement.ValueKind != JsonValueKind.String)
            throw new JsonException();
        if (!valueElement.TryGetProperty(nameof(IEntityEvent<IEntityIdentifier, UserIdentifier>.Action), out var actionElement) || actionElement.ValueKind != JsonValueKind.String)
            throw new JsonException();
        
        var idValue = idElement.GetString()!;
        var entityIdValue = entityIdElement.GetString()!;
        var userEntityIdValue = userEntityIdElement.GetString()!;
        var actionValue = actionElement.GetString()!;

        var id = new EventIdentifier(idValue);
        var entityId = _entityIdentifierFactory.Create(entity, entityIdValue);
        var userId = new UserIdentifier(userEntityIdValue);
        
        return _eventsFactory.Create(id, entityId, userId, actionValue);
    }

    public override void Write(
        Utf8JsonWriter writer, IEntityEvent<IEntityIdentifier, UserIdentifier> value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        var entity = _nameProvider.Get(value.EntityId);
        writer.WriteString(EntityPropertyName, entity);

        writer.WritePropertyName(ValuePropertyName);
        
        writer.WriteStartObject();
        writer.WriteString(nameof(IEntityEvent<IEntityIdentifier, UserIdentifier>.Id), value.Id);
        writer.WriteString(nameof(IEntityEvent<IEntityIdentifier, UserIdentifier>.EntityId), value.EntityId.ToString());
        writer.WriteString(nameof(IEntityEvent<IEntityIdentifier, UserIdentifier>.UserEntityId), value.UserEntityId.ToString());
        writer.WriteString(nameof(IEntityEvent<IEntityIdentifier, UserIdentifier>.Action), value.Action);
        writer.WriteEndObject();
        
        writer.WriteEndObject();
    }
}