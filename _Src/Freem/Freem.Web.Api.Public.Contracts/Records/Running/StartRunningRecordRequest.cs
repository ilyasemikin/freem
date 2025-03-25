using Freem.Entities.Abstractions.Relations.Collection;
using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Records.Models;
using Freem.Entities.Relations.Collections;
using Freem.Entities.Tags.Identifiers;

namespace Freem.Web.Api.Public.Contracts.Records.Running;

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