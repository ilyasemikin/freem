using Freem.Domain.Events.Identifiers;
using Freem.Domain.Records.Models;
using Freem.Domain.Relations.Collections;
using Freem.Domain.RunningRecords.Events.Removed;
using Freem.Domain.RunningRecords.Events.Started;
using Freem.Domain.RunningRecords.Events.Stopped;
using Freem.Domain.RunningRecords.Events.Updated;
using Freem.Domain.RunningRecords.Identifiers;
using Freem.Domain.Users.Identifiers;
using Freem.Time;

namespace Freem.Domain.Entities.RunningRecords;

public class RunningRecord : 
    IEntity<RunningRecordIdentifier>
{
    public RunningRecordIdentifier Id => UserId;
    public UserIdentifier UserId { get; }
    public RelatedActivitiesIdentifiersCollection Activities { get; }
    public RelatedTagsIdentifiersCollection Tags { get; }
    

    public RecordName? Name { get; set; }
    public RecordDescription? Description { get; set; }

    public DateTimeOffset StartAt { get; }

    public RunningRecord(
        UserIdentifier userId,
        RelatedActivitiesIdentifiersCollection activities,
        RelatedTagsIdentifiersCollection tags,
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
}
