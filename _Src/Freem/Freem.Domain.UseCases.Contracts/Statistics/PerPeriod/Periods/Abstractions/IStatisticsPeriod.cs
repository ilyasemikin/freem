using Freem.Time.Models;

namespace Freem.Domain.UseCases.Contracts.Statistics.PerPeriod.Periods.Abstractions;

public interface IStatisticsPeriod
{
    DateUnit Unit { get; }

    DatePeriod ToDatePeriod();
}