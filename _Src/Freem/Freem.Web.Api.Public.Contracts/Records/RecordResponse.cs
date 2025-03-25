using Freem.Entities.Abstractions.Relations.Collection;
using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Records.Identifiers;
using Freem.Entities.Records.Models;
using Freem.Entities.Tags.Identifiers;

namespace Freem.Web.Api.Public.Contracts.Records;

public sealed class RecordResponse
{
    public RecordIdentifier Id { get; }
    
    public RecordName? Name { get; init; }
    public RecordDescription? Description { get; init; }
    
    public IReadOnlyRelatedEntitiesIdentifiersCollection<ActivityIdentifier> Activities { get; }
    public IReadOnlyRelatedEntitiesIdentifiersCollection<TagIdentifier> Tags { get; }

    public RecordResponse(
        RecordIdentifier id,
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