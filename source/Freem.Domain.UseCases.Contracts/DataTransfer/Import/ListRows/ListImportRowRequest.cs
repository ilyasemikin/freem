using Freem.Domain.DataTransfer.Import;
using Freem.Domain.UseCases.Contracts.Filter;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Domain.UseCases.Contracts.DataTransfer.Import.ListRows;

public sealed class ListImportRowRequest : IRequest
{
    public RequestType Type => RequestTypes.DataTransfer.Import.ListRows;

    public ImportDocumentId DocumentId { get; }
    public Limit Limit { get; }
    public Offset Offset { get; }

    public ListImportRowRequest(ImportDocumentId documentId)
        : this(documentId, Limit.DefaultValue, Offset.DefaultValue)
    {
    }

    public ListImportRowRequest(ImportDocumentId documentId, Limit limit, Offset offset)
    {
        ArgumentNullException.ThrowIfNull(documentId);

        DocumentId = documentId;
        Limit = limit;
        Offset = offset;
    }
}
