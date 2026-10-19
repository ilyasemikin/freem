using Freem.Domain.Activities;
using Freem.Domain.Records.Models;
using Freem.Domain.Relations.Collections;
using Freem.Domain.RunningRecords;
using Freem.Domain.Tags;

namespace Freem.Web.Api.Public.Contracts.DTO.Records.Running;

public sealed class RunningRecordResponse
{
    public RunningRecordId Id { get; }
    
    public DateTimeOffset StartAt { get; }
    
    public RecordName? Name { get; init; }
    public RecordDescription? Description { get; init; }
    
    public IReadOnlyRelatedIdsCollection<ActivityId> Activities { get; }
    public IReadOnlyRelatedIdsCollection<TagId> Tags { get; }
    
    public RunningRecordResponse(
        RunningRecordId id, 
        DateTimeOffset startAt,
        IReadOnlyRelatedIdsCollection<ActivityId> activities, 
        IReadOnlyRelatedIdsCollection<TagId> tags)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(activities);
        ArgumentNullException.ThrowIfNull(tags);
        
        Id = id;
        StartAt = startAt;
        Activities = activities;
        Tags = tags;
    }
}