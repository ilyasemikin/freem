using System.Text.Json;
using System.Text.Json.Serialization;
using Freem.Domain.Activities;
using Freem.Domain.Activities.Models;
using Freem.Domain.Events;
using Freem.Domain.Records;
using Freem.Domain.Records.Models;
using Freem.Domain.RunningRecords;
using Freem.Domain.Tags;
using Freem.Domain.Tags.Models;
using Freem.Domain.Users;
using Freem.Domain.Users.Models;
using Freem.Domain.Serialization.Json.Events;
using Freem.Domain.Serialization.Json.Relations;
using Freem.Domain.Serialization.Json.Statistics;
using Freem.Time.Models;
using Utilities.Serialization.Json;

namespace Freem.Domain.Serialization.Json;

public static class EntitiesJsonSerialization
{
    private static readonly IReadOnlyList<JsonConverter> Converters;

    static EntitiesJsonSerialization()
    {
        Converters = CreateConverters().ToArray();
    }

    public static JsonSerializerOptions CreateSerializerOptions()
    {
        var options = new JsonSerializerOptions();
        Populate(options);
        
        return options;
    }
    
    public static void Populate(JsonSerializerOptions options)
    {
        options.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
        
        foreach (var converter in Converters)
            options.Converters.Add(converter);
    }
    
    private static IEnumerable<JsonConverter> CreateConverters()
    {
        yield return new ParsableJsonConverter<DatePeriod>();
        yield return new ParsableJsonConverter<DateTimePeriod>();
        
        yield return new ParsableJsonConverter<ActivityId>();
        yield return new ParsableJsonConverter<ActivityName>();
        yield return new ParsableJsonConverter<ActivityStatus>();

        yield return new ParsableJsonConverter<EventId>();
        yield return new ParsableJsonConverter<EventType>();
        yield return new JsonStringEnumConverter<EntityType>(JsonNamingPolicy.SnakeCaseLower);
        yield return new EventPayloadJsonConverter();

        yield return new ParsableJsonConverter<RecordId>();
        yield return new ParsableJsonConverter<RecordDescription>();
        yield return new ParsableJsonConverter<RecordName>();
        yield return new ParsableJsonConverter<RecordPeriodId>();
        yield return new ParsableJsonConverter<RecordPeriodDescription>();

        yield return new ParsableJsonConverter<RunningRecordId>();

        yield return new ParsableJsonConverter<TagId>();
        yield return new ParsableJsonConverter<TagName>();

        yield return new TimeStatisticsPerActivityCollectionJsonConverter();

        yield return new ParsableJsonConverter<DayUtcOffset>();
        yield return new ParsableJsonConverter<Nickname>();

        yield return new ParsableJsonConverter<UserId>();
        
        yield return new RelatedIdsCollectionJsonConverter<ActivityId>();
        yield return new RelatedIdsCollectionJsonConverter<TagId>();

    }
}