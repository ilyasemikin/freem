using System.Text.Json;
using System.Text.Json.Serialization;
using Freem.Entities.Activities.Models;

namespace Freem.Entities.Serialization.Json.Activities.Models;

public sealed class ActivityStatusJsonConverter : JsonConverter<ActivityStatus>
{
    public override ActivityStatus? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType is not JsonTokenType.String)
            throw new JsonException();
        
        var valueString = reader.GetString()!;
        if (!Enum.TryParse<ActivityStatus.Value>(valueString, out var value))
            throw new JsonException();
        
        return value;
    }

    public override void Write(Utf8JsonWriter writer, ActivityStatus value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}