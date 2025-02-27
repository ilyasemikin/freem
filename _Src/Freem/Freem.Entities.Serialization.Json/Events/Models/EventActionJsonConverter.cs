using System.Text.Json;
using System.Text.Json.Serialization;
using Freem.Entities.Abstractions.Events.Models;

namespace Freem.Entities.Serialization.Json.Events.Models;

public sealed class EventActionJsonConverter : JsonConverter<EventAction>
{
    public override EventAction? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (!reader.Read() || reader.TokenType is not JsonTokenType.String)
            throw new JsonException();

        var value = reader.GetString()!;
        return value;
    }

    public override void Write(Utf8JsonWriter writer, EventAction value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value);
    }
}