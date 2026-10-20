using Freem.Api.Public.Contracts.DTO.Tags;
using Freem.Api.Public.ModelBinders.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Freem.Api.Public.ModelBinders.Binders;

internal sealed class ListTagRequestModelBinder : IModelBinder
{
    private readonly string _offsetName;
    private readonly string _limitName;

    public ListTagRequestModelBinder()
    {
        _offsetName = nameof(ListTagRequest.Offset).ToLowerInvariant();
        _limitName = nameof(ListTagRequest.Limit).ToLowerInvariant();
    }
    
    public Task BindModelAsync(ModelBindingContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var or = context.ValueProvider.GetValue(_offsetName);
        var lr = context.ValueProvider.GetValue(_limitName);

        if (!or.TryGetIntValue(ListTagRequest.DefaultOffset, out var offset) ||
            !lr.TryGetIntValue(ListTagRequest.DefaultLimit, out var limit))
            return Task.CompletedTask;

        var value = new ListTagRequest(offset.Value, limit.Value);
        context.Result = ModelBindingResult.Success(value);

        return Task.CompletedTask;
    }
}