using Freem.Domain.Activities.Models;
using Freem.Domain.Relations.Collections;
using Freem.Domain.Tags.Identifiers;

namespace Freem.Web.Api.Public.Contracts.DTO.Activities;

public sealed class CreateActivityRequest
{
    public ActivityName Name { get; }
    
    public IReadOnlyRelatedEntitiesIdentifiersCollection<TagIdentifier> Tags { get; }
    
    public CreateActivityRequest(ActivityName name, IReadOnlyRelatedEntitiesIdentifiersCollection<TagIdentifier>? tags = null)
    {
        ArgumentNullException.ThrowIfNull(name);
        
        Name = name;
        Tags = tags ?? RelatedTagsIdentifiersCollection.Empty;
    }
}