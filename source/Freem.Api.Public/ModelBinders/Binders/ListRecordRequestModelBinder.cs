using Freem.Api.Public.Contracts.V1.Records;
using Freem.Api.Public.ModelBinders.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Freem.Api.Public.ModelBinders.Binders;

internal sealed class ListRecordRequestModelBinder : IModelBinder
{
    private readonly string _offsetName;
    private readonly string _limitName;

    public ListRecordRequestModelBinder()
    {
        _offsetName = nameof(RecordListRequest.Offset).ToLowerInvariant();
        _limitName = nameof(RecordListRequest.Limit).ToLowerInvariant();
    }
    
    public Task BindModelAsync(ModelBindingContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var or = context.ValueProvider.GetValue(_offsetName);
        var lr = context.ValueProvider.GetValue(_limitName);

        if (!or.TryGetIntValue(RecordListRequest.DefaultOffset, out var offset) ||
            !lr.TryGetIntValue(RecordListRequest.DefaultLimit, out var limit))
            return Task.CompletedTask;

        var value = new RecordListRequest(offset.Value, limit.Value);
        context.Result = ModelBindingResult.Success(value);
        
        return Task.CompletedTask;
    }
}