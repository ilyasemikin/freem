using Freem.Clones;
using Freem.Entities.Abstractions;
using Freem.Entities.Abstractions.Relations;
using Freem.Entities.Abstractions.Relations.Collection;
using Freem.Entities.Activities;
using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Common.Relations.Collections;
using Freem.Entities.Records.Identifiers;
using Freem.Entities.Records.Models;
using Freem.Entities.Tags;
using Freem.Entities.Tags.Identifiers;
using Freem.Entities.Users.Identifiers;
using Freem.Time.Models;

namespace Freem.Entities.Records;

public sealed class Record : 
    IEntity<RecordIdentifier>, 
    IEntityRelation<Activity, ActivityIdentifier>, 
    IEntityRelation<Tag, TagIdentifier>,
    ICloneable<Record>
{
    public RecordIdentifier Id { get; }
    public UserIdentifier UserId { get; }
    public RelatedActivitiesCollection Activities { get; }
    public RelatedTagsCollection Tags { get; }

    IReadOnlyRelatedEntitiesCollection<Activity, ActivityIdentifier> IEntityRelation<Activity, ActivityIdentifier>.RelatedEntities => Activities;
    IReadOnlyRelatedEntitiesCollection<Tag, TagIdentifier> IEntityRelation<Tag, TagIdentifier>.RelatedEntities => Tags;

    public RecordName? Name { get; set; }
    public RecordDescription? Description { get; set; }

    public DateTimePeriod Period { get; set; }

    public Record(
        RecordIdentifier id,
        UserIdentifier userId,
        RelatedActivitiesCollection activities,
        RelatedTagsCollection tags,
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

    public Record Clone()
    {
        return new Record(Id, UserId, Activities, Tags, Period)
        {
            Name = Name,
            Description = Description
        };
    }
}
