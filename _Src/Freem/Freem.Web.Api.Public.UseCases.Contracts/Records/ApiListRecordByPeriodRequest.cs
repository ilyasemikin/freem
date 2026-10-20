using Freem.Web.Api.Public.Contracts.DTO.Records;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Web.Api.Public.UseCases.Contracts.Records;

public sealed class ApiListRecordByPeriodRequest : IRequest
{
    public RequestType Type => RequestTypes.Records.ListByPeriod;

    public ListRecordByPeriodRequest Query { get; }

    public ApiListRecordByPeriodRequest(ListRecordByPeriodRequest query)
    {
        ArgumentNullException.ThrowIfNull(query);

        Query = query;
    }
}
