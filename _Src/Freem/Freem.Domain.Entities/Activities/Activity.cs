using Freem.Domain.Activities;
using Freem.Domain.Activities.Events.Arhived;
using Freem.Domain.Activities.Events.Created;
using Freem.Domain.Activities.Events.Removed;
using Freem.Domain.Activities.Events.Unarchived;
using Freem.Domain.Activities.Events.Updated;
using Freem.Domain.Activities.Models;
using Freem.Domain.Events;
using Freem.Domain.Relations.Collections;
using Freem.Domain.Users;

namespace Freem.Domain.Entities.Activities;

public sealed class Activity : 
    IEntity<ActivityId>
{
    private ActivityName _activityName = null!;
    private ActivityStatus _status = null!;

    public ActivityId Id { get; }
    public UserId UserId { get; }
    public RelatedTagIdsCollection Tags { get; }

    public ActivityName Name
    {
        get => _activityName;
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            
            _activityName = value;
        }
    }

    public ActivityStatus Status
    {
        get => _status;
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            
            _status = value;
        }
    }

    public Activity(
        ActivityId id,
        UserId userId,
        ActivityName name,
        RelatedTagIdsCollection relatedTags,
        ActivityStatus status)
    {
        ArgumentNullException.ThrowIfNull(relatedTags);

        Id = id;
        UserId = userId;
        Name = name;
        Tags = relatedTags;
        Status = status;
    }

    public ActivityCreatedEvent BuildCreatedEvent(EventId eventId)
    {
        return new ActivityCreatedEvent(eventId, Id, UserId);
    }

    public ActivityUpdatedEvent BuildUpdatedEvent(EventId eventId)
    {
        return new ActivityUpdatedEvent(eventId, Id, UserId);
    }

    public ActivityRemovedEvent BuildRemovedEvent(EventId eventId)
    {
        return new ActivityRemovedEvent(eventId, Id, UserId);
    }

    public ActivityArchivedEvent BuildArchivedEvent(EventId eventId)
    {
        return new ActivityArchivedEvent(eventId, Id, UserId);
    }

    public ActivityUnarchivedEvent BuildUnarchivedEvent(EventId eventId)
    {
        return new ActivityUnarchivedEvent(eventId, Id, UserId);
    }
}
