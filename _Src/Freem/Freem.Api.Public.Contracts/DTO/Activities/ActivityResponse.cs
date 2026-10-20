using Freem.Domain.Activities;
using Freem.Domain.Activities.Models;
using Freem.Domain.Relations.Collections;
using Freem.Domain.Tags;

namespace Freem.Api.Public.Contracts.DTO.Activities;

/// <summary>An activity.</summary>
public sealed class ActivityResponse
{
    /// <summary>Activity id.</summary>
    public ActivityId Id { get; }
    /// <summary>Activity name.</summary>
    public ActivityName Name { get; }
    /// <summary>Whether the activity is active or archived.</summary>
    public ActivityStatus Status { get; }
    
    /// <summary>Ids of the tags attached to the activity.</summary>
    public IReadOnlyRelatedIdsCollection<TagId> Tags { get; }

    public ActivityResponse(
        ActivityId id, ActivityName name, ActivityStatus status, 
        IReadOnlyRelatedIdsCollection<TagId> tags)
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