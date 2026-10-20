using Freem.Time.Models;

namespace Freem.Api.Public.Contracts.DTO.Statistics;

public sealed class StatisticsPerDaysRequest
{
    public DatePeriod Period { get; }
    
    public StatisticsPerDaysRequest(DatePeriod period)
    {
        ArgumentNullException.ThrowIfNull(period);
        
        Period = period;
    }
}