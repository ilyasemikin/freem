using Freem.Domain.Activities.Identifiers;
using Freem.Domain.Activities.Models;
using Freem.Domain.Relations.Collections;

namespace Freem.Entities.UseCases.Contracts.Activities.Update;

public sealed class UpdateActivityRequest
{
    public ActivityIdentifier Id { get; }
    
    public UpdateField<ActivityName>? Name { get; init; }
    
    public UpdateField<RelatedTagsIdentifiersCollection>? Tags { get; init; }

    public UpdateActivityRequest(ActivityIdentifier id)
    {
        ArgumentNullException.ThrowIfNull(id);

        Id = id;
    }

    public bool HasChanges()
    {
        return Name is not null || Tags is not null;
    }
}