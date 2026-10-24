using Freem.Api.Public.Contracts.V1.DataTransfer.Import;
using Freem.Domain.DataTransfer.Import;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.DataTransfer.Import;

public sealed class ApiListImportRowRequest : IRequest
{
    public RequestType Type => RequestTypes.DataTransfer.Import.ListRows;

    public ImportDocumentId Id { get; }
    public ImportRowListRequest Query { get; }

    public ApiListImportRowRequest(ImportDocumentId id, ImportRowListRequest query)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(query);

        Id = id;
        Query = query;
    }
}
