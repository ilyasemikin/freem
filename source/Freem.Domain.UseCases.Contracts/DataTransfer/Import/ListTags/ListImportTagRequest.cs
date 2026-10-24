using Freem.Domain.DataTransfer.Import;
using Freem.Domain.UseCases.Contracts.Filter;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Domain.UseCases.Contracts.DataTransfer.Import.ListTags;

public sealed class ListImportTagRequest : IRequest
{
    public RequestType Type => RequestTypes.DataTransfer.Import.ListTags;

    public ImportDocumentId DocumentId { get; }
    public Limit Limit { get; }
    public Offset Offset { get; }

    public ListImportTagRequest(ImportDocumentId documentId)
        : this(documentId, Limit.DefaultValue, Offset.DefaultValue)
    {
    }

    public ListImportTagRequest(ImportDocumentId documentId, Limit limit, Offset offset)
    {
        ArgumentNullException.ThrowIfNull(documentId);

        DocumentId = documentId;
        Limit = limit;
        Offset = offset;
    }
}
