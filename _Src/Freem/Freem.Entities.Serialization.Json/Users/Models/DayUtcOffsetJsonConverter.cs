using System.Text.Json;
using System.Text.Json.Serialization;
using Freem.Entities.Users.Models;

namespace Freem.Entities.Serialization.Json.Users.Models;

public sealed class DayUtcOffsetJsonConverter : JsonConverter<DayUtcOffset>
{
    public override DayUtcOffset? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return JsonSerializer.Deserialize<TimeSpan?>(ref reader, options);
    }

    public override void Write(Utf8JsonWriter writer, DayUtcOffset value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, (TimeSpan)value, options);
    }
}