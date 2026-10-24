using Freem.Domain.Tags;

namespace Freem.Api.Public.Contracts.DTO.Imports;

/// <summary>Request body for updating the mapping of an import tag to an existing tag of the user.</summary>
public sealed class UpdateImportTagMappingRequest
{
    /// <summary>Existing tag; null removes the mapping.</summary>
    public TagId? TagId { get; }

    public UpdateImportTagMappingRequest(TagId? tagId)
    {
        TagId = tagId;
    }
}
