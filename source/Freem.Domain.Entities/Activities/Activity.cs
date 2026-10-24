using Freem.Domain.Activities;
using Freem.Domain.Activities.Events;
using Freem.Domain.Activities.Models;
using Freem.Domain.Events;
using Freem.Domain.Relations.Collections;
using Freem.Domain.Users;

namespace Freem.Domain.Entities.Activities;

public sealed class Activity : 
    IEntity<ActivityId>
{
    public ActivityId Id { get; }
    public UserId UserId { get; }
    public RelatedTagIdsCollection Tags { get; }

    public ActivityName Name
    {
        get;
        set
        {
            ArgumentNullException.ThrowIfNull(value);

            field = value;
        }
    }

    public ActivityColor Color
    {
        get;
        set
        {
            ArgumentNullException.ThrowIfNull(value);

            field = value;
        }
    }

    public ActivityStatus Status
    {
        get;
        set
        {
            ArgumentNullException.ThrowIfNull(value);

            field = value;
        }
    }

    public Activity(
        ActivityId id,
        UserId userId,
        ActivityName name,
        ActivityColor color,
        RelatedTagIdsCollection relatedTags,
        ActivityStatus status)
    {
        ArgumentNullException.ThrowIfNull(relatedTags);

        Id = id;
        UserId = userId;
        Name = name;
        Color = color;
        Tags = relatedTags;
        Status = status;
    }

    public ActivityCreatedEvent BuildCreatedEvent(EventId eventId)
    {
        var tagIds = Tags.Ids.ToArray();
        return new ActivityCreatedEvent(eventId)
        {
            ActivityId = Id,
            Name = Name,
            Color = Color,
            Status = Status,
            TagIds = tagIds
        };
    }

    public ActivityUpdatedEvent BuildUpdatedEvent(EventId eventId)
    {
        var tagIds = Tags.Ids.ToArray();
        return new ActivityUpdatedEvent(eventId)
        {
            ActivityId = Id,
            Name = Name,
            Color = Color,
            Status = Status,
            TagIds = tagIds
        };
    }

    public ActivityRemovedEvent BuildRemovedEvent(EventId eventId)
    {
        return new ActivityRemovedEvent(eventId)
        {
            ActivityId = Id
        };
    }

    public ActivityArchivedEvent BuildArchivedEvent(EventId eventId)
    {
        return new ActivityArchivedEvent(eventId)
        {
            ActivityId = Id
        };
    }

    public ActivityUnarchivedEvent BuildUnarchivedEvent(EventId eventId)
    {
        return new ActivityUnarchivedEvent(eventId)
        {
            ActivityId = Id
        };
    }
}
