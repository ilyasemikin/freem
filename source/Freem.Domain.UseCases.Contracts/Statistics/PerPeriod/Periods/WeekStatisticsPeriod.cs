using Freem.Domain.UseCases.Contracts.Statistics.PerPeriod.Periods.Abstractions;
using Freem.Domain.Users.Models;
using Freem.Time;

namespace Freem.Domain.UseCases.Contracts.Statistics.PerPeriod.Periods;

public sealed class WeekStatisticsPeriod : IStatisticsPeriod
{
    private const int DaysInWeek = 7;

    public DateUnit Unit { get; } = DateUnit.Week;

    // Any day of the week: the start depends on the user's first day of the week
    public DateOnly Day { get; }

    public WeekStatisticsPeriod(DateOnly day)
    {
        Day = day;
    }

    public DatePeriod ToDatePeriod(WeekStart weekStart)
    {
        ArgumentNullException.ThrowIfNull(weekStart);

        var start = weekStart.GetWeekStart(Day);
        var end = start.AddDays(DaysInWeek);
        return new DatePeriod(start, end);
    }
}
