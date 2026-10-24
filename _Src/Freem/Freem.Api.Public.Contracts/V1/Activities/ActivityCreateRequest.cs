using Freem.Domain.Activities.Models;
using Freem.Domain.Relations.Collections;
using Freem.Domain.Tags;

namespace Freem.Api.Public.Contracts.V1.Activities;

/// <summary>Request body for creating an activity.</summary>
public sealed class ActivityCreateRequest
{
    /// <summary>Activity name.</summary>
    public ActivityName Name { get; }

    /// <summary>Activity color as a <c>#rrggbb</c> hex string.</summary>
    public ActivityColor Color { get; }
    
    /// <summary>Ids of the tags to attach. Empty when omitted.</summary>
    public IReadOnlyRelatedIdsCollection<TagId> Tags { get; }
    
    public ActivityCreateRequest(
        ActivityName name, ActivityColor color, IReadOnlyRelatedIdsCollection<TagId>? tags = null)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(color);
        
        Name = name;
        Color = color;
        Tags = tags ?? RelatedTagIdsCollection.Empty;
    }
}