using Freem.Entities.UseCases.Contracts.Statistics.PerPeriod.Periods.Abstractions;
using Freem.Time.Models;

namespace Freem.Entities.UseCases.Contracts.Statistics.PerPeriod.Periods;

public sealed class MonthStatisticsPeriod : IStatisticsPeriod
{
    public DateUnit Unit { get; } = DateUnit.Month;

    public MonthOnly Month { get; }

    public MonthStatisticsPeriod(MonthOnly month)
    {
        ArgumentNullException.ThrowIfNull(month);
        
        Month = month;
    }
    
    public DatePeriod ToDatePeriod()
    {
        var start = Month.ToDateOnly(1);
        var end = start.AddMonths(1);
        return new DatePeriod(start, end);
    }
}