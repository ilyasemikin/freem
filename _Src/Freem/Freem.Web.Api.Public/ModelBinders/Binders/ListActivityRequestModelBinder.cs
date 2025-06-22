using Freem.Web.Api.Public.Contracts.DTO.Activities;
using Freem.Web.Api.Public.ModelBinders.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Freem.Web.Api.Public.ModelBinders.Binders;

internal sealed class ListActivityRequestModelBinder : IModelBinder
{
    private readonly string _offsetName;
    private readonly string _limitName;

    public ListActivityRequestModelBinder()
    {
        _offsetName = nameof(ListActivityRequest.Offset).ToLowerInvariant();
        _limitName = nameof(ListActivityRequest.Limit).ToLowerInvariant();
    }
    
    public Task BindModelAsync(ModelBindingContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        
        var or = context.ValueProvider.GetValue(_offsetName);
        var lr = context.ValueProvider.GetValue(_limitName);
        
        if (!or.TryGetIntValue(ListActivityRequest.DefaultOffset, out var offset) || 
            !lr.TryGetIntValue(ListActivityRequest.DefaultLimit, out var limit))
            return Task.CompletedTask;

        var value = new ListActivityRequest(offset.Value, limit.Value);
        context.Result = ModelBindingResult.Success(value);
        
        return Task.CompletedTask;
    }
}