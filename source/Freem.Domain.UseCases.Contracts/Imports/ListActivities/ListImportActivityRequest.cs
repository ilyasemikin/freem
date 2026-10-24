using Freem.Domain.Import;
using Freem.Domain.UseCases.Contracts.Filter;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Domain.UseCases.Contracts.Imports.ListActivities;

public sealed class ListImportActivityRequest : IRequest
{
    public RequestType Type => RequestTypes.Imports.ListActivities;

    public ImportDocumentId DocumentId { get; }
    public Limit Limit { get; }
    public Offset Offset { get; }

    public ListImportActivityRequest(ImportDocumentId documentId)
        : this(documentId, Limit.DefaultValue, Offset.DefaultValue)
    {
    }

    public ListImportActivityRequest(ImportDocumentId documentId, Limit limit, Offset offset)
    {
        ArgumentNullException.ThrowIfNull(documentId);

        DocumentId = documentId;
        Limit = limit;
        Offset = offset;
    }
}
