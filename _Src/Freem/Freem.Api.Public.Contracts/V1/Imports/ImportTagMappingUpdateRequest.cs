using Freem.Domain.Tags;

namespace Freem.Api.Public.Contracts.V1.Imports;

/// <summary>Request body for updating the mapping of an import tag to an existing tag of the user.</summary>
public sealed class ImportTagMappingUpdateRequest
{
    /// <summary>Existing tag; null removes the mapping.</summary>
    public TagId? TagId { get; }

    public ImportTagMappingUpdateRequest(TagId? tagId)
    {
        TagId = tagId;
    }
}
