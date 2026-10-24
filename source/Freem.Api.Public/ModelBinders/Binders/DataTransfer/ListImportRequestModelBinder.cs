using Freem.Api.Public.Contracts.V1.DataTransfer.Import;
using Freem.Api.Public.ModelBinders.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Freem.Api.Public.ModelBinders.Binders.DataTransfer;

internal sealed class ListImportRequestModelBinder : IModelBinder
{
    private readonly string _offsetName;
    private readonly string _limitName;

    public ListImportRequestModelBinder()
    {
        _offsetName = nameof(ImportListRequest.Offset).ToLowerInvariant();
        _limitName = nameof(ImportListRequest.Limit).ToLowerInvariant();
    }

    public Task BindModelAsync(ModelBindingContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var or = context.ValueProvider.GetValue(_offsetName);
        var lr = context.ValueProvider.GetValue(_limitName);

        if (!or.TryGetIntValue(ImportListRequest.DefaultOffset, out var offset) ||
            !lr.TryGetIntValue(ImportListRequest.DefaultLimit, out var limit))
            return Task.CompletedTask;

        var value = new ImportListRequest(offset.Value, limit.Value);
        context.Result = ModelBindingResult.Success(value);

        return Task.CompletedTask;
    }
}
