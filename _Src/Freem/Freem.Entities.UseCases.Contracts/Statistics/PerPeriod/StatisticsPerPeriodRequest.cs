using Freem.Entities.UseCases.Contracts.Statistics.PerPeriod.Periods.Abstractions;

namespace Freem.Entities.UseCases.Contracts.Statistics.PerPeriod;

public sealed class StatisticsPerPeriodRequest
{
    public IStatisticsPeriod Period { get; }

    public StatisticsPerPeriodRequest(IStatisticsPeriod period)
    {
        ArgumentNullException.ThrowIfNull(period);
        
        Period = period;
    }
}