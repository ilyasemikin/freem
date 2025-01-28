using Freem.Time.Models;

namespace Freem.Entities.UseCases.Contracts.Statistics.PerDays;

public sealed class StatisticsPerDaysRequest
{
    public DatePeriod Period { get; }

    public StatisticsPerDaysRequest(DatePeriod period)
    {
        ArgumentNullException.ThrowIfNull(period);
        
        Period = period;
    }
}