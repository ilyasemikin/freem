using System.Text.Json;
using System.Text.Json.Serialization;
using Freem.Entities.Abstractions.Events.Identifiers;

namespace Freem.Entities.Serialization.Json.Events.Identifiers;

public sealed class EventIdentifierJsonConverter : JsonConverter<EventIdentifier>
{
    public override EventIdentifier? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType is not JsonTokenType.String)
            throw new JsonException();

        var value = reader.GetString()!;
        return value;
    }

    public override void Write(Utf8JsonWriter writer, EventIdentifier value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value);
    }
}