using System.Text.Json;
using System.Text.Json.Serialization;
using Freem.Domain;
using Freem.Domain.Relations.Collections;
using Freem.Domain.Relations.Collections.Base;

namespace Freem.Domain.Serialization.Json.Relations;

public sealed class RelatedIdsCollectionJsonConverter<TId> : JsonConverter<IReadOnlyRelatedIdsCollection<TId>>
    where TId : Id
{
    public override IReadOnlyRelatedIdsCollection<TId>? Read(
        ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var ids = JsonSerializer.Deserialize<IEnumerable<TId>>(ref reader, options);
        return ids is not null
            ? new RelatedIdsCollection<TId>(ids)
            : null;
    }

    public override void Write(
        Utf8JsonWriter writer, IReadOnlyRelatedIdsCollection<TId> value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value.Ids, options);
    }
}