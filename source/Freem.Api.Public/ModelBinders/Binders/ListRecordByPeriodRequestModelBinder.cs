using Freem.Api.Public.Contracts.V1.Records;
using Freem.Api.Public.ModelBinders.Extensions;
using Freem.Time;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Freem.Api.Public.ModelBinders.Binders;

internal sealed class ListRecordByPeriodRequestModelBinder : IModelBinder
{
    private readonly string _periodName;
    private readonly string _limitName;

    public ListRecordByPeriodRequestModelBinder()
    {
        _periodName = nameof(RecordListByPeriodRequest.Period).ToLowerInvariant();
        _limitName = nameof(RecordListByPeriodRequest.Limit).ToLowerInvariant();
    }
    
    public Task BindModelAsync(ModelBindingContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var pr = context.ValueProvider.GetValue(_periodName);
        var lr = context.ValueProvider.GetValue(_limitName);
        
        if (!pr.TryGetDateTimePeriodValue(DateTimePeriod.Empty, out var period) ||
            !lr.TryGetIntValue(RecordListByPeriodRequest.DefaultLimit, out var limit))
            return Task.CompletedTask;
        
        var value = new RecordListByPeriodRequest(period, limit.Value);
        context.Result = ModelBindingResult.Success(value);

        return Task.CompletedTask;
    }
}