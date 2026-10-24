using Freem.Domain.DataTransfer.Import;
using Freem.Domain.Storage.Models.Filters.Abstractions;
using Freem.Domain.Storage.Models.Filters.Models;

namespace Freem.Domain.Storage.Repositories.DataTransfer.Import;

public sealed class ImportDocumentTagByDocumentFilter : ILimitFilter, IOffsetFilter
{
    public required Limit Limit { get; init; }
    public required Offset Offset { get; init; }

    public ImportDocumentId DocumentId { get; }

    public ImportDocumentTagByDocumentFilter(ImportDocumentId documentId)
    {
        ArgumentNullException.ThrowIfNull(documentId);

        DocumentId = documentId;
    }
}
