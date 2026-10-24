using Freem.Api.Public.Contracts.V1.DataTransfer.Import;
using Freem.Domain.DataTransfer.Import;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.DataTransfer.Import;

public sealed class ApiListImportTagRequest : IRequest
{
    public RequestType Type => RequestTypes.DataTransfer.Import.ListTags;

    public ImportDocumentId Id { get; }
    public ImportTagListRequest Query { get; }

    public ApiListImportTagRequest(ImportDocumentId id, ImportTagListRequest query)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(query);

        Id = id;
        Query = query;
    }
}
