using Freem.Time.Models;
using Freem.Web.Api.Public.Contracts.Records;
using Freem.Web.Api.Public.ModelBinders.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Freem.Web.Api.Public.ModelBinders.Binders;

internal sealed class ListRecordByPeriodRequestModelBinder : IModelBinder
{
    private readonly string _periodName;
    private readonly string _limitName;

    public ListRecordByPeriodRequestModelBinder()
    {
        _periodName = nameof(ListRecordByPeriodRequest.Period).ToLowerInvariant();
        _limitName = nameof(ListRecordByPeriodRequest.Limit).ToLowerInvariant();
    }
    
    public Task BindModelAsync(ModelBindingContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var pr = context.ValueProvider.GetValue(_periodName);
        var lr = context.ValueProvider.GetValue(_limitName);
        
        if (!pr.TryGetDateTimePeriodValue(DateTimePeriod.Empty, out var period) ||
            !lr.TryGetIntValue(ListRecordByPeriodRequest.DefaultLimit, out var limit))
            return Task.CompletedTask;
        
        var value = new ListRecordByPeriodRequest(period, limit.Value);
        context.Result = ModelBindingResult.Success(value);

        return Task.CompletedTask;
    }
}