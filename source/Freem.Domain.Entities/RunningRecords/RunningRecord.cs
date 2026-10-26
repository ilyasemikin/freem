using Freem.Domain.Events;
using Freem.Domain.Records.Models;
using Freem.Domain.Relations.Collections;
using Freem.Domain.RunningRecords;
using Freem.Domain.RunningRecords.Events;
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

    private DateTimeOffset _startAt;

    public DateTimeOffset StartAt
    {
        get => _startAt;
        set => _startAt = DateTimeOperations.TruncateToMilliseconds(value.UtcDateTime);
    }

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

        StartAt = startAt;
    }

    public RunningRecordStartedEvent BuildStartedEvent(EventId eventId)
    {
        var activityIds = Activities.Ids.ToArray();
        var tagIds = Tags.Ids.ToArray();
        return new RunningRecordStartedEvent(eventId)
        {
            RunningRecordId = Id,
            Name = Name,
            Description = Description,
            StartAt = StartAt,
            ActivityIds = activityIds,
            TagIds = tagIds
        };
    }

    public RunningRecordStoppedEvent BuildStoppedEvent(EventId eventId)
    {
        return new RunningRecordStoppedEvent(eventId)
        {
            RunningRecordId = Id
        };
    }

    public RunningRecordUpdatedEvent BuildUpdatedEvent(EventId eventId)
    {
        var activityIds = Activities.Ids.ToArray();
        var tagIds = Tags.Ids.ToArray();
        return new RunningRecordUpdatedEvent(eventId)
        {
            RunningRecordId = Id,
            Name = Name,
            Description = Description,
            StartAt = StartAt,
            ActivityIds = activityIds,
            TagIds = tagIds
        };
    }

    public RunningRecordRemovedEvent BuildRemovedEvent(EventId eventId)
    {
        return new RunningRecordRemovedEvent(eventId)
        {
            RunningRecordId = Id
        };
    }
}
