using Freem.Web.Api.Public.Contracts.Records;
using Freem.Web.Api.Public.ModelBinders.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Freem.Web.Api.Public.ModelBinders.Binders;

internal sealed class ListRecordRequestModelBinder : IModelBinder
{
    private readonly string _offsetName;
    private readonly string _limitName;

    public ListRecordRequestModelBinder()
    {
        _offsetName = nameof(ListRecordRequest.Offset).ToLowerInvariant();
        _limitName = nameof(ListRecordRequest.Limit).ToLowerInvariant();
    }
    
    public Task BindModelAsync(ModelBindingContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var or = context.ValueProvider.GetValue(_offsetName);
        var lr = context.ValueProvider.GetValue(_limitName);

        if (!or.TryGetIntValue(ListRecordRequest.DefaultLimit, out var limit) ||
            !lr.TryGetIntValue(ListRecordRequest.DefaultOffset, out var offset))
            return Task.CompletedTask;

        var value = new ListRecordRequest(offset.Value, limit.Value);
        context.Result = ModelBindingResult.Success(value);
        
        return Task.CompletedTask;
    }
}