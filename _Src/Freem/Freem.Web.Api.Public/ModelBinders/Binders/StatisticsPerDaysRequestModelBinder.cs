using Freem.Time.Models;
using Freem.Web.Api.Public.Contracts.DTO.Statistics;
using Freem.Web.Api.Public.ModelBinders.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Freem.Web.Api.Public.ModelBinders.Binders;

internal sealed class StatisticsPerDaysRequestModelBinder : IModelBinder
{
    private readonly string _periodName;

    public StatisticsPerDaysRequestModelBinder()
    {
        _periodName = nameof(StatisticsPerDaysRequest.Period).ToLowerInvariant();
    }
    
    public Task BindModelAsync(ModelBindingContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var pr = context.ValueProvider.GetValue(_periodName);

        if (!pr.TryGetDatePeriodValue(DatePeriod.Empty, out var period))
            return Task.CompletedTask;

        var value = new StatisticsPerDaysRequest(period);
        context.Result = ModelBindingResult.Success(value);
        
        return Task.CompletedTask;
    }
}