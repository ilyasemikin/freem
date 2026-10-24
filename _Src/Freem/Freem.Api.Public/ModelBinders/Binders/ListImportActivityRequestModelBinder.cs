using Freem.Api.Public.Contracts.DTO.Imports;
using Freem.Api.Public.ModelBinders.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Freem.Api.Public.ModelBinders.Binders;

internal sealed class ListImportActivityRequestModelBinder : IModelBinder
{
    private readonly string _offsetName;
    private readonly string _limitName;

    public ListImportActivityRequestModelBinder()
    {
        _offsetName = nameof(ListImportActivityRequest.Offset).ToLowerInvariant();
        _limitName = nameof(ListImportActivityRequest.Limit).ToLowerInvariant();
    }

    public Task BindModelAsync(ModelBindingContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var or = context.ValueProvider.GetValue(_offsetName);
        var lr = context.ValueProvider.GetValue(_limitName);

        if (!or.TryGetIntValue(ListImportActivityRequest.DefaultOffset, out var offset) ||
            !lr.TryGetIntValue(ListImportActivityRequest.DefaultLimit, out var limit))
            return Task.CompletedTask;

        var value = new ListImportActivityRequest(offset.Value, limit.Value);
        context.Result = ModelBindingResult.Success(value);

        return Task.CompletedTask;
    }
}
