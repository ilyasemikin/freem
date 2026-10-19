using Freem.Domain.Activities.Identifiers;
using Freem.Domain.Records.Identifiers;
using Freem.Domain.Records.Models;
using Freem.Domain.Relations.Collections;
using Freem.Domain.Tags.Identifiers;
using Freem.Time.Models;

namespace Freem.Web.Api.Public.Contracts.DTO.Records;

public sealed class RecordResponse
{
    public RecordIdentifier Id { get; }
    
    public RecordName? Name { get; init; }
    public RecordDescription? Description { get; init; }
    
    public DateTimePeriod Period { get; }
    
    public IReadOnlyRelatedEntitiesIdentifiersCollection<ActivityIdentifier> Activities { get; }
    public IReadOnlyRelatedEntitiesIdentifiersCollection<TagIdentifier> Tags { get; }

    public RecordResponse(
        RecordIdentifier id,
        DateTimePeriod period,
        IReadOnlyRelatedEntitiesIdentifiersCollection<ActivityIdentifier> activities,
        IReadOnlyRelatedEntitiesIdentifiersCollection<TagIdentifier>? tags = null)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(period);
        ArgumentNullException.ThrowIfNull(activities);
        
        Id = id;
        Period = period;
        Activities = activities;
        Tags = tags ?? RelatedTagsIdentifiersCollection.Empty;
    }
}