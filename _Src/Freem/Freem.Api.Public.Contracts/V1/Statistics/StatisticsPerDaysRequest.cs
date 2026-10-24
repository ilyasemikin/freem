using Freem.Time;

namespace Freem.Api.Public.Contracts.V1.Statistics;

/// <summary>Query for per-day statistics.</summary>
public sealed class StatisticsPerDaysRequest
{
    /// <summary>Range of days to report.</summary>
    public DatePeriod Period { get; }
    
    public StatisticsPerDaysRequest(DatePeriod period)
    {
        ArgumentNullException.ThrowIfNull(period);
        
        Period = period;
    }
}