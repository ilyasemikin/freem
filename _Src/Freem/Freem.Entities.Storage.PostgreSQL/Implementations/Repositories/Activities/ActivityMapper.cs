using Freem.Domain.Activities.Identifiers;
using Freem.Domain.Entities.Activities;
using Freem.Domain.Relations.Collections;
using Freem.Domain.Users.Identifiers;
using Freem.Entities.Storage.PostgreSQL.Database.Entities;
using Freem.Entities.Storage.PostgreSQL.Implementations.Mappers;
using DatabaseActivityStatus = Freem.Entities.Storage.PostgreSQL.Database.Entities.Models.ActivityStatus;
using EntityActivityStatus = Freem.Domain.Activities.Models.ActivityStatus;

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

    public static RelatedTagsIdentifiersCollection MapToDomainRelatedTags(this ActivityEntity entity)
    {
        return entity.Tags.MapToRelatedTagsCollection();
    }
}
