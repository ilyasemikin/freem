using Freem.Entities.Identifiers;
using Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Activities;

internal static class ActivityRelationFactory
{
    public static IEnumerable<ActivityTagRelationEntity> CreateDatabaseActivityTagRelations(this Activity activity)
    {
        return activity.Tags.Identifiers.Select(id => CreateDatabaseActivityTagRelation(activity.Id, id.Value));
    }

    public static ActivityTagRelationEntity CreateDatabaseActivityTagRelation(ActivityIdentifier activityId, string tagIdString)
    {
        return new ActivityTagRelationEntity
        {
            ActivityId = activityId.Value,
            TagId = tagIdString
        };
    }
}