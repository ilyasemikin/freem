using Freem.Entities.Statistics.Time;

namespace Freem.Web.Api.Public.Contracts.DTO.Statistics;

public sealed class StatisticsPerDaysResponse
{
    public IReadOnlyDictionary<DateOnly, TimeStatistics> Statistics { get; }

    public StatisticsPerDaysResponse(IReadOnlyDictionary<DateOnly, TimeStatistics> statistics)
    {
        ArgumentNullException.ThrowIfNull(statistics);
        
        Statistics = statistics;
    }
}