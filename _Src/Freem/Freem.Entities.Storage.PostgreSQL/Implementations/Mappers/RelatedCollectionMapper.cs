using Freem.Entities.Activities;
using Freem.Entities.Common.Relations.Collections;
using Freem.Entities.Storage.PostgreSQL.Database.Entities;
using Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Activities;
using Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Tags;
using Freem.Entities.Tags;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Mappers;

internal static class RelatedCollectionMapper
{
    public static RelatedTagsCollection MapToRelatedTagsCollection(this IEnumerable<TagEntity>? entities)
    {
        var tags = Enumerable.Empty<Tag>();
        if (entities is not null)
            tags = entities.Select(TagMapper.MapToDomainEntity);

        return new RelatedTagsCollection(tags);
    }

    public static RelatedActivitiesCollection MapToRelatedActivitiesCollection(this IEnumerable<ActivityEntity>? entities)
    {
        var activities = Enumerable.Empty<Activity>();
        if (entities is not null)
            activities = entities.Select(ActivityMapper.MapToDomainEntity);

        return new RelatedActivitiesCollection(activities);
    }
}
