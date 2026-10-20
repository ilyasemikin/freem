using Freem.Domain.Activities.Models;
using Freem.Domain.Relations.Collections;
using Freem.Domain.Tags;

namespace Freem.Api.Public.Contracts.DTO.Activities;

/// <summary>Request body for creating an activity.</summary>
public sealed class CreateActivityRequest
{
    /// <summary>Activity name.</summary>
    public ActivityName Name { get; }
    
    /// <summary>Ids of the tags to attach. Empty when omitted.</summary>
    public IReadOnlyRelatedIdsCollection<TagId> Tags { get; }
    
    public CreateActivityRequest(ActivityName name, IReadOnlyRelatedIdsCollection<TagId>? tags = null)
    {
        ArgumentNullException.ThrowIfNull(name);
        
        Name = name;
        Tags = tags ?? RelatedTagIdsCollection.Empty;
    }
}