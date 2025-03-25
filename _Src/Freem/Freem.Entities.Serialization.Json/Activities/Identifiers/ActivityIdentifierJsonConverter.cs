using System.Text.Json;
using System.Text.Json.Serialization;
using Freem.Entities.Activities.Identifiers;

namespace Freem.Entities.Serialization.Json.Activities.Identifiers;

public sealed class ActivityIdentifierJsonConverter : JsonConverter<ActivityIdentifier>
{
    public override ActivityIdentifier? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType is not JsonTokenType.String)
            throw new JsonException();

        var value = reader.GetString()!;
        return value;
    }

    public override void Write(Utf8JsonWriter writer, ActivityIdentifier value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value);
    }
}