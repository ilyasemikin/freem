using Freem.Domain.Activities;
using Freem.Domain.Records.Models;
using Freem.Domain.Relations.Collections;
using Freem.Domain.Tags;

namespace Freem.Api.Public.Contracts.DTO.Records.Running;

public sealed class StartRunningRecordRequest
{
    public DateTimeOffset? StartAt { get; }
    
    public RecordName? Name { get; init; }
    public RecordDescription? Description { get; init; }
    
    public IReadOnlyRelatedIdsCollection<ActivityId> Activities { get; }
    public IReadOnlyRelatedIdsCollection<TagId> Tags { get; }
    
    public StartRunningRecordRequest(
        DateTimeOffset? startAt, 
        IReadOnlyRelatedIdsCollection<ActivityId> activities, 
        IReadOnlyRelatedIdsCollection<TagId>? tags = null)
    {
        ArgumentNullException.ThrowIfNull(activities);
        
        StartAt = startAt;
        Activities = activities;
        Tags = tags ?? RelatedTagIdsCollection.Empty;
    }
}