using Freem.Domain.Import;
using Freem.Domain.Tags;
using Freem.Domain.Tags.Models;

namespace Freem.Api.Public.Contracts.DTO.Imports;

/// <summary>A tag found in the imported file.</summary>
public sealed class ImportTagResponse
{
    /// <summary>Key of the tag in the source file.</summary>
    public ImportTagId SourceId { get; }

    /// <summary>Tag name.</summary>
    public TagName Name { get; }

    /// <summary>Decision on the matching Freem tag, absent while not made.</summary>
    public ImportResolutionResponse<TagId>? Resolution { get; init; }

    public ImportTagResponse(ImportTagId sourceId, TagName name)
    {
        ArgumentNullException.ThrowIfNull(sourceId);
        ArgumentNullException.ThrowIfNull(name);

        SourceId = sourceId;
        Name = name;
    }
}
