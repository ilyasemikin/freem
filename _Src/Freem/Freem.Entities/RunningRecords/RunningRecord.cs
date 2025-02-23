using Freem.Clones;
using Freem.Entities.Abstractions;
using Freem.Entities.Abstractions.Events.Identifiers;
using Freem.Entities.Abstractions.Relations;
using Freem.Entities.Abstractions.Relations.Collection;
using Freem.Entities.Activities;
using Freem.Entities.Identifiers;
using Freem.Entities.Models.Records;
using Freem.Entities.Relations.Collections;
using Freem.Entities.RunningRecords.Events.Removed;
using Freem.Entities.RunningRecords.Events.Started;
using Freem.Entities.RunningRecords.Events.Stopped;
using Freem.Entities.RunningRecords.Events.Updated;
using Freem.Entities.Tags;
using Freem.Time;

namespace Freem.Entities.RunningRecords;

public class RunningRecord : 
    IEntity<RunningRecordIdentifier>,
    IEntityRelation<Activity, ActivityIdentifier>, 
    IEntityRelation<Tag, TagIdentifier>,
    ICloneable<RunningRecord>
{
    public const string EntityName = "running_record";
    
    public RunningRecordIdentifier Id => UserId;
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

        StartAt = DateTimeOperations.EraseMilliseconds(startAt.UtcDateTime);
    }

    public RunningRecordStartedEvent BuildStartedEvent(EventIdentifier eventId)
    {
        return new RunningRecordStartedEvent(eventId, Id);
    }

    public RunningRecordStoppedEvent BuildStoppedEvent(EventIdentifier eventId)
    {
        return new RunningRecordStoppedEvent(eventId, Id);
    }

    public RunningRecordUpdatedEvent BuildUpdatedEvent(EventIdentifier eventId)
    {
        return new RunningRecordUpdatedEvent(eventId, Id);
    }

    public RunningRecordRemovedEvent BuildRemovedEvent(EventIdentifier eventId)
    {
        return new RunningRecordRemovedEvent(eventId, Id);
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
