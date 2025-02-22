using Freem.Entities.Abstractions.Relations.Collection;
using Freem.Entities.Identifiers;
using Freem.Entities.Models.Records;

namespace Freem.Web.Api.Public.Contracts.Records.Running;

public sealed class RunningRecordResponse
{
    public RunningRecordIdentifier Id { get; }
    
    public RecordName? Name { get; init; }
    public RecordDescription? Description { get; init; }
    
    public IReadOnlyRelatedEntitiesIdentifiersCollection<ActivityIdentifier> Activities { get; }
    public IReadOnlyRelatedEntitiesIdentifiersCollection<TagIdentifier> Tags { get; }
    
    public RunningRecordResponse(
        RunningRecordIdentifier id, 
        IReadOnlyRelatedEntitiesIdentifiersCollection<ActivityIdentifier> activities, 
        IReadOnlyRelatedEntitiesIdentifiersCollection<TagIdentifier> tags)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(activities);
        ArgumentNullException.ThrowIfNull(tags);
        
        Id = id;
        Activities = activities;
        Tags = tags;
    }
}