using Freem.Api.Public.Contracts.V1.Imports;
using Freem.Api.Public.ModelBinders.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Freem.Api.Public.ModelBinders.Binders;

internal sealed class ListImportTagRequestModelBinder : IModelBinder
{
    private readonly string _offsetName;
    private readonly string _limitName;

    public ListImportTagRequestModelBinder()
    {
        _offsetName = nameof(ImportTagListRequest.Offset).ToLowerInvariant();
        _limitName = nameof(ImportTagListRequest.Limit).ToLowerInvariant();
    }

    public Task BindModelAsync(ModelBindingContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var or = context.ValueProvider.GetValue(_offsetName);
        var lr = context.ValueProvider.GetValue(_limitName);

        if (!or.TryGetIntValue(ImportTagListRequest.DefaultOffset, out var offset) ||
            !lr.TryGetIntValue(ImportTagListRequest.DefaultLimit, out var limit))
            return Task.CompletedTask;

        var value = new ImportTagListRequest(offset.Value, limit.Value);
        context.Result = ModelBindingResult.Success(value);

        return Task.CompletedTask;
    }
}
