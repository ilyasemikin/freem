using Freem.Api.Public.Contracts.DTO.Imports;
using Freem.Api.Public.ModelBinders.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Freem.Api.Public.ModelBinders.Binders;

internal sealed class ListImportTagRequestModelBinder : IModelBinder
{
    private readonly string _offsetName;
    private readonly string _limitName;

    public ListImportTagRequestModelBinder()
    {
        _offsetName = nameof(ListImportTagRequest.Offset).ToLowerInvariant();
        _limitName = nameof(ListImportTagRequest.Limit).ToLowerInvariant();
    }

    public Task BindModelAsync(ModelBindingContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var or = context.ValueProvider.GetValue(_offsetName);
        var lr = context.ValueProvider.GetValue(_limitName);

        if (!or.TryGetIntValue(ListImportTagRequest.DefaultOffset, out var offset) ||
            !lr.TryGetIntValue(ListImportTagRequest.DefaultLimit, out var limit))
            return Task.CompletedTask;

        var value = new ListImportTagRequest(offset.Value, limit.Value);
        context.Result = ModelBindingResult.Success(value);

        return Task.CompletedTask;
    }
}
