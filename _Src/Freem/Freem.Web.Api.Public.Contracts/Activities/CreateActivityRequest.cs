using Freem.Entities.Abstractions.Relations.Collection;
using Freem.Entities.Identifiers;
using Freem.Entities.Models.Activities;

namespace Freem.Web.Api.Public.Contracts.Activities;

public sealed class CreateActivityRequest
{
    public ActivityName Name { get; }
    
    public IReadOnlyRelatedEntitiesIdentifiersCollection<TagIdentifier> Tags { get; }
    
    public CreateActivityRequest(ActivityName name, IReadOnlyRelatedEntitiesIdentifiersCollection<TagIdentifier> tags)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(tags);
        
        Name = name;
        Tags = tags;
    }
}