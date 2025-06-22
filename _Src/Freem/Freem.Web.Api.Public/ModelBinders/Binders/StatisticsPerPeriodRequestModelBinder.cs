using Freem.Time.Models;
using Freem.Web.Api.Public.Contracts.DTO.Statistics;
using Freem.Web.Api.Public.Contracts.DTO.Statistics.Periods;
using Freem.Web.Api.Public.Contracts.DTO.Statistics.Periods.Abstractions;
using Freem.Web.Api.Public.ModelBinders.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Freem.Web.Api.Public.ModelBinders.Binders;

public sealed class StatisticsPerPeriodRequestModelBinder : IModelBinder
{
    private readonly string _unitName;

    private readonly string _dayName;
    private readonly string _monthName;
    private readonly string _yearName;

    public StatisticsPerPeriodRequestModelBinder()
    {
        _unitName = nameof(IUnitPeriod.Unit).ToLowerInvariant();

        _dayName = nameof(DayUnitPeriod.Day).ToLowerInvariant();
        _monthName = nameof(MonthUnitPeriod.Month).ToLowerInvariant();
        _yearName = nameof(YearUnitPeriod.Year).ToLowerInvariant();
    }

    public Task BindModelAsync(ModelBindingContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var upr = context.ValueProvider.GetValue(_unitName);

        if (!upr.TryGetEnumValue<DateUnit>(out var unit))
            return Task.CompletedTask;

        IUnitPeriod period;
        switch (unit)
        {
            case DateUnit.Day:
            {
                var vpr = context.ValueProvider.GetValue(_dayName);
                if (!vpr.TryGetDateOnlyValue(out var day))
                    return Task.CompletedTask;
                period = new DayUnitPeriod(day.Value);
                break;
            }
            case DateUnit.Month:
            {
                var vpr = context.ValueProvider.GetValue(_monthName);
                if (!vpr.TryGetMonthOnlyValue(out var month))
                    return Task.CompletedTask;
                period = new MonthUnitPeriod(month);
                break;
            }
            case DateUnit.Year:
            {
                var vpr = context.ValueProvider.GetValue(_yearName);
                if (!vpr.TryGetIntValue(null, out var year))
                    return Task.CompletedTask;
                period = new YearUnitPeriod(year.Value);
                break;
            }
            default:
                return Task.CompletedTask;
        }

        var value = new StatisticsPerPeriodRequest(period);
        context.Result = ModelBindingResult.Success(value);
        
        return Task.CompletedTask;
    }
}