using Freem.Api.Public.Contracts.DTO.Events;
using Freem.Api.Public.ModelBinders.Extensions;
using Freem.Domain.Events;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Freem.Api.Public.ModelBinders.Binders;

internal sealed class ListEventRequestModelBinder : IModelBinder
{
    private readonly string _offsetName;
    private readonly string _limitName;
    private readonly string _entityTypeName;
    private readonly string _entityRefName;
    private readonly string _fromName;
    private readonly string _toName;

    public ListEventRequestModelBinder()
    {
        _offsetName = nameof(ListEventRequest.Offset).ToLowerInvariant();
        _limitName = nameof(ListEventRequest.Limit).ToLowerInvariant();
        _entityTypeName = nameof(ListEventRequest.EntityType).ToLowerInvariant();
        _entityRefName = nameof(ListEventRequest.EntityRef).ToLowerInvariant();
        _fromName = nameof(ListEventRequest.From).ToLowerInvariant();
        _toName = nameof(ListEventRequest.To).ToLowerInvariant();
    }

    public Task BindModelAsync(ModelBindingContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var or = context.ValueProvider.GetValue(_offsetName);
        var lr = context.ValueProvider.GetValue(_limitName);
        var tr = context.ValueProvider.GetValue(_entityTypeName);
        var rr = context.ValueProvider.GetValue(_entityRefName);
        var fr = context.ValueProvider.GetValue(_fromName);
        var dr = context.ValueProvider.GetValue(_toName);

        if (!or.TryGetIntValue(ListEventRequest.DefaultOffset, out var offset) ||
            !lr.TryGetIntValue(ListEventRequest.DefaultLimit, out var limit))
            return Task.CompletedTask;

        if (!TryGetEntityType(tr.FirstValue, out var entityType))
            return Task.CompletedTask;

        if (!rr.TryGetStringValue(null, out var entityRef) && rr.Length != 0)
            return Task.CompletedTask;

        if (!fr.TryGetDateTimeOffsetValue(null, out var from) && fr.Length != 0 ||
            !dr.TryGetDateTimeOffsetValue(null, out var to) && dr.Length != 0)
            return Task.CompletedTask;

        var value = new ListEventRequest(offset.Value, limit.Value, entityType, entityRef, from, to);
        context.Result = ModelBindingResult.Success(value);

        return Task.CompletedTask;
    }

    private static bool TryGetEntityType(string? text, out EntityType? entityType)
    {
        entityType = null;
        if (string.IsNullOrEmpty(text))
            return true;

        var name = text.Replace("_", string.Empty);
        if (!Enum.TryParse<EntityType>(name, true, out var parsed) || !Enum.IsDefined(parsed))
            return false;

        entityType = parsed;
        return true;
    }
}
