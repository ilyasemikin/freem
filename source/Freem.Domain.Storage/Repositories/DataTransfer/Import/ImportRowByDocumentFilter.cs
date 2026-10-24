using Freem.Domain.DataTransfer.Import;
using Freem.Domain.Storage.Models.Filters.Abstractions;
using Freem.Domain.Storage.Models.Filters.Models;

namespace Freem.Domain.Storage.Repositories.DataTransfer.Import;

public sealed class ImportRowByDocumentFilter : ILimitFilter, IOffsetFilter
{
    public required Limit Limit { get; init; }
    public required Offset Offset { get; init; }

    public ImportDocumentId DocumentId { get; }

    /// <summary>
    /// When set, only rows with these ids are returned.
    /// </summary>
    public IReadOnlyList<ImportRowId>? Ids { get; init; }

    public ImportRowByDocumentFilter(ImportDocumentId documentId)
    {
        ArgumentNullException.ThrowIfNull(documentId);

        DocumentId = documentId;
    }
}
