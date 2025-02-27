using System.Text.Json;
using System.Text.Json.Serialization;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.Serialization.Json.Users.Identifiers;

public sealed class UserIdentifierJsonConverter : JsonConverter<UserIdentifier>
{
    public override UserIdentifier? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (!reader.Read() || reader.TokenType is not JsonTokenType.String)
            throw new JsonException();
        
        var value = reader.GetString()!;
        return value;
    }

    public override void Write(Utf8JsonWriter writer, UserIdentifier value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value);
    }
}