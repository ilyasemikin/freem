using Freem.Domain.Activities.Models;
using Freem.Domain.Relations.Collections;
using Freem.Domain.Tags;
using Freem.Api.Public.Contracts.Models;

namespace Freem.Api.Public.Contracts.DTO.Activities;

public sealed class UpdateActivityRequest
{
    public UpdateField<ActivityName>? Name { get; init; }
    public UpdateField<IReadOnlyRelatedIdsCollection<TagId>>? Tags { get; init; }

    public bool HasChanges()
    {
        return 
            Name is not null || 
            Tags is not null;
    }
}