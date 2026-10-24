using Freem.Domain.Tags;

namespace Freem.Domain.Entities.DataTransfer.Import;

public sealed class ImportDocumentTag
{
    public ImportTag Tag { get; }

    /// <summary>
    /// Existing Freem tag the import tag is mapped to; null when not mapped yet.
    /// </summary>
    public TagId? MappedTagId { get; }

    public ImportDocumentTag(ImportTag tag, TagId? mappedTagId = null)
    {
        ArgumentNullException.ThrowIfNull(tag);

        Tag = tag;
        MappedTagId = mappedTagId;
    }
}
