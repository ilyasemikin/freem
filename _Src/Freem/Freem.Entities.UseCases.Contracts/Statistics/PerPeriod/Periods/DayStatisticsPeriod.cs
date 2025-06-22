using Freem.Entities.UseCases.Contracts.Statistics.PerPeriod.Periods.Abstractions;
using Freem.Time.Models;

namespace Freem.Entities.UseCases.Contracts.Statistics.PerPeriod.Periods;

public sealed class DayStatisticsPeriod : IStatisticsPeriod
{
    public DateUnit Unit { get; } = DateUnit.Day;

    public DateOnly Date { get; }

    public DayStatisticsPeriod(DateOnly date)
    {
        Date = date;
    }
    
    public DatePeriod ToDatePeriod()
    {
        var start = Date;
        var end = Date.AddDays(1);
        return new DatePeriod(start, end);
    }
}