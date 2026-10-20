using Freem.Domain.Activities;
using Freem.Domain.Records.Models;
using Freem.Domain.Relations.Collections;
using Freem.Domain.Tags;

namespace Freem.Api.Public.Contracts.DTO.Records.Running;

/// <summary>Request body for starting a running record.</summary>
public sealed class StartRunningRecordRequest
{
    /// <summary>When tracking started. The current time when omitted.</summary>
    public DateTimeOffset? StartAt { get; }
    
    /// <summary>Optional record name.</summary>
    public RecordName? Name { get; init; }
    /// <summary>Optional record description.</summary>
    public RecordDescription? Description { get; init; }
    
    /// <summary>Ids of the activities to track.</summary>
    public IReadOnlyRelatedIdsCollection<ActivityId> Activities { get; }
    /// <summary>Ids of the tags to attach. Empty when omitted.</summary>
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