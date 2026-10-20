using Freem.Api.Public.Contracts.DTO.Statistics.Periods.Abstractions;

namespace Freem.Api.Public.Contracts.DTO.Statistics;

public class StatisticsPerPeriodRequest
{
    public IUnitPeriod Period { get; }
    
    public StatisticsPerPeriodRequest(IUnitPeriod period)
    {
        ArgumentNullException.ThrowIfNull(period);
        
        Period = period;
    }
}