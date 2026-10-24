using Freem.Api.Public.Contracts.V1.Records;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.Records;

public sealed class ApiListRecordByPeriodRequest : IRequest
{
    public RequestType Type => RequestTypes.Records.ListByPeriod;

    public RecordListByPeriodRequest Query { get; }

    public ApiListRecordByPeriodRequest(RecordListByPeriodRequest query)
    {
        ArgumentNullException.ThrowIfNull(query);

        Query = query;
    }
}
