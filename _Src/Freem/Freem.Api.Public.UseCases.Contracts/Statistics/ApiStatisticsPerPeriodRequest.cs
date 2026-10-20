using Freem.Api.Public.Contracts.DTO.Statistics;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Api.Public.UseCases.Contracts.Statistics;

public sealed class ApiStatisticsPerPeriodRequest : IRequest
{
    public RequestType Type => RequestTypes.Statistics.PerPeriod;

    public StatisticsPerPeriodRequest Query { get; }

    public ApiStatisticsPerPeriodRequest(StatisticsPerPeriodRequest query)
    {
        ArgumentNullException.ThrowIfNull(query);

        Query = query;
    }
}
