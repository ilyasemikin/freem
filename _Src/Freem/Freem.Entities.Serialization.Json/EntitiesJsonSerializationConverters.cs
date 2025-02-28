using System.Text.Json.Serialization;
using Freem.Entities.Events;
using Freem.Entities.Serialization.Json.Activities.Identifiers;
using Freem.Entities.Serialization.Json.Activities.Models;
using Freem.Entities.Serialization.Json.Events;
using Freem.Entities.Serialization.Json.Events.Identifiers;
using Freem.Entities.Serialization.Json.Events.Models;
using Freem.Entities.Serialization.Json.Records.Identifiers;
using Freem.Entities.Serialization.Json.Records.Models;
using Freem.Entities.Serialization.Json.RunningRecords.Identifiers;
using Freem.Entities.Serialization.Json.Tags.Identifiers;
using Freem.Entities.Serialization.Json.Tags.Models;
using Freem.Entities.Serialization.Json.Users.Models;

namespace Freem.Entities.Serialization.Json;

public static class EntitiesJsonSerialization
{
    private static readonly IReadOnlyList<JsonConverter> Converters;

    static EntitiesJsonSerialization()
    {
        Converters = CreateConverters().ToArray();
    }

    public static void Populate(IList<JsonConverter> converters)
    {
        foreach (var converter in Converters)
            converters.Add(converter);
    }
    
    private static IEnumerable<JsonConverter> CreateConverters()
    {
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

        yield return new DayUtcOffsetJsonConverter();
        yield return new LoginJsonConverter();
        yield return new NicknameJsonConverter();
        yield return new PasswordJsonConverter();
    }
}