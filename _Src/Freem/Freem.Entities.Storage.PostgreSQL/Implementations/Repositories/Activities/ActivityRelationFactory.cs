using Freem.Domain.Activities;
using Freem.Domain.Entities.Activities;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Activities;

internal static class ActivityRelationFactory
{
    public static IEnumerable<ActivityTagRelationEntity> CreateDatabaseActivityTagRelations(this Activity activity)
    {
        return activity.Tags.Ids
            .Select(id => id.ToString())
            .Select(tagId => CreateDatabaseActivityTagRelation(activity.Id, tagId));
    }

    public static ActivityTagRelationEntity CreateDatabaseActivityTagRelation(ActivityId activityId, string tagIdString)
    {
        return new ActivityTagRelationEntity
        {
            ActivityId = activityId.ToString(),
            TagId = tagIdString
        };
    }
}