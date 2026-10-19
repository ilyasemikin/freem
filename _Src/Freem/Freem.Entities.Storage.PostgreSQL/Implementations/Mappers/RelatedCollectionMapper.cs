using Freem.Domain.Activities;
using Freem.Domain.Relations.Collections;
using Freem.Domain.Tags;
using Freem.Entities.Storage.PostgreSQL.Database.Entities;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Mappers;

internal static class RelatedCollectionMapper
{
    public static RelatedTagIdsCollection MapToRelatedTagsCollection(this IEnumerable<TagEntity>? entities)
    {
        var ids = Enumerable.Empty<TagId>();
        if (entities is not null)
            ids = entities.Select(entity => TagId.Parse(entity.Id, null));

        return new RelatedTagIdsCollection(ids);
    }

    public static RelatedActivityIdsCollection MapToRelatedActivitiesCollection(
        this IEnumerable<ActivityEntity>? entities)
    {
        var ids = Enumerable.Empty<ActivityId>();
        if (entities is not null)
            ids = entities.Select(entity => ActivityId.Parse(entity.Id, null));

        return new RelatedActivityIdsCollection(ids);
    }
}