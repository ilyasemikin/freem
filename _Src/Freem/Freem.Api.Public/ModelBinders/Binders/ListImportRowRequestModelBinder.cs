using Freem.Api.Public.Contracts.DTO.Imports;
using Freem.Api.Public.ModelBinders.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Freem.Api.Public.ModelBinders.Binders;

internal sealed class ListImportRowRequestModelBinder : IModelBinder
{
    private readonly string _offsetName;
    private readonly string _limitName;

    public ListImportRowRequestModelBinder()
    {
        _offsetName = nameof(ListImportRowRequest.Offset).ToLowerInvariant();
        _limitName = nameof(ListImportRowRequest.Limit).ToLowerInvariant();
    }

    public Task BindModelAsync(ModelBindingContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var or = context.ValueProvider.GetValue(_offsetName);
        var lr = context.ValueProvider.GetValue(_limitName);

        if (!or.TryGetIntValue(ListImportRowRequest.DefaultOffset, out var offset) ||
            !lr.TryGetIntValue(ListImportRowRequest.DefaultLimit, out var limit))
            return Task.CompletedTask;

        var value = new ListImportRowRequest(offset.Value, limit.Value);
        context.Result = ModelBindingResult.Success(value);

        return Task.CompletedTask;
    }
}
