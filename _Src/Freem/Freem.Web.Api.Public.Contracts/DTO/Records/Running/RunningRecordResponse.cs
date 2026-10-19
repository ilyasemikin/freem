using Freem.Domain.Activities.Identifiers;
using Freem.Domain.Records.Models;
using Freem.Domain.Relations.Collections;
using Freem.Domain.RunningRecords.Identifiers;
using Freem.Domain.Tags.Identifiers;

namespace Freem.Web.Api.Public.Contracts.DTO.Records.Running;

public sealed class RunningRecordResponse
{
    public RunningRecordIdentifier Id { get; }
    
    public DateTimeOffset StartAt { get; }
    
    public RecordName? Name { get; init; }
    public RecordDescription? Description { get; init; }
    
    public IReadOnlyRelatedEntitiesIdentifiersCollection<ActivityIdentifier> Activities { get; }
    public IReadOnlyRelatedEntitiesIdentifiersCollection<TagIdentifier> Tags { get; }
    
    public RunningRecordResponse(
        RunningRecordIdentifier id, 
        DateTimeOffset startAt,
        IReadOnlyRelatedEntitiesIdentifiersCollection<ActivityIdentifier> activities, 
        IReadOnlyRelatedEntitiesIdentifiersCollection<TagIdentifier> tags)
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