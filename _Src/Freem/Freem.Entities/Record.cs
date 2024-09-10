using Freem.Entities.Abstractions;
using Freem.Entities.Abstractions.Relations.Collection;
using Freem.Entities.Identifiers;
using Freem.Entities.Relations.Collections;
using Freem.Time.Models;

namespace Freem.Entities;

public class Record : 
    IEntity<RecordIdentifier>, 
    IEntityRelation<Activity, ActivityIdentifier>, 
    IEntityRelation<Tag, TagIdentifier>
{
    public const int MaxNameLength = 128;
    public const int MaxDescriptionLength = 1024;

    private string? _name;
    private string? _description;

    public RecordIdentifier Id { get; }
    public UserIdentifier UserId { get; }
    public RelatedActivitiesCollection Activities { get; }
    public RelatedTagsCollection Tags { get; }

    IReadOnlyRelatedEntitiesCollection<Activity, ActivityIdentifier> IEntityRelation<Activity, ActivityIdentifier>.RelatedEntities => Activities;
    IReadOnlyRelatedEntitiesCollection<Tag, TagIdentifier> IEntityRelation<Tag, TagIdentifier>.RelatedEntities => Tags;

    public string? Name
    {
        get => _name;
        set
        {
            if (value?.Length > MaxNameLength)
                throw new ArgumentException($"Length cannot be more than {MaxNameLength}", nameof(value));

            _name = value;
        }
    }

    public string? Description
    {
        get => _description;
        set
        {
            if (value?.Length > MaxDescriptionLength)
                throw new ArgumentException($"Length cannot be more than {MaxDescriptionLength}", nameof(value));

            _description = value;
        }
    }

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
}
