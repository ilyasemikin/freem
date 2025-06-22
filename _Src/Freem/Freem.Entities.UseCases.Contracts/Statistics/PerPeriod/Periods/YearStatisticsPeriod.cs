using Freem.Entities.UseCases.Contracts.Statistics.PerPeriod.Periods.Abstractions;
using Freem.Time.Models;

namespace Freem.Entities.UseCases.Contracts.Statistics.PerPeriod.Periods;

public sealed class YearStatisticsPeriod : IStatisticsPeriod
{
    public DateUnit Unit { get; } = DateUnit.Year;

    public int Year { get; }

    public YearStatisticsPeriod(int year)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(year);
        
        Year = year;
    }
    
    public DatePeriod ToDatePeriod()
    {
        var start = new DateOnly(Year, 1, 1);
        var end = start.AddYears(1);
        return new DatePeriod(start, end);
    }
}