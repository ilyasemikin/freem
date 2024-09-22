using Freem.Clones;
using Freem.Entities.Abstractions;
using Freem.Entities.Abstractions.Relations;
using Freem.Entities.Abstractions.Relations.Collection;
using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Activities.Models;
using Freem.Entities.Common.Relations.Collections;
using Freem.Entities.Tags;
using Freem.Entities.Tags.Identifiers;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.Activities;

public sealed class Activity : 
    IEntity<ActivityIdentifier>, 
    IEntityRelation<Tag, TagIdentifier>, 
    ICloneable<Activity>
{
    private ActivityName _activityName = null!;
    private ActivityStatus _status = null!;

    public ActivityIdentifier Id { get; }
    public UserIdentifier UserId { get; }
    public RelatedTagsCollection Tags { get; }

    IReadOnlyRelatedEntitiesCollection<Tag, TagIdentifier> IEntityRelation<Tag, TagIdentifier>.RelatedEntities => Tags;

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
        RelatedTagsCollection relatedTags,
        ActivityStatus status)
    {
        ArgumentNullException.ThrowIfNull(relatedTags);

        Id = id;
        UserId = userId;
        Name = name;
        Tags = relatedTags;
        Status = status;
    }

    public Activity Clone()
    {
        return new Activity(Id, UserId, Name, Tags, Status)
        {
            Status = Status
        };
    }
}
