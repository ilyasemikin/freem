using Freem.Entities.Abstractions.Relations.Collection;
using Freem.Entities.Activities.Models;
using Freem.Entities.Relations.Collections;
using Freem.Entities.Tags.Identifiers;

namespace Freem.Web.Api.Public.Contracts.DTO.Activities;

public sealed class CreateActivityRequest
{
    public ActivityName Name { get; }
    
    public IReadOnlyRelatedEntitiesIdentifiersCollection<TagIdentifier> Tags { get; }
    
    public CreateActivityRequest(ActivityName name, IReadOnlyRelatedEntitiesIdentifiersCollection<TagIdentifier>? tags = null)
    {
        ArgumentNullException.ThrowIfNull(name);
        
        Name = name;
        Tags = tags ?? RelatedTagsCollection.Empty;
    }
}