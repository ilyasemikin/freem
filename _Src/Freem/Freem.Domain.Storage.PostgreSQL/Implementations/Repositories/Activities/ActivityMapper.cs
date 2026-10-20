using Freem.Domain.Activities;
using Freem.Domain.Activities.Models;
using Freem.Domain.Entities.Activities;
using Freem.Domain.Relations.Collections;
using Freem.Domain.Users;
using Freem.Domain.Storage.PostgreSQL.Database.Entities;
using Freem.Domain.Storage.PostgreSQL.Implementations.Mappers;
using DatabaseActivityStatus = Freem.Domain.Storage.PostgreSQL.Database.Entities.Models.ActivityStatus;
using EntityActivityStatus = Freem.Domain.Activities.Models.ActivityStatus;

namespace Freem.Domain.Storage.PostgreSQL.Implementations.Repositories.Activities;

internal static class ActivityMapper
{
    public static ActivityEntity MapToDatabaseEntity(this Activity activity)
    {
        return new ActivityEntity
        {
            Id = activity.Id.ToString(),
            Name = activity.Name.ToString(),
            Status = MapToDatabaseEntityStatus(activity.Status),
            UserId = activity.UserId.ToString()
        };
    }

    public static DatabaseActivityStatus MapToDatabaseEntityStatus(this EntityActivityStatus status)
    {
        return status.ToValue() switch
        {
            EntityActivityStatus.Value.Active => DatabaseActivityStatus.Active,
            EntityActivityStatus.Value.Archived => DatabaseActivityStatus.Archived,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    public static Activity MapToDomainEntity(this ActivityEntity entity)
    {
        var id = ActivityId.Parse(entity.Id, null);
        var userId = UserId.Parse(entity.UserId, null);
        var name = ActivityName.Parse(entity.Name, null);
        var tags = MapToDomainRelatedTags(entity);
        
        return new Activity(id, userId, name, tags, MapToDomainEntityStatus(entity.Status));
    }
    
    public static EntityActivityStatus MapToDomainEntityStatus(this DatabaseActivityStatus status)
    {
        return status switch
        {
            Database.Entities.Models.ActivityStatus.Active => EntityActivityStatus.Active,
            Database.Entities.Models.ActivityStatus.Archived => EntityActivityStatus.Archived,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public static RelatedTagIdsCollection MapToDomainRelatedTags(this ActivityEntity entity)
    {
        return entity.Tags.MapToRelatedTagsCollection();
    }
}
