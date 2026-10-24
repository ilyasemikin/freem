using Freem.Domain.Activities;
using Freem.Domain.Activities.Models;
using Freem.Domain.Relations.Collections;
using Freem.Domain.Tags;

namespace Freem.Api.Public.Contracts.V1.Activities.Dtos;

/// <summary>An activity.</summary>
public sealed class ActivityDto
{
    /// <summary>Activity id.</summary>
    public ActivityId Id { get; }
    /// <summary>Activity name.</summary>
    public ActivityName Name { get; }
    /// <summary>Activity color as a <c>#rrggbb</c> hex string.</summary>
    public ActivityColor Color { get; }
    /// <summary>Whether the activity is active or archived.</summary>
    public ActivityStatus Status { get; }
    
    /// <summary>Ids of the tags attached to the activity.</summary>
    public IReadOnlyRelatedIdsCollection<TagId> Tags { get; }

    public ActivityDto(
        ActivityId id, ActivityName name, ActivityColor color, ActivityStatus status, 
        IReadOnlyRelatedIdsCollection<TagId> tags)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(color);
        ArgumentNullException.ThrowIfNull(status);
        ArgumentNullException.ThrowIfNull(tags);
        
        Id = id;
        Name = name;
        Color = color;
        Status = status;
        Tags = tags;
    }
}