using Freem.Api.Public.Contracts.DTO.Statistics.Periods.Abstractions;

namespace Freem.Api.Public.Contracts.DTO.Statistics;

/// <summary>Query for statistics over a single day, month or year.</summary>
public class StatisticsPerPeriodRequest
{
    /// <summary>The day, month or year to aggregate.</summary>
    public IUnitPeriod Period { get; }
    
    public StatisticsPerPeriodRequest(IUnitPeriod period)
    {
        ArgumentNullException.ThrowIfNull(period);
        
        Period = period;
    }
}