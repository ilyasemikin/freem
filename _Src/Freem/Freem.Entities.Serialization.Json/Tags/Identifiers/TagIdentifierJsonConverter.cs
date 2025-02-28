using System.Text.Json;
using System.Text.Json.Serialization;
using Freem.Entities.Tags.Identifiers;

namespace Freem.Entities.Serialization.Json.Tags.Identifiers;

public sealed class TagIdentifierJsonConverter : JsonConverter<TagIdentifier>
{
    public override TagIdentifier? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType is not JsonTokenType.String)
            throw new JsonException();
        
        var value = reader.GetString()!;
        return value;
    }

    public override void Write(Utf8JsonWriter writer, TagIdentifier value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value);
    }
}