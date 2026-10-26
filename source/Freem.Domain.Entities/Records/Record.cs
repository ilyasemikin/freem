using Freem.Domain.Events;
using Freem.Domain.Records;
using Freem.Domain.Records.Events;
using Freem.Domain.Records.Models;
using Freem.Domain.Records.Sources;
using Freem.Domain.Relations.Collections;
using Freem.Domain.Users;

namespace Freem.Domain.Entities.Records;

public sealed class Record : 
    IEntity<RecordId>
{
    public RecordId Id { get; }
    public UserId UserId { get; }
    public RelatedActivityIdsCollection Activities { get; }
    public RelatedTagIdsCollection Tags { get; }

    public RecordName? Name { get; set; }
    public RecordDescription? Description { get; set; }

    public RecordPeriodsCollection Periods { get; }

    public RecordSource Source { get; }

    public Record(
        RecordId id,
        UserId userId,
        RelatedActivityIdsCollection activities,
        RelatedTagIdsCollection tags,
        RecordPeriodsCollection periods,
        RecordSource source)
    {
        ArgumentNullException.ThrowIfNull(activities);
        ArgumentNullException.ThrowIfNull(tags);
        ArgumentNullException.ThrowIfNull(periods);
        ArgumentNullException.ThrowIfNull(source);

        Id = id;
        UserId = userId;
        
        Activities = activities;
        Tags = tags;
        
        Periods = periods;
        Source = source;
    }

    public RecordCreatedEvent BuildCreatedEvent(EventId eventId)
    {
        var activityIds = Activities.Ids.ToArray();
        var tagIds = Tags.Ids.ToArray();
        var periods = CreatePeriodSnapshots();
        return new RecordCreatedEvent(eventId)
        {
            RecordId = Id,
            Name = Name,
            Description = Description,
            Periods = periods,
            ActivityIds = activityIds,
            TagIds = tagIds
        };
    }

    public RecordUpdatedEvent BuildUpdatedEvent(EventId eventId)
    {
        var activityIds = Activities.Ids.ToArray();
        var tagIds = Tags.Ids.ToArray();
        var periods = CreatePeriodSnapshots();
        return new RecordUpdatedEvent(eventId)
        {
            RecordId = Id,
            Name = Name,
            Description = Description,
            Periods = periods,
            ActivityIds = activityIds,
            TagIds = tagIds
        };
    }

    private RecordPeriodSnapshot[] CreatePeriodSnapshots()
    {
        return Periods
            .Select(p => new RecordPeriodSnapshot(p.Id, p.Period, p.TimeZoneOffset, p.Description))
            .ToArray();
    }

    public RecordRemovedEvent BuildRemovedEvent(EventId eventId)
    {
        return new RecordRemovedEvent(eventId)
        {
            RecordId = Id
        };
    }
}
