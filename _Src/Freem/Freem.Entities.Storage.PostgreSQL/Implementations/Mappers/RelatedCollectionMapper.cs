using Freem.Domain.Activities.Identifiers;
using Freem.Domain.Relations.Collections;
using Freem.Domain.Tags.Identifiers;
using Freem.Entities.Storage.PostgreSQL.Database.Entities;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Mappers;

internal static class RelatedCollectionMapper
{
    public static RelatedTagsIdentifiersCollection MapToRelatedTagsCollection(this IEnumerable<TagEntity>? entities)
    {
        var identifiers = Enumerable.Empty<TagIdentifier>();
        if (entities is not null)
            identifiers = entities.Select(entity => new TagIdentifier(entity.Id));

        return new RelatedTagsIdentifiersCollection(identifiers);
    }

    public static RelatedActivitiesIdentifiersCollection MapToRelatedActivitiesCollection(
        this IEnumerable<ActivityEntity>? entities)
    {
        var identifiers = Enumerable.Empty<ActivityIdentifier>();
        if (entities is not null)
            identifiers = entities.Select(entity => new ActivityIdentifier(entity.Id));

        return new RelatedActivitiesIdentifiersCollection(identifiers);
    }
}