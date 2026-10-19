using Freem.Domain.Activities.Models;
using Freem.Domain.Relations.Collections;
using Freem.Domain.Tags.Identifiers;
using Freem.Web.Api.Public.Contracts.Models;

namespace Freem.Web.Api.Public.Contracts.DTO.Activities;

public sealed class UpdateActivityRequest
{
    public UpdateField<ActivityName>? Name { get; init; }
    public UpdateField<IReadOnlyRelatedEntitiesIdentifiersCollection<TagIdentifier>>? Tags { get; init; }

    public bool HasChanges()
    {
        return 
            Name is not null || 
            Tags is not null;
    }
}