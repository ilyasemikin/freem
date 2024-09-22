using Freem.Clones;
using Freem.Entities.Abstractions;
using Freem.Entities.Abstractions.Relations;
using Freem.Entities.Abstractions.Relations.Collection;
using Freem.Entities.Activities;
using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Common.Relations.Collections;
using Freem.Entities.Records.Models;
using Freem.Entities.Tags;
using Freem.Entities.Tags.Identifiers;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.RunningRecords;

public class RunningRecord : 
    IEntity<UserIdentifier>,
    IEntityRelation<Activity, ActivityIdentifier>, 
    IEntityRelation<Tag, TagIdentifier>,
    ICloneable<RunningRecord>
{
    public UserIdentifier Id => UserId;
    public UserIdentifier UserId { get; }
    public RelatedActivitiesCollection Activities { get; }
    public RelatedTagsCollection Tags { get; }
    
    IReadOnlyRelatedEntitiesCollection<Activity, ActivityIdentifier> IEntityRelation<Activity, ActivityIdentifier>.RelatedEntities => Activities;
    IReadOnlyRelatedEntitiesCollection<Tag, TagIdentifier> IEntityRelation<Tag, TagIdentifier>.RelatedEntities => Tags;

    public RecordName? Name { get; set; }
    public RecordDescription? Description { get; set; }

    public DateTimeOffset StartAt { get; }

    public RunningRecord(
        UserIdentifier userId,
        RelatedActivitiesCollection activities,
        RelatedTagsCollection tags,
        DateTimeOffset startAt)
    {
        ArgumentNullException.ThrowIfNull(activities);
        ArgumentNullException.ThrowIfNull(tags);

        UserId = userId;
        Activities = activities;
        Tags = tags;

        StartAt = startAt;
    }

    public RunningRecord Clone()
    {
        return new RunningRecord(UserId, Activities, Tags, StartAt)
        {
            Name = Name,
            Description = Description,
        };
    }
}
