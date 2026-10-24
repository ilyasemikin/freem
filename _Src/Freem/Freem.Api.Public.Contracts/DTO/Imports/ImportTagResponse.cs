using Freem.Domain.Import;
using Freem.Domain.Tags;
using Freem.Domain.Tags.Models;

namespace Freem.Api.Public.Contracts.DTO.Imports;

/// <summary>An import tag: a tag found in the imported file.</summary>
public sealed class ImportTagResponse
{
    /// <summary>Key of the tag in the imported file.</summary>
    public ImportTagId Id { get; }

    /// <summary>Tag name.</summary>
    public TagName Name { get; }

    /// <summary>Existing Freem tag the import tag is mapped to, absent while not mapped.</summary>
    public TagId? MappedTagId { get; init; }

    public ImportTagResponse(ImportTagId id, TagName name)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(name);

        Id = id;
        Name = name;
    }
}
