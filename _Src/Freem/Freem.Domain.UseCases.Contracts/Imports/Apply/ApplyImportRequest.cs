using Freem.Domain.Import;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Domain.UseCases.Contracts.Imports.Apply;

public sealed class ApplyImportRequest : IRequest
{
    public RequestType Type => RequestTypes.Imports.Apply;

    public ImportDocumentId DocumentId { get; }

    /// <summary>
    /// Selected rows; null selects all rows of the import.
    /// </summary>
    public IReadOnlyList<ImportRowId>? RowIds { get; }

    private ApplyImportRequest(ImportDocumentId documentId, IReadOnlyList<ImportRowId>? rowIds)
    {
        ArgumentNullException.ThrowIfNull(documentId);

        DocumentId = documentId;
        RowIds = rowIds;
    }

    public static ApplyImportRequest ForAllRows(ImportDocumentId documentId)
    {
        return new ApplyImportRequest(documentId, null);
    }

    public static ApplyImportRequest ForRows(ImportDocumentId documentId, IReadOnlyList<ImportRowId> rowIds)
    {
        ArgumentNullException.ThrowIfNull(rowIds);

        return new ApplyImportRequest(documentId, rowIds);
    }
}
