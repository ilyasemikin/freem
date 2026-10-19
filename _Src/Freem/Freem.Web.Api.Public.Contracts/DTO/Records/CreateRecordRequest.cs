using Freem.Domain.Activities.Identifiers;
using Freem.Domain.Records.Models;
using Freem.Domain.Relations.Collections;
using Freem.Domain.Tags.Identifiers;
using Freem.Time.Models;

namespace Freem.Web.Api.Public.Contracts.DTO.Records;

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
        IReadOnlyRelatedEntitiesIdentifiersCollection<TagIdentifier>? tags = null)
    {
        ArgumentNullException.ThrowIfNull(period);
        ArgumentNullException.ThrowIfNull(activities);
        
        Period = period;
        Activities = activities;
        Tags = tags ?? RelatedTagsIdentifiersCollection.Empty;
    }
}