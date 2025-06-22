using Freem.Web.Api.Public.Contracts.DTO.Statistics.Periods.Abstractions;

namespace Freem.Web.Api.Public.Contracts.DTO.Statistics;

public class StatisticsPerPeriodRequest
{
    public IUnitPeriod Period { get; }
    
    public StatisticsPerPeriodRequest(IUnitPeriod period)
    {
        ArgumentNullException.ThrowIfNull(period);
        
        Period = period;
    }
}