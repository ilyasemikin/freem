using Freem.Domain.Events.Identifiers;
using Freem.Domain.Records.Events.Created;
using Freem.Domain.Records.Events.Removed;
using Freem.Domain.Records.Events.Updated;
using Freem.Domain.Records.Identifiers;
using Freem.Domain.Records.Models;
using Freem.Domain.Relations.Collections;
using Freem.Domain.Users.Identifiers;
using Freem.Time.Models;

namespace Freem.Domain.Entities.Records;

public sealed class Record : 
    IEntity<RecordIdentifier>
{
    public RecordIdentifier Id { get; }
    public UserIdentifier UserId { get; }
    public RelatedActivitiesIdentifiersCollection Activities { get; }
    public RelatedTagsIdentifiersCollection Tags { get; }

    public RecordName? Name { get; set; }
    public RecordDescription? Description { get; set; }

    public DateTimePeriod Period { get; set; }

    public Record(
        RecordIdentifier id,
        UserIdentifier userId,
        RelatedActivitiesIdentifiersCollection activities,
        RelatedTagsIdentifiersCollection tags,
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

    public RecordCreatedEvent BuildCreatedEvent(EventIdentifier eventId)
    {
        return new RecordCreatedEvent(eventId, Id, UserId);
    }

    public RecordUpdatedEvent BuildUpdatedEvent(EventIdentifier eventId)
    {
        return new RecordUpdatedEvent(eventId, Id, UserId);
    }

    public RecordRemovedEvent BuildRemovedEvent(EventIdentifier eventId)
    {
        return new RecordRemovedEvent(eventId, Id, UserId);
    }
}
