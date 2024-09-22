using Freem.Entities.Activities;
using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Activities;

internal static class ActivityRelationFactory
{
    public static IEnumerable<ActivityTagRelationEntity> CreateDatabaseActivityTagRelations(this Activity activity)
    {
        return activity.Tags.Identifiers.Select(id => CreateDatabaseActivityTagRelation(activity.Id, id));
    }

    public static ActivityTagRelationEntity CreateDatabaseActivityTagRelation(ActivityIdentifier activityId, string tagIdString)
    {
        return new ActivityTagRelationEntity
        {
            ActivityId = activityId,
            TagId = tagIdString
        };
    }
}