using Freem.Api.Public.Contracts.V1.Activities;
using Freem.Api.Public.ModelBinders.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Freem.Api.Public.ModelBinders.Binders;

internal sealed class ListActivityRequestModelBinder : IModelBinder
{
    private readonly string _offsetName;
    private readonly string _limitName;

    public ListActivityRequestModelBinder()
    {
        _offsetName = nameof(ActivityListRequest.Offset).ToLowerInvariant();
        _limitName = nameof(ActivityListRequest.Limit).ToLowerInvariant();
    }
    
    public Task BindModelAsync(ModelBindingContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        
        var or = context.ValueProvider.GetValue(_offsetName);
        var lr = context.ValueProvider.GetValue(_limitName);
        
        if (!or.TryGetIntValue(ActivityListRequest.DefaultOffset, out var offset) || 
            !lr.TryGetIntValue(ActivityListRequest.DefaultLimit, out var limit))
            return Task.CompletedTask;

        var value = new ActivityListRequest(offset.Value, limit.Value);
        context.Result = ModelBindingResult.Success(value);
        
        return Task.CompletedTask;
    }
}