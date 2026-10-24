using Freem.Api.Public.Contracts.V1.Records;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.Records;

public sealed class ApiListRecordRequest : IRequest
{
    public RequestType Type => RequestTypes.Records.List;

    public RecordListRequest Query { get; }

    public ApiListRecordRequest(RecordListRequest query)
    {
        ArgumentNullException.ThrowIfNull(query);

        Query = query;
    }
}
