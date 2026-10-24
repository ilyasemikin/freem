using Freem.Api.Public.Contracts.V1.Imports;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.Imports;

public sealed class ApiListImportRequest : IRequest
{
    public RequestType Type => RequestTypes.Imports.List;

    public ImportListRequest Query { get; }

    public ApiListImportRequest(ImportListRequest query)
    {
        ArgumentNullException.ThrowIfNull(query);

        Query = query;
    }
}
