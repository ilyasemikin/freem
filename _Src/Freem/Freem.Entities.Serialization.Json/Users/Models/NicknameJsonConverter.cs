﻿using System.Text.Json;
using System.Text.Json.Serialization;
using Freem.Entities.Users.Models;

namespace Freem.Entities.Serialization.Json.Users.Models;

public sealed class NicknameJsonConverter : JsonConverter<Nickname>
{
    public override Nickname? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType is not JsonTokenType.String)
            throw new JsonException();

        var value = reader.GetString()!;
        return value;
    }

    public override void Write(Utf8JsonWriter writer, Nickname value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value);
    }
}