using System.Text.Json;
using System.Text.Json.Serialization;
using Freem.Time.Models;

namespace Freem.Time.Serialization.Json;

public sealed class DatePeriodJsonConverter : JsonConverter<DatePeriod>
{
    public override DatePeriod? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var input = JsonSerializer.Deserialize<string>(ref reader, options);
        if (input is null)
            throw new JsonException();
        
        if (!DatePeriod.TryParse(input, out var period))
            throw new JsonException();

        return period;
    }

    public override void Write(Utf8JsonWriter writer, DatePeriod value, JsonSerializerOptions options)
    {
        var output = value.ToString();
        JsonSerializer.Serialize(writer, output, options);
    }
}