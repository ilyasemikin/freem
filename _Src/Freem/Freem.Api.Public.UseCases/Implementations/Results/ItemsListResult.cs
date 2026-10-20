using Freem.Api.Public.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Freem.Api.Public.UseCases.Implementations.Results;

/// <summary>
/// 200 OK with a list body and the <see cref="HeaderNames.ItemsCount"/>/<see cref="HeaderNames.TotalItemsCount"/> headers.
/// </summary>
internal sealed class ItemsListResult<T> : OkObjectResult
{
    private readonly int _itemsCount;
    private readonly int _totalCount;

    public ItemsListResult(IReadOnlyList<T> items, int totalCount)
        : base(items)
    {
        ArgumentNullException.ThrowIfNull(items);

        _itemsCount = items.Count;
        _totalCount = totalCount;
    }

    public override Task ExecuteResultAsync(ActionContext context)
    {
        var itemsCount = _itemsCount.ToString();
        var totalCount = _totalCount.ToString();

        context.HttpContext.Response.Headers.Append(HeaderNames.ItemsCount, itemsCount);
        context.HttpContext.Response.Headers.Append(HeaderNames.TotalItemsCount, totalCount);

        return base.ExecuteResultAsync(context);
    }
}
