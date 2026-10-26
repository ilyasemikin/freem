using Freem.Api.Public.Contracts.V1.Statistics.Periods;
using Freem.Domain.UseCases.Contracts.Statistics.PerPeriod;
using Freem.Domain.UseCases.Contracts.Statistics.PerPeriod.Periods;
using Freem.Domain.UseCases.Contracts.Statistics.PerPeriod.Periods.Abstractions;
using Freem.Api.Public.UseCases.Contracts.Statistics;

namespace Freem.Api.Public.UseCases.Implementations.Mappers.Requests;

internal static class StatisticsRequestMapper
{
    public static StatisticsPerPeriodRequest Map(this ApiStatisticsPerPeriodRequest request)
    {
        IStatisticsPeriod period = request.Query.Period switch
        {
            DayUnitPeriod day => new DayStatisticsPeriod(day.Day),
            WeekUnitPeriod week => new WeekStatisticsPeriod(week.Week),
            MonthUnitPeriod month => new MonthStatisticsPeriod(month.Month),
            YearUnitPeriod year => new YearStatisticsPeriod(year.Year),
            _ => throw new InvalidOperationException()
        };

        return new StatisticsPerPeriodRequest(period);
    }
}
