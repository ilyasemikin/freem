using System.Text.Json;
using System.Text.Json.Serialization;
using Freem.Entities.Activities;
using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Serialization.Json.Activities.Identifiers;
using Freem.Entities.Serialization.Json.Activities.Models;
using Freem.Entities.Serialization.Json.Events;
using Freem.Entities.Serialization.Json.Events.Identifiers;
using Freem.Entities.Serialization.Json.Events.Models;
using Freem.Entities.Serialization.Json.Records.Identifiers;
using Freem.Entities.Serialization.Json.Records.Models;
using Freem.Entities.Serialization.Json.Relations;
using Freem.Entities.Serialization.Json.RunningRecords.Identifiers;
using Freem.Entities.Serialization.Json.Statistics;
using Freem.Entities.Serialization.Json.Tags.Identifiers;
using Freem.Entities.Serialization.Json.Tags.Models;
using Freem.Entities.Serialization.Json.Users.Identifiers;
using Freem.Entities.Serialization.Json.Users.Models;
using Freem.Entities.Tags;
using Freem.Entities.Tags.Identifiers;
using Freem.Time.Serialization.Json;

namespace Freem.Entities.Serialization.Json;

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
        yield return new DatePeriodJsonConverter();
        yield return new DateTimePeriodJsonConverter();
        
        yield return new ActivityIdentifierJsonConverter();
        yield return new ActivityNameJsonConverter();
        yield return new ActivityStatusJsonConverter();

        yield return new EventIdentifierJsonConverter();
        yield return new EventActionJsonConverter();
        yield return new EventJsonConverter();

        yield return new RecordIdentifierJsonConverter();
        yield return new RecordDescriptionJsonConverter();
        yield return new RecordNameJsonConverter();

        yield return new RunningRecordIdentifierJsonConverter();

        yield return new TagIdentifierJsonConverter();
        yield return new TagNameJsonConverter();

        yield return new TimeStatisticsPerActivityCollectionJsonConverter();

        yield return new DayUtcOffsetJsonConverter();
        yield return new LoginJsonConverter();
        yield return new NicknameJsonConverter();
        yield return new PasswordJsonConverter();

        yield return new UserIdentifierJsonConverter();
        
        yield return new RelatedEntitiesIdentifiersCollectionJsonConverter<ActivityIdentifier>();
        yield return new RelatedEntitiesIdentifiersCollectionJsonConverter<TagIdentifier>();

        yield return new RelatedEntitiesCollectionJsonConverter<Activity, ActivityIdentifier>();
        yield return new RelatedEntitiesCollectionJsonConverter<Tag, TagIdentifier>();
    }
}