using Freem.Entities.Statistics.Time;

namespace Freem.Web.Api.Public.Contracts.DTO.Statistics;

public sealed class StatisticsPerPeriodResponse
{
    public TimeStatistics Statistics { get; }
    
    public StatisticsPerPeriodResponse(TimeStatistics statistics)
    {
        Statistics = statistics;
    }
}