using System.ComponentModel.DataAnnotations;
using Freem.Entities.Tags;
using Freem.Entities.UseCases;
using Freem.Entities.UseCases.Contracts.Filter;
using Freem.Entities.UseCases.Contracts.Tags.List;
using Freem.UseCases.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;
using Freem.Web.Api.Public.Contracts;
using Freem.Web.Api.Public.Contracts.Tags;
using Freem.Web.Api.Public.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApiListTagRequest = Freem.Web.Api.Public.Contracts.Tags.ListTagRequest;
using UseCaseListTagRequest = Freem.Entities.UseCases.Contracts.Tags.List.ListTagRequest;

namespace Freem.Web.Api.Public.Controllers.V1.Tags;

[Authorize]
[Route("api/v1/tags")]
public sealed class ListTagController : BaseController
{
    private readonly UseCaseContextProvider _contextProvider;
    private readonly IUseCaseExecutor<UseCaseExecutionContext> _executor;

    public ListTagController(
        UseCaseContextProvider contextProvider, 
        IUseCaseExecutor<UseCaseExecutionContext> executor)
    {
        ArgumentNullException.ThrowIfNull(contextProvider);
        ArgumentNullException.ThrowIfNull(executor);
        
        _contextProvider = contextProvider;
        _executor = executor;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IAsyncEnumerable<TagResponse>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ListAsync(
        [Required] [FromQuery] ApiListTagRequest query,
        CancellationToken cancellationToken = default)
    {
        var context = _contextProvider.Get();
        var request = Map(query);

        var response = await _executor.ExecuteAsync<UseCaseListTagRequest, ListTagResponse>(context, request, cancellationToken);

        return response.Success
            ? CreateSuccess(Response, response.Tags, response.TotalCount)
            : CreateFailure(response.Error);
    }

    private static UseCaseListTagRequest Map(ApiListTagRequest request)
    {
        var limit = new Limit(request.Limit);
        var offset = new Offset(request.Offset);
        return new UseCaseListTagRequest(limit, offset);
    }

    private static IActionResult CreateSuccess(
        HttpResponse response, IReadOnlyList<Tag> tags, int totalCount)
    {
        response.Headers.Append(HeaderNames.ItemsCount, tags.Count.ToString());
        response.Headers.Append(HeaderNames.TotalItemsCount, totalCount.ToString());

        var value = MapTags(tags);
        return new OkObjectResult(value);

        static async IAsyncEnumerable<TagResponse> MapTags(IEnumerable<Tag> tags)
        {
            foreach (var tag in tags)
                yield return new TagResponse(tag.Id, tag.Name);
        }
    }

    private static IActionResult CreateFailure(Error<ListTagErrorCode> error)
    {
        return error.Code switch
        {
            _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
        };
    }
}