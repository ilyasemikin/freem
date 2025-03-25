using System.Text.Json;
using System.Text.Json.Serialization;
using Freem.Entities.Statistics.Time;

namespace Freem.Entities.Serialization.Json.Statistics;

public sealed class TimeStatisticsPerActivityCollectionJsonConverter 
    : JsonConverter<TimeStatisticsPerActivityCollection>
{
    public override TimeStatisticsPerActivityCollection? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var enumerable = JsonSerializer.Deserialize<IEnumerable<TimeStatisticsPerActivity>>(ref reader, options);
        if (enumerable is null)
            throw new JsonException();
        
        return new TimeStatisticsPerActivityCollection(enumerable);
    }

    public override void Write(Utf8JsonWriter writer, TimeStatisticsPerActivityCollection value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value.Values, options);
    }
}