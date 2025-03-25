using Freem.Entities.Abstractions.Relations.Collection;
using Freem.Entities.Activities.Models;
using Freem.Entities.Tags.Identifiers;

namespace Freem.Web.Api.Public.Contracts.Activities;

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