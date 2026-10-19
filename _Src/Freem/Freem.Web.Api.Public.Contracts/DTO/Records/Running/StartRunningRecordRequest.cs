using Freem.Domain.Activities.Identifiers;
using Freem.Domain.Records.Models;
using Freem.Domain.Relations.Collections;
using Freem.Domain.Tags.Identifiers;

namespace Freem.Web.Api.Public.Contracts.DTO.Records.Running;

public sealed class StartRunningRecordRequest
{
    public DateTimeOffset? StartAt { get; }
    
    public RecordName? Name { get; init; }
    public RecordDescription? Description { get; init; }
    
    public IReadOnlyRelatedEntitiesIdentifiersCollection<ActivityIdentifier> Activities { get; }
    public IReadOnlyRelatedEntitiesIdentifiersCollection<TagIdentifier> Tags { get; }
    
    public StartRunningRecordRequest(
        DateTimeOffset? startAt, 
        IReadOnlyRelatedEntitiesIdentifiersCollection<ActivityIdentifier> activities, 
        IReadOnlyRelatedEntitiesIdentifiersCollection<TagIdentifier>? tags = null)
    {
        ArgumentNullException.ThrowIfNull(activities);
        
        StartAt = startAt;
        Activities = activities;
        Tags = tags ?? RelatedTagsIdentifiersCollection.Empty;
    }
}