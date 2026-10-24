using System.Text.Json;
using System.Text.Json.Serialization;
using Freem.Domain.Events;
using Utilities.Naming.Abstractions;

namespace Freem.Domain.Serialization.Json.Events;

public sealed class EventPayloadJsonConverter : JsonConverter<EventPayload>
{
    public override EventPayload Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var values = JsonSerializer.Deserialize<Dictionary<string, string?>>(ref reader, options);
        if (values is null)
            throw new JsonException();

        return new EventPayload(values, INamingPolicy.CamelCase);
    }

    public override void Write(Utf8JsonWriter writer, EventPayload value, JsonSerializerOptions options)
    {
        var values = value.ToReadOnlyDictionary();
        JsonSerializer.Serialize(writer, values, options);
    }
}
