using Freem.Domain.Activities.Models;
using Freem.Domain.Relations.Collections;
using Freem.Domain.Tags;

namespace Freem.Api.Public.Contracts.V1.Activities;

/// <summary>Request body for updating an activity. Fields that are omitted stay unchanged.</summary>
public sealed class ActivityUpdateRequest
{
    /// <summary>New activity name.</summary>
    public UpdateField<ActivityName>? Name { get; init; }
    /// <summary>New activity color as a <c>#rrggbb</c> hex string.</summary>
    public UpdateField<ActivityColor>? Color { get; init; }
    /// <summary>New full set of tag ids; replaces the current one.</summary>
    public UpdateField<IReadOnlyRelatedIdsCollection<TagId>>? Tags { get; init; }

    /// <summary>Whether at least one field is set.</summary>
    public bool HasChanges()
    {
        return 
            Name is not null || 
            Color is not null || 
            Tags is not null;
    }
}