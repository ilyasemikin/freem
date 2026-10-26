using Freem.Api.Public.Contracts.V1.Statistics.Periods.Abstractions;

namespace Freem.Api.Public.Contracts.V1.Statistics;

/// <summary>Query for statistics over a single day, week, month or year.</summary>
public class StatisticsPerPeriodRequest
{
    /// <summary>The day, week, month or year to aggregate.</summary>
    public IUnitPeriod Period { get; }
    
    public StatisticsPerPeriodRequest(IUnitPeriod period)
    {
        ArgumentNullException.ThrowIfNull(period);
        
        Period = period;
    }
}