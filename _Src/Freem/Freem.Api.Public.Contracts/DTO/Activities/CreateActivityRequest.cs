using Freem.Domain.Activities.Models;
using Freem.Domain.Relations.Collections;
using Freem.Domain.Tags;

namespace Freem.Api.Public.Contracts.DTO.Activities;

public sealed class CreateActivityRequest
{
    public ActivityName Name { get; }
    
    public IReadOnlyRelatedIdsCollection<TagId> Tags { get; }
    
    public CreateActivityRequest(ActivityName name, IReadOnlyRelatedIdsCollection<TagId>? tags = null)
    {
        ArgumentNullException.ThrowIfNull(name);
        
        Name = name;
        Tags = tags ?? RelatedTagIdsCollection.Empty;
    }
}