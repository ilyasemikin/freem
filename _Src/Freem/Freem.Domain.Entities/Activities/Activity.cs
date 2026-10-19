using Freem.Domain.Activities.Events.Arhived;
using Freem.Domain.Activities.Events.Created;
using Freem.Domain.Activities.Events.Removed;
using Freem.Domain.Activities.Events.Unarchived;
using Freem.Domain.Activities.Events.Updated;
using Freem.Domain.Activities.Identifiers;
using Freem.Domain.Activities.Models;
using Freem.Domain.Events.Identifiers;
using Freem.Domain.Relations.Collections;
using Freem.Domain.Users.Identifiers;

namespace Freem.Domain.Entities.Activities;

public sealed class Activity : 
    IEntity<ActivityIdentifier>
{
    private ActivityName _activityName = null!;
    private ActivityStatus _status = null!;

    public ActivityIdentifier Id { get; }
    public UserIdentifier UserId { get; }
    public RelatedTagsIdentifiersCollection Tags { get; }

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
        ActivityIdentifier id,
        UserIdentifier userId,
        ActivityName name,
        RelatedTagsIdentifiersCollection relatedTags,
        ActivityStatus status)
    {
        ArgumentNullException.ThrowIfNull(relatedTags);

        Id = id;
        UserId = userId;
        Name = name;
        Tags = relatedTags;
        Status = status;
    }

    public ActivityCreatedEvent BuildCreatedEvent(EventIdentifier eventId)
    {
        return new ActivityCreatedEvent(eventId, Id, UserId);
    }

    public ActivityUpdatedEvent BuildUpdatedEvent(EventIdentifier eventId)
    {
        return new ActivityUpdatedEvent(eventId, Id, UserId);
    }

    public ActivityRemovedEvent BuildRemovedEvent(EventIdentifier eventId)
    {
        return new ActivityRemovedEvent(eventId, Id, UserId);
    }

    public ActivityArchivedEvent BuildArchivedEvent(EventIdentifier eventId)
    {
        return new ActivityArchivedEvent(eventId, Id, UserId);
    }

    public ActivityUnarchivedEvent BuildUnarchivedEvent(EventIdentifier eventId)
    {
        return new ActivityUnarchivedEvent(eventId, Id, UserId);
    }
}
