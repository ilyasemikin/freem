using System.Text.Json;
using System.Text.Json.Serialization;
using Freem.Entities.Tags.Models;

namespace Freem.Entities.Serialization.Json.Tags.Models;

public sealed class TagNameJsonConverter : JsonConverter<TagName>
{
    public override TagName? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (!reader.Read() || reader.TokenType is not JsonTokenType.String)
            throw new JsonException();

        var value = reader.GetString()!;
        return value;
    }

    public override void Write(Utf8JsonWriter writer, TagName value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value);
    }
}