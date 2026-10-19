using Freem.Domain.Activities;
using Freem.Domain.Activities.Models;
using Freem.Domain.Relations.Collections;
using Freem.Domain.Tags;

namespace Freem.Web.Api.Public.Contracts.DTO.Activities;

public sealed class ActivityResponse
{
    public ActivityId Id { get; }
    public ActivityName Name { get; }
    public ActivityStatus Status { get; }
    
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