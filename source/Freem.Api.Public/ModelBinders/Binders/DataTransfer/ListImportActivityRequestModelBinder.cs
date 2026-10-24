using Freem.Api.Public.Contracts.V1.DataTransfer.Import;
using Freem.Api.Public.ModelBinders.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Freem.Api.Public.ModelBinders.Binders.DataTransfer;

internal sealed class ListImportActivityRequestModelBinder : IModelBinder
{
    private readonly string _offsetName;
    private readonly string _limitName;

    public ListImportActivityRequestModelBinder()
    {
        _offsetName = nameof(ImportActivityListRequest.Offset).ToLowerInvariant();
        _limitName = nameof(ImportActivityListRequest.Limit).ToLowerInvariant();
    }

    public Task BindModelAsync(ModelBindingContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var or = context.ValueProvider.GetValue(_offsetName);
        var lr = context.ValueProvider.GetValue(_limitName);

        if (!or.TryGetIntValue(ImportActivityListRequest.DefaultOffset, out var offset) ||
            !lr.TryGetIntValue(ImportActivityListRequest.DefaultLimit, out var limit))
            return Task.CompletedTask;

        var value = new ImportActivityListRequest(offset.Value, limit.Value);
        context.Result = ModelBindingResult.Success(value);

        return Task.CompletedTask;
    }
}
