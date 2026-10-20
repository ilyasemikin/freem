using Freem.Domain.Statistics.Time;

namespace Freem.Api.Public.Contracts.DTO.Statistics;

public sealed class StatisticsPerPeriodResponse
{
    public TimeStatistics Statistics { get; }
    
    public StatisticsPerPeriodResponse(TimeStatistics statistics)
    {
        Statistics = statistics;
    }
}