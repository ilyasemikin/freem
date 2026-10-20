using Freem.Domain.Statistics.Time;

namespace Freem.Domain.UseCases.Contracts.Statistics.PerDays;

public sealed class StatisticsPerDaysResponse
{
    public IReadOnlyDictionary<DateOnly, TimeStatistics> Statistics { get; }

    public StatisticsPerDaysResponse(IReadOnlyDictionary<DateOnly, TimeStatistics> statistics)
    {
        ArgumentNullException.ThrowIfNull(statistics);

        Statistics = statistics;
    }
}