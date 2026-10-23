using Freem.Domain.Import;
using Freem.Domain.Storage.Models.Filters.Abstractions;
using Freem.Domain.Storage.Models.Filters.Models;

namespace Freem.Domain.Storage.Repositories.Imports;

public sealed class ImportRowByDocumentFilter : ILimitFilter, IOffsetFilter
{
    public required Limit Limit { get; init; }
    public required Offset Offset { get; init; }

    public ImportDocumentId DocumentId { get; }

    public ImportRowByDocumentFilter(ImportDocumentId documentId)
    {
        ArgumentNullException.ThrowIfNull(documentId);

        DocumentId = documentId;
    }
}
