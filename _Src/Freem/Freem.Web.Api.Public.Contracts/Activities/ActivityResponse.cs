using Freem.Entities.Abstractions.Relations.Collection;
using Freem.Entities.Identifiers;
using Freem.Entities.Models.Activities;

namespace Freem.Web.Api.Public.Contracts.Activities;

public sealed class ActivityResponse
{
    public ActivityIdentifier Id { get; }
    public ActivityName Name { get; }
    public ActivityStatus Status { get; }
    
    public IReadOnlyRelatedEntitiesIdentifiersCollection<TagIdentifier> Tags { get; }

    public ActivityResponse(
        ActivityIdentifier id, ActivityName name, ActivityStatus status, 
        IReadOnlyRelatedEntitiesIdentifiersCollection<TagIdentifier> tags)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(status);
        ArgumentNullException.ThrowIfNull(tags);
        
        Id = id;
        Name = name;
        Status = status;
        Tags = tags;
    }
}