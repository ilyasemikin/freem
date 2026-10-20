using Freem.Entities.UseCases.Contracts.Statistics.PerDays;
using Freem.Entities.UseCases.Contracts.Statistics.PerPeriod;
using Freem.Entities.UseCases.Contracts.Statistics.PerPeriod.Periods;
using Freem.Entities.UseCases.Contracts.Statistics.PerPeriod.Periods.Abstractions;
using Freem.Web.Api.Public.Contracts.DTO.Statistics.Periods;
using Freem.Web.Api.Public.UseCases.Contracts.Statistics;

namespace Freem.Web.Api.Public.UseCases.Implementations.Mappers.Requests;

internal static class StatisticsRequestMapper
{
    public static StatisticsPerDaysRequest Map(this ApiStatisticsPerDaysRequest request)
    {
        return new StatisticsPerDaysRequest(request.Query.Period);
    }

    public static StatisticsPerPeriodRequest Map(this ApiStatisticsPerPeriodRequest request)
    {
        IStatisticsPeriod period = request.Query.Period switch
        {
            DayUnitPeriod day => new DayStatisticsPeriod(day.Day),
            MonthUnitPeriod month => new MonthStatisticsPeriod(month.Month),
            YearUnitPeriod year => new YearStatisticsPeriod(year.Year),
            _ => throw new InvalidOperationException()
        };

        return new StatisticsPerPeriodRequest(period);
    }
}
