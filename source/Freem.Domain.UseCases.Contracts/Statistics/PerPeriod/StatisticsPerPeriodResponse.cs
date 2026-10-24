using Freem.Domain.Statistics.Time;

namespace Freem.Domain.UseCases.Contracts.Statistics.PerPeriod;

public sealed class StatisticsPerPeriodResponse
{
    public TimeStatistics Statistics { get; }

    public StatisticsPerPeriodResponse(TimeStatistics statistics)
    {
        ArgumentNullException.ThrowIfNull(statistics);

        Statistics = statistics;
    }
}