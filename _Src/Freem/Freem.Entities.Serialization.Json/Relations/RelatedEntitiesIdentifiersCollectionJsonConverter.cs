using System.Text.Json;
using System.Text.Json.Serialization;
using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.Abstractions.Relations.Collection;
using Freem.Entities.Relations.Collections.Base;

namespace Freem.Entities.Serialization.Json.Relations;

public sealed class RelatedEntitiesIdentifiersCollectionJsonConverter<TIdentifier> : JsonConverter<IReadOnlyRelatedEntitiesIdentifiersCollection<TIdentifier>>
    where TIdentifier : IEntityIdentifier
{
    public override IReadOnlyRelatedEntitiesIdentifiersCollection<TIdentifier>? Read(
        ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var ids = JsonSerializer.Deserialize<IEnumerable<TIdentifier>>(ref reader, options);
        return ids is not null
            ? new RelatedEntitiesIdentifiersCollection<TIdentifier>(ids)
            : null;
    }

    public override void Write(
        Utf8JsonWriter writer, IReadOnlyRelatedEntitiesIdentifiersCollection<TIdentifier> value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value.Identifiers, options);
    }
}