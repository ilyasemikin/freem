using Freem.Domain.DataTransfer.Import;
using Freem.Domain.Tags;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Domain.UseCases.Contracts.DataTransfer.Import.UpdateTagMapping;

public sealed class UpdateImportTagMappingRequest : IRequest
{
    public RequestType Type => RequestTypes.DataTransfer.Import.UpdateTagMapping;

    public ImportDocumentId DocumentId { get; }
    public ImportTagId ImportTagId { get; }

    /// <summary>
    /// Existing tag of the user; null removes the mapping.
    /// </summary>
    public TagId? TagId { get; }

    public UpdateImportTagMappingRequest(
        ImportDocumentId documentId,
        ImportTagId importTagId,
        TagId? tagId)
    {
        ArgumentNullException.ThrowIfNull(documentId);
        ArgumentNullException.ThrowIfNull(importTagId);

        DocumentId = documentId;
        ImportTagId = importTagId;
        TagId = tagId;
    }
}
