using System.Text.Json;
using System.Text.Json.Serialization;
using Freem.Entities.Records.Models;

namespace Freem.Entities.Serialization.Json.Records.Models;

public sealed class RecordNameJsonConverter : JsonConverter<RecordName>
{
    public override RecordName? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (!reader.Read() || reader.TokenType is not JsonTokenType.String)
            throw new JsonException();
        
        var value = reader.GetString()!;
        return value;
    }

    public override void Write(Utf8JsonWriter writer, RecordName value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value);
    }
}