﻿using Freem.Entities.Identifiers;
using Freem.Entities.Relations.Collections;
using Freem.Entities.Storage.PostgreSQL.Database.Entities;
using Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Tags;
using DatabaseActivityStatus = Freem.Entities.Storage.PostgreSQL.Database.Entities.Models.ActivityStatus;
using EntityActivityStatus = Freem.Entities.ActivityStatus;

namespace Freem.Entities.Storage.PostgreSQL.Implementations.Repositories.Activities;

internal static class ActivityMapper
{
    public static ActivityEntity MapToDatabaseEntity(this Activity activity)
    {
        return new ActivityEntity
        {
            Id = activity.Id.Value,
            Name = activity.Name,
            Status = MapToDatabaseEntityStatus(activity.Status),
            UserId = activity.UserId.Value
        };
    }

    public static DatabaseActivityStatus MapToDatabaseEntityStatus(this EntityActivityStatus status)
    {
        return status switch
        {
            EntityActivityStatus.Active => DatabaseActivityStatus.Active,
            EntityActivityStatus.Archived => DatabaseActivityStatus.Archived,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    public static Activity MapToDomainEntity(this ActivityEntity entity)
    {
        var id = new ActivityIdentifier(entity.Id);
        var userId = new UserIdentifier(entity.UserId);
        var tags = MapToDomainRelatedTags(entity);
        
        var activity = new Activity(id, userId, tags, MapToDomainEntityStatus(entity.Status));
        if (entity.Name is not null)
            activity.Name = entity.Name;
        
        return activity;
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

    public static RelatedTagsCollection MapToDomainRelatedTags(this ActivityEntity entity)
    {
        var tags = Enumerable.Empty<Tag>();
        if (entity.Tags is not null)
            tags = entity.Tags.Select(TagMapper.MapToDomainEntity);

        return new RelatedTagsCollection(tags);
    }
}
