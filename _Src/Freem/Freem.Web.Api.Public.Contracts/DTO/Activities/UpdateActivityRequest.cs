using Freem.Entities.Abstractions.Relations.Collection;
using Freem.Entities.Activities.Models;
using Freem.Entities.Tags.Identifiers;
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