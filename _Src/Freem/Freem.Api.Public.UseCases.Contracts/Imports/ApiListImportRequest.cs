using Freem.Api.Public.Contracts.DTO.Imports;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.Imports;

public sealed class ApiListImportRequest : IRequest
{
    public RequestType Type => RequestTypes.Imports.List;

    public ListImportRequest Query { get; }

    public ApiListImportRequest(ListImportRequest query)
    {
        ArgumentNullException.ThrowIfNull(query);

        Query = query;
    }
}
