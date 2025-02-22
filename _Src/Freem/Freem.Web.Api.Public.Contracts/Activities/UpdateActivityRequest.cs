using Freem.Entities.Abstractions.Relations.Collection;
using Freem.Entities.Identifiers;
using Freem.Entities.Models.Activities;

namespace Freem.Web.Api.Public.Contracts.Activities;

public sealed class UpdateActivityRequest
{
    public UpdateField<ActivityName>? Name { get; init; }
    public UpdateField<ActivityStatus>? Status { get; init; }
    
    public UpdateField<IReadOnlyRelatedEntitiesIdentifiersCollection<TagIdentifier>>? Tags { get; init; }

    public bool HasChanges()
    {
        return 
            Name is not null || 
            Status is not null || 
            Tags is not null;
    }
}