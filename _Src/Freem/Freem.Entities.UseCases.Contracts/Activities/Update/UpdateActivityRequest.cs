using Freem.Domain.Activities;
using Freem.Domain.Activities.Models;
using Freem.Domain.Relations.Collections;

namespace Freem.Entities.UseCases.Contracts.Activities.Update;

public sealed class UpdateActivityRequest
{
    public ActivityId Id { get; }
    
    public UpdateField<ActivityName>? Name { get; init; }
    
    public UpdateField<RelatedTagIdsCollection>? Tags { get; init; }

    public UpdateActivityRequest(ActivityId id)
    {
        ArgumentNullException.ThrowIfNull(id);

        Id = id;
    }

    public bool HasChanges()
    {
        return Name is not null || Tags is not null;
    }
}