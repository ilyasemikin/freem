using Freem.Domain.Users.Models;
using Freem.Time;

namespace Freem.Domain.UseCases.Contracts.Statistics.PerPeriod.Periods.Abstractions;

public interface IStatisticsPeriod
{
    DateUnit Unit { get; }

    DatePeriod ToDatePeriod(WeekStart weekStart);
}