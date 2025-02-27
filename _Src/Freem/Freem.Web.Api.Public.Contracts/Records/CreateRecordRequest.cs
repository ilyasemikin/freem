using Freem.Entities.Abstractions.Relations.Collection;
using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Records.Models;
using Freem.Entities.Tags.Identifiers;
using Freem.Time.Models;

namespace Freem.Web.Api.Public.Contracts.Records;

public sealed class CreateRecordRequest
{
    public DateTimePeriod Period { get; }
    
    public RecordName? Name { get; init; }
    public RecordDescription? Description { get; init; }
    
    public IReadOnlyRelatedEntitiesIdentifiersCollection<ActivityIdentifier> Activities { get; }
    public IReadOnlyRelatedEntitiesIdentifiersCollection<TagIdentifier> Tags { get; }

    public CreateRecordRequest(
        DateTimePeriod period, 
        IReadOnlyRelatedEntitiesIdentifiersCollection<ActivityIdentifier> activities,
        IReadOnlyRelatedEntitiesIdentifiersCollection<TagIdentifier> tags)
    {
        ArgumentNullException.ThrowIfNull(period);
        ArgumentNullException.ThrowIfNull(activities);
        ArgumentNullException.ThrowIfNull(tags);
        
        Period = period;
        Activities = activities;
        Tags = tags;
    }
}