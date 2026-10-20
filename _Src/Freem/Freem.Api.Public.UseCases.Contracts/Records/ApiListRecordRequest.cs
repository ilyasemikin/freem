using Freem.Api.Public.Contracts.DTO.Records;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.Records;

public sealed class ApiListRecordRequest : IRequest
{
    public RequestType Type => RequestTypes.Records.List;

    public ListRecordRequest Query { get; }

    public ApiListRecordRequest(ListRecordRequest query)
    {
        ArgumentNullException.ThrowIfNull(query);

        Query = query;
    }
}
