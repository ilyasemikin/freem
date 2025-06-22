using Freem.Web.Api.Public.Contracts.DTO.Events;
using Freem.Web.Api.Public.ModelBinders.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Freem.Web.Api.Public.ModelBinders.Binders;

internal sealed class ListEventRequestModelBinder : IModelBinder
{
    private readonly string _limitName;
    private readonly string _afterName;

    public ListEventRequestModelBinder()
    {
        _limitName = nameof(ListEventRequest.Limit).ToLowerInvariant();
        _afterName = nameof(ListEventRequest.After).ToLowerInvariant();
    }
    
    public Task BindModelAsync(ModelBindingContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        
        var lr = context.ValueProvider.GetValue(_limitName);
        var ar = context.ValueProvider.GetValue(_afterName);

        if (!lr.TryGetIntValue(ListEventRequest.DefaultLimit, out var limit))
            return Task.CompletedTask;

        ar.TryGetDateTimeOffsetValue(null, out var after);
        
        var value = new ListEventRequest(limit.Value, after);
        context.Result = ModelBindingResult.Success(value);
        
        return Task.CompletedTask;
    }
}