using Freem.Domain.Events;
using Freem.Domain.Records.Models;
using Freem.Domain.Relations.Collections;
using Freem.Domain.RunningRecords;
using Freem.Domain.RunningRecords.Events.Removed;
using Freem.Domain.RunningRecords.Events.Started;
using Freem.Domain.RunningRecords.Events.Stopped;
using Freem.Domain.RunningRecords.Events.Updated;
using Freem.Domain.Users;
using Freem.Time;

namespace Freem.Domain.Entities.RunningRecords;

public class RunningRecord : 
    IEntity<RunningRecordId>
{
    public RunningRecordId Id => RunningRecordId.FromUserId(UserId);
    public UserId UserId { get; }
    public RelatedActivityIdsCollection Activities { get; }
    public RelatedTagIdsCollection Tags { get; }
    

    public RecordName? Name { get; set; }
    public RecordDescription? Description { get; set; }

    public DateTimeOffset StartAt { get; }

    public RunningRecord(
        UserId userId,
        RelatedActivityIdsCollection activities,
        RelatedTagIdsCollection tags,
        DateTimeOffset startAt)
    {
        ArgumentNullException.ThrowIfNull(activities);
        ArgumentNullException.ThrowIfNull(tags);

        UserId = userId;
        Activities = activities;
        Tags = tags;

        StartAt = DateTimeOperations.EraseMilliseconds(startAt.UtcDateTime);
    }

    public RunningRecordStartedEvent BuildStartedEvent(EventId eventId)
    {
        return new RunningRecordStartedEvent(eventId, Id);
    }

    public RunningRecordStoppedEvent BuildStoppedEvent(EventId eventId)
    {
        return new RunningRecordStoppedEvent(eventId, Id);
    }

    public RunningRecordUpdatedEvent BuildUpdatedEvent(EventId eventId)
    {
        return new RunningRecordUpdatedEvent(eventId, Id);
    }

    public RunningRecordRemovedEvent BuildRemovedEvent(EventId eventId)
    {
        return new RunningRecordRemovedEvent(eventId, Id);
    }
}
