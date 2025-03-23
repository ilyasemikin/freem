using System.Text.Json;
using System.Text.Json.Serialization;
using Freem.Time.Models;

namespace Freem.Time.Serialization.Json;

public sealed class DateTimePeriodJsonConverter : JsonConverter<DateTimePeriod>
{
    public override DateTimePeriod? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var input = JsonSerializer.Deserialize<string>(ref reader, options);
        if (input is null)
            throw new JsonException();
        
        if (!DateTimePeriod.TryParse(input, out var result))
            throw new JsonException();
        
        return result;
    }

    public override void Write(Utf8JsonWriter writer, DateTimePeriod value, JsonSerializerOptions options)
    {
        var output = value.ToString();

        JsonSerializer.Serialize(writer, output, options);
    }
}