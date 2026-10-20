using Freem.Domain.Tags.Models;
using Freem.Api.Public.Contracts.Models;

namespace Freem.Api.Public.Contracts.DTO.Tags;

/// <summary>Request body for updating a tag. Fields that are omitted stay unchanged.</summary>
public sealed class UpdateTagRequest
{
    /// <summary>New tag name.</summary>
    public UpdateField<TagName>? Name { get; init; }

    /// <summary>Whether at least one field is set.</summary>
    public bool HasChanges()
    {
        return Name is not null;
    }
}