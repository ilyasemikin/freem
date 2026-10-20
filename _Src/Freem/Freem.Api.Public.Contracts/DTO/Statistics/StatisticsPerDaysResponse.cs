using Freem.Domain.Statistics.Time;

namespace Freem.Api.Public.Contracts.DTO.Statistics;

/// <summary>Per-day statistics.</summary>
public sealed class StatisticsPerDaysResponse
{
    /// <summary>Statistics keyed by day.</summary>
    public IReadOnlyDictionary<DateOnly, TimeStatistics> Statistics { get; }

    public StatisticsPerDaysResponse(IReadOnlyDictionary<DateOnly, TimeStatistics> statistics)
    {
        ArgumentNullException.ThrowIfNull(statistics);
        
        Statistics = statistics;
    }
}