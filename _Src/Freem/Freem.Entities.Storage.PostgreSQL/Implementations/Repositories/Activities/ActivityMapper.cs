using Freem.Entities.Activities;
using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Relations.Collections;
using Freem.Entities.Storage.PostgreSQL.Database.Entities;
using Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Tags;
using Freem.Entities.Tags;
using Freem.Entities.Users.Identifiers;
using DatabaseActivityStatus = Freem.Entities.Storage.PostgreSQL.Database.Entities.Models.ActivityStatus;
using EntityActivityStatus = Freem.Entities.Activities.Models.ActivityStatus;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Activities;

internal static class ActivityMapper
{
    public static ActivityEntity MapToDatabaseEntity(this Activity activity)
    {
        return new ActivityEntity
        {
            Id = activity.Id,
            Name = activity.Name,
            Status = MapToDatabaseEntityStatus(activity.Status),
            UserId = activity.UserId
        };
    }

    public static DatabaseActivityStatus MapToDatabaseEntityStatus(this EntityActivityStatus status)
    {
        return (EntityActivityStatus.Value)status switch
        {
            EntityActivityStatus.Value.Active => DatabaseActivityStatus.Active,
            EntityActivityStatus.Value.Archived => DatabaseActivityStatus.Archived,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    public static Activity MapToDomainEntity(this ActivityEntity entity)
    {
        var id = new ActivityIdentifier(entity.Id);
        var userId = new UserIdentifier(entity.UserId);
        var name = entity.Name;
        var tags = MapToDomainRelatedTags(entity);
        
        return new Activity(id, userId, name, tags, MapToDomainEntityStatus(entity.Status));
    }
    
    public static EntityActivityStatus MapToDomainEntityStatus(this DatabaseActivityStatus status)
    {
        return status switch
        {
            Database.Entities.Models.ActivityStatus.Active => EntityActivityStatus.Value.Active,
            Database.Entities.Models.ActivityStatus.Archived => EntityActivityStatus.Value.Archived,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public static RelatedTagsCollection MapToDomainRelatedTags(this ActivityEntity entity)
    {
        var tags = Enumerable.Empty<Tag>();
        if (entity.Tags is not null)
            tags = entity.Tags.Select(TagMapper.MapToDomainEntity);

        return new RelatedTagsCollection(tags);
    }
}
