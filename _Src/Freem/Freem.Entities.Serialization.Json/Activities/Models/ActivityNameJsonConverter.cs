using System.Text.Json;
using System.Text.Json.Serialization;
using Freem.Entities.Activities.Models;

namespace Freem.Entities.Serialization.Json.Activities.Models;

public sealed class ActivityNameJsonConverter : JsonConverter<ActivityName>
{
    public override ActivityName? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType is not JsonTokenType.String)
            throw new JsonException();
        
        var value = reader.GetString()!;
        return value;
    }

    public override void Write(Utf8JsonWriter writer, ActivityName value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value);
    }
}