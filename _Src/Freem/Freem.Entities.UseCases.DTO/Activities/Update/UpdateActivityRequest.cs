using Freem.Entities.Activities.Identifiers;
using Freem.Entities.Activities.Models;
using Freem.Entities.Common.Relations.Collections;
using Freem.Entities.UseCases.DTO.Abstractions.Models;

namespace Freem.Entities.UseCases.DTO.Activities.Update;

public sealed class UpdateActivityRequest
{
    public ActivityIdentifier Id { get; }
    
    public UpdateField<ActivityName>? Name { get; init; }
    
    public UpdateField<RelatedTagsCollection>? Tags { get; init; }

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