using System.Text.Json;
using System.Text.Json.Serialization;
using Freem.Entities.Records.Identifiers;

namespace Freem.Entities.Serialization.Json.Records.Identifiers;

public class RecordIdentifierJsonConverter : JsonConverter<RecordIdentifier>
{
    public override RecordIdentifier? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (!reader.Read() || reader.TokenType is not JsonTokenType.String)
            throw new JsonException();

        var value = reader.GetString()!;
        return value;
    }

    public override void Write(Utf8JsonWriter writer, RecordIdentifier value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value);
    }
}