using Freem.Domain.Tags.Models;

namespace Freem.Api.Public.Contracts.V1.Tags;

/// <summary>Request body for updating a tag. Fields that are omitted stay unchanged.</summary>
public sealed class TagUpdateRequest
{
    /// <summary>New tag name.</summary>
    public UpdateField<TagName>? Name { get; init; }

    /// <summary>Whether at least one field is set.</summary>
    public bool HasChanges()
    {
        return Name is not null;
    }
}