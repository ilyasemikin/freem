using System.ComponentModel.DataAnnotations;
using Freem.Domain.Tags;
using Freem.Domain.Tags.Models;
using Freem.Api.Public.Constants;
using Freem.Api.Public.Contracts;
using Freem.Api.Public.Contracts.DTO;
using Freem.Api.Public.Contracts.DTO.Tags;
using Freem.Api.Public.Extensions;
using Freem.Api.Public.OpenApi.Headers;
using Freem.Api.Public.UseCases.Context;
using Freem.Api.Public.UseCases.Contracts.Tags;
using Microsoft.AspNetCore.Mvc;
using Utilities.UseCases.Abstractions;

namespace Freem.Api.Public.Controllers.V1;

/// <summary>Manages the current user's tags: labels attached to activities and records.</summary>
[Route("api/v1/tags")]
[Tags(ControllerTags.Tags)]
public sealed class TagsController : BaseController
{
    public TagsController(IRequestMediator<ApiAuthorizedHandlerContext> mediator)
        : base(mediator)
    {
    }

    /// <summary>Creates a tag.</summary>
    /// <param name="body">Tag name.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The id of the created tag.</returns>
    [HttpPost]
    [ProducesResponseType<CreateTagResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateAsync(
        [Required] [FromBody] CreateTagRequest body,
        CancellationToken cancellationToken = default)
    {
        var context = HttpContext.ToAuthorizedHandlerContext();
        var request = new ApiCreateTagRequest(body);
        return await Mediator.ExecuteAsync<ApiCreateTagRequest, IActionResult>(context, request, cancellationToken);
    }

    /// <summary>Returns a page of the user's tags.</summary>
    /// <param name="query">Paging: offset and limit.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <remarks>The page size and the total count are returned in the <c>X-ITEMS-COUNT</c> and <c>X-TOTAL-ITEMS-COUNT</c> headers.</remarks>
    [HttpGet]
    [ProducesResponseType<IReadOnlyList<TagResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status500InternalServerError)]
    [ProducesHeader(HeaderNames.ItemsCount, StatusCode = StatusCodes.Status200OK)]
    [ProducesHeader(HeaderNames.TotalItemsCount, StatusCode = StatusCodes.Status200OK)]
    public async Task<IActionResult> ListAsync(
        [Required] [FromQuery] ListTagRequest query,
        CancellationToken cancellationToken = default)
    {
        var context = HttpContext.ToAuthorizedHandlerContext();
        var request = new ApiListTagRequest(query);
        return await Mediator.ExecuteAsync<ApiListTagRequest, IActionResult>(context, request, cancellationToken);
    }

    /// <summary>Returns the first of the user's tags whose name contains the given text (case-sensitive). Responds with 404 if there is none.</summary>
    /// <param name="tagName">Text to look for in tag names.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpGet("by-name/{tagName:required}")]
    [ProducesResponseType<TagResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> FindByNameAsync(
        [Required] [FromRoute] TagName tagName,
        CancellationToken cancellationToken = default)
    {
        var context = HttpContext.ToAuthorizedHandlerContext();
        var request = new ApiFindTagByNameRequest(tagName);
        return await Mediator.ExecuteAsync<ApiFindTagByNameRequest, IActionResult>(context, request, cancellationToken);
    }

    /// <summary>Returns a single tag.</summary>
    /// <param name="tagId">Tag id.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpGet("{tagId:required}")]
    [ProducesResponseType<TagResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAsync(
        [Required] [FromRoute] TagId tagId,
        CancellationToken cancellationToken = default)
    {
        var context = HttpContext.ToAuthorizedHandlerContext();
        var request = new ApiGetTagRequest(tagId);
        return await Mediator.ExecuteAsync<ApiGetTagRequest, IActionResult>(context, request, cancellationToken);
    }

    /// <summary>Permanently removes a tag.</summary>
    /// <param name="tagId">Tag id.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpDelete("{tagId:required}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemoveAsync(
        [Required] [FromRoute] TagId tagId,
        CancellationToken cancellationToken = default)
    {
        var context = HttpContext.ToAuthorizedHandlerContext();
        var request = new ApiRemoveTagRequest(tagId);
        return await Mediator.ExecuteAsync<ApiRemoveTagRequest, IActionResult>(context, request, cancellationToken);
    }

    /// <summary>Partially updates a tag: only the fields present in the body are changed.</summary>
    /// <param name="tagId">Tag id.</param>
    /// <param name="body">Fields to change.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpPut("{tagId:required}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateAsync(
        [Required] [FromRoute] TagId tagId,
        [Required] [FromBody] UpdateTagRequest body,
        CancellationToken cancellationToken = default)
    {
        var context = HttpContext.ToAuthorizedHandlerContext();
        var request = new ApiUpdateTagRequest(tagId, body);
        return await Mediator.ExecuteAsync<ApiUpdateTagRequest, IActionResult>(context, request, cancellationToken);
    }
}
