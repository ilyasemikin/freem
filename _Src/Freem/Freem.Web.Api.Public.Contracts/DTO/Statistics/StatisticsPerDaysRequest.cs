using Freem.Time.Models;

namespace Freem.Web.Api.Public.Contracts.DTO.Statistics;

public sealed class StatisticsPerDaysRequest
{
    public DatePeriod Period { get; }
    
    public StatisticsPerDaysRequest(DatePeriod period)
    {
        ArgumentNullException.ThrowIfNull(period);
        
        Period = period;
    }
}