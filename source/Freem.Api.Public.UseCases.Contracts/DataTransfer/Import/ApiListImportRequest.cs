using Freem.Api.Public.Contracts.V1.DataTransfer.Import;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.DataTransfer.Import;

public sealed class ApiListImportRequest : IRequest
{
    public RequestType Type => RequestTypes.DataTransfer.Import.List;

    public ImportListRequest Query { get; }

    public ApiListImportRequest(ImportListRequest query)
    {
        ArgumentNullException.ThrowIfNull(query);

        Query = query;
    }
}
