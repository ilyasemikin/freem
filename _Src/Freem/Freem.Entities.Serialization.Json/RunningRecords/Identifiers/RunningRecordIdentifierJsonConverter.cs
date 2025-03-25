using System.Text.Json;
using System.Text.Json.Serialization;
using Freem.Entities.RunningRecords.Identifiers;

namespace Freem.Entities.Serialization.Json.RunningRecords.Identifiers;

public sealed class RunningRecordIdentifierJsonConverter : JsonConverter<RunningRecordIdentifier>
{
    public override RunningRecordIdentifier? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType is not JsonTokenType.String)
            throw new JsonException();

        var value = reader.GetString()!;
        return value;
    }

    public override void Write(Utf8JsonWriter writer, RunningRecordIdentifier value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value);
    }
}