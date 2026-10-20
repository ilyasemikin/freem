using System.ComponentModel.DataAnnotations;
using Freem.Domain.Entities.Tags;
using Freem.Entities.UseCases.Context;
using Freem.Entities.UseCases.Contracts.Filter;
using Freem.Entities.UseCases.Contracts.Tags.List;
using Freem.Web.Api.Public.Constants;
using Freem.Web.Api.Public.Contracts;
using Freem.Web.Api.Public.Contracts.DTO.Tags;
using Freem.Web.Api.Public.OpenApi.Headers;
using Freem.Web.Api.Public.Services.Implementations;
using Microsoft.AspNetCore.Mvc;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;
using ApiListTagRequest = Freem.Web.Api.Public.Contracts.DTO.Tags.ListTagRequest;
using UseCaseListTagRequest = Freem.Entities.UseCases.Contracts.Tags.List.ListTagRequest;

namespace Freem.Web.Api.Public.Controllers.V1.Tags;

[Route("api/v1/tags")]
[Tags(ControllerTags.Tags)]
[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IAsyncEnumerable<TagResponse>))]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
[ProducesHeader(HeaderNames.ItemsCount, StatusCode = StatusCodes.Status200OK)]
[ProducesHeader(HeaderNames.TotalItemsCount, StatusCode = StatusCodes.Status200OK)]
public sealed class ListTagController : BaseController
{
    private readonly UseCaseContextProvider _contextProvider;
    private readonly IRequestMediator<HandlerContext> _mediator;

    public ListTagController(
        UseCaseContextProvider contextProvider, 
        IRequestMediator<HandlerContext> mediator)
    {
        ArgumentNullException.ThrowIfNull(contextProvider);
        ArgumentNullException.ThrowIfNull(mediator);
        
        _contextProvider = contextProvider;
        _mediator = mediator;
    }

    [HttpGet]
    [EndpointSummary("List all tags")]
    public async Task<IActionResult> ListAsync(
        [Required] [FromQuery] ApiListTagRequest query,
        CancellationToken cancellationToken = default)
    {
        if (!_contextProvider.TryGet(out var context))
            return new UnauthorizedResult();

        var request = Map(query);

        var response = await _mediator.ExecuteAsync<UseCaseListTagRequest, Result<ListTagResponse, Error>>(context, request, cancellationToken);

        return response.Success
            ? CreateSuccess(Response, response.Value.Tags, response.Value.TotalCount)
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

    private static IActionResult CreateFailure(Error error)
    {
        return new StatusCodeResult(StatusCodes.Status500InternalServerError);
    }
}