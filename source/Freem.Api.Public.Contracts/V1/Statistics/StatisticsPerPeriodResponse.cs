using Freem.Domain.Statistics.Time;

namespace Freem.Api.Public.Contracts.V1.Statistics;

/// <summary>Statistics over a single period.</summary>
public sealed class StatisticsPerPeriodResponse
{
    /// <summary>Time spent over the period.</summary>
    public TimeStatistics Statistics { get; }
    
    public StatisticsPerPeriodResponse(TimeStatistics statistics)
    {
        Statistics = statistics;
    }
}