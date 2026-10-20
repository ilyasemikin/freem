using Freem.Web.Api.Public.Contracts.DTO.Statistics;
using Utilities.UseCases.Abstractions;
using Utilities.UseCases.Models;

namespace Freem.Web.Api.Public.UseCases.Contracts.Statistics;

public sealed class ApiStatisticsPerDaysRequest : IRequest
{
    public RequestType Type => RequestTypes.Statistics.PerDays;

    public StatisticsPerDaysRequest Query { get; }

    public ApiStatisticsPerDaysRequest(StatisticsPerDaysRequest query)
    {
        ArgumentNullException.ThrowIfNull(query);

        Query = query;
    }
}
