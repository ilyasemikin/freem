using Freem.Domain.Events;
using Freem.Domain.Records;
using Freem.Domain.Records.Events.Created;
using Freem.Domain.Records.Events.Removed;
using Freem.Domain.Records.Events.Updated;
using Freem.Domain.Records.Models;
using Freem.Domain.Relations.Collections;
using Freem.Domain.Users;
using Freem.Time.Models;

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

    public DateTimePeriod Period { get; set; }

    public Record(
        RecordId id,
        UserId userId,
        RelatedActivityIdsCollection activities,
        RelatedTagIdsCollection tags,
        DateTimePeriod period)
    {
        ArgumentNullException.ThrowIfNull(activities);
        ArgumentNullException.ThrowIfNull(tags);
        ArgumentNullException.ThrowIfNull(period);

        Id = id;
        UserId = userId;
        
        Activities = activities;
        Tags = tags;
        
        Period = period;
    }

    public RecordCreatedEvent BuildCreatedEvent(EventId eventId)
    {
        return new RecordCreatedEvent(eventId, Id, UserId);
    }

    public RecordUpdatedEvent BuildUpdatedEvent(EventId eventId)
    {
        return new RecordUpdatedEvent(eventId, Id, UserId);
    }

    public RecordRemovedEvent BuildRemovedEvent(EventId eventId)
    {
        return new RecordRemovedEvent(eventId, Id, UserId);
    }
}
