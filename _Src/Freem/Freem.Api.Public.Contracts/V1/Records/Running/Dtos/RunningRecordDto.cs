using Freem.Domain.Activities;
using Freem.Domain.Records.Models;
using Freem.Domain.Relations.Collections;
using Freem.Domain.RunningRecords;
using Freem.Domain.Tags;

namespace Freem.Api.Public.Contracts.V1.Records.Running.Dtos;

/// <summary>The record that is being tracked right now.</summary>
public sealed class RunningRecordDto
{
    /// <summary>Running record id; derived from the user id.</summary>
    public RunningRecordId Id { get; }
    
    /// <summary>When tracking started.</summary>
    public DateTimeOffset StartAt { get; }
    
    /// <summary>Record name, if set.</summary>
    public RecordName? Name { get; init; }
    /// <summary>Record description, if set.</summary>
    public RecordDescription? Description { get; init; }
    
    /// <summary>Ids of the activities being tracked.</summary>
    public IReadOnlyRelatedIdsCollection<ActivityId> Activities { get; }
    /// <summary>Ids of the attached tags.</summary>
    public IReadOnlyRelatedIdsCollection<TagId> Tags { get; }
    
    public RunningRecordDto(
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