using Freem.Domain.Import;
using Freem.Domain.UseCases.Contracts.Filter;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Domain.UseCases.Contracts.Imports.ListRows;

public sealed class ListImportRowRequest : IRequest
{
    public RequestType Type => RequestTypes.Imports.ListRows;

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
