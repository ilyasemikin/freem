using System.ComponentModel.DataAnnotations;
using Freem.Domain.Activities;
using Freem.Api.Public.Constants;
using Freem.Api.Public.Contracts;
using Freem.Api.Public.Contracts.DTO;
using Freem.Api.Public.Contracts.DTO.Activities;
using Freem.Api.Public.Extensions;
using Freem.Api.Public.OpenApi.Headers;
using Freem.Api.Public.UseCases.Context;
using Freem.Api.Public.UseCases.Contracts.Activities;
using Microsoft.AspNetCore.Mvc;
using Utilities.UseCases.Abstractions;

namespace Freem.Api.Public.Controllers.V1;

/// <summary>Manages the current user's activities: what the user spends time on.</summary>
[Route("api/v1/activities")]
[Tags(ControllerTags.Activities)]
public sealed class ActivitiesController : BaseController
{
    public ActivitiesController(IRequestMediator<ApiAuthorizedHandlerContext> mediator)
        : base(mediator)
    {
    }

    /// <summary>Creates an activity with the given name and tags.</summary>
    /// <param name="body">Activity name and optional tag ids.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The id of the created activity.</returns>
    [HttpPost]
    [ProducesResponseType<CreateActivityResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateAsync(
        [Required] [FromBody] CreateActivityRequest body,
        CancellationToken cancellationToken = default)
    {
        var context = HttpContext.ToAuthorizedHandlerContext();
        var request = new ApiCreateActivityRequest(body);
        return await Mediator.ExecuteAsync<ApiCreateActivityRequest, IActionResult>(context, request, cancellationToken);
    }

    /// <summary>Returns a page of the user's activities.</summary>
    /// <param name="query">Paging: offset and limit.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <remarks>The page size and the total count are returned in the <c>X-ITEMS-COUNT</c> and <c>X-TOTAL-ITEMS-COUNT</c> headers.</remarks>
    [HttpGet]
    [ProducesResponseType<IReadOnlyList<ActivityResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status500InternalServerError)]
    [ProducesHeader(HeaderNames.ItemsCount, StatusCode = StatusCodes.Status200OK)]
    [ProducesHeader(HeaderNames.TotalItemsCount, StatusCode = StatusCodes.Status200OK)]
    public async Task<IActionResult> ListAsync(
        [Required] [FromQuery] ListActivityRequest query,
        CancellationToken cancellationToken = default)
    {
        var context = HttpContext.ToAuthorizedHandlerContext();
        var request = new ApiListActivityRequest(query);
        return await Mediator.ExecuteAsync<ApiListActivityRequest, IActionResult>(context, request, cancellationToken);
    }

    /// <summary>Searches the user's activities by a fragment of the name (case-sensitive).</summary>
    /// <remarks>Returns at most the default page size, without paging. The page size and the total count are returned in the <c>X-ITEMS-COUNT</c> and <c>X-TOTAL-ITEMS-COUNT</c> headers.</remarks>
    /// <param name="query">Text to search for in activity names.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpGet("by-name")]
    [ProducesResponseType<IReadOnlyList<ActivityResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status500InternalServerError)]
    [ProducesHeader(HeaderNames.ItemsCount, StatusCode = StatusCodes.Status200OK)]
    [ProducesHeader(HeaderNames.TotalItemsCount, StatusCode = StatusCodes.Status200OK)]
    public async Task<IActionResult> FindByNameAsync(
        [Required] [FromQuery] FindActivityByNameRequest query,
        CancellationToken cancellationToken = default)
    {
        var context = HttpContext.ToAuthorizedHandlerContext();
        var request = new ApiFindActivityByNameRequest(query);
        return await Mediator.ExecuteAsync<ApiFindActivityByNameRequest, IActionResult>(context, request, cancellationToken);
    }

    /// <summary>Returns a single activity.</summary>
    /// <param name="activityId">Activity id.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpGet("{activityId:required}")]
    [ProducesResponseType<ActivityResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAsync(
        [Required] [FromRoute] ActivityId activityId,
        CancellationToken cancellationToken = default)
    {
        var context = HttpContext.ToAuthorizedHandlerContext();
        var request = new ApiGetActivityRequest(activityId);
        return await Mediator.ExecuteAsync<ApiGetActivityRequest, IActionResult>(context, request, cancellationToken);
    }

    /// <summary>Permanently removes an activity.</summary>
    /// <param name="activityId">Activity id.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpDelete("{activityId:required}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemoveAsync(
        [Required] [FromRoute] ActivityId activityId,
        CancellationToken cancellationToken = default)
    {
        var context = HttpContext.ToAuthorizedHandlerContext();
        var request = new ApiRemoveActivityRequest(activityId);
        return await Mediator.ExecuteAsync<ApiRemoveActivityRequest, IActionResult>(context, request, cancellationToken);
    }

    /// <summary>Partially updates an activity: only the fields present in the body are changed.</summary>
    /// <param name="activityId">Activity id.</param>
    /// <param name="body">Fields to change.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpPut("{activityId:required}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateAsync(
        [Required] [FromRoute] ActivityId activityId,
        [Required] [FromBody] UpdateActivityRequest body,
        CancellationToken cancellationToken = default)
    {
        var context = HttpContext.ToAuthorizedHandlerContext();
        var request = new ApiUpdateActivityRequest(activityId, body);
        return await Mediator.ExecuteAsync<ApiUpdateActivityRequest, IActionResult>(context, request, cancellationToken);
    }

    /// <summary>Moves an activity to the archive. Fails if it is already archived.</summary>
    /// <param name="activityId">Activity id.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpPost("{activityId:required}/archive")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ArchiveAsync(
        [Required] [FromRoute] ActivityId activityId,
        CancellationToken cancellationToken = default)
    {
        var context = HttpContext.ToAuthorizedHandlerContext();
        var request = new ApiArchiveActivityRequest(activityId);
        return await Mediator.ExecuteAsync<ApiArchiveActivityRequest, IActionResult>(context, request, cancellationToken);
    }

    /// <summary>Returns an archived activity to the active state. Fails if it is already active.</summary>
    /// <param name="activityId">Activity id.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpPost("{activityId:required}/unarchive")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UnarchiveAsync(
        [Required] [FromRoute] ActivityId activityId,
        CancellationToken cancellationToken = default)
    {
        var context = HttpContext.ToAuthorizedHandlerContext();
        var request = new ApiUnarchiveActivityRequest(activityId);
        return await Mediator.ExecuteAsync<ApiUnarchiveActivityRequest, IActionResult>(context, request, cancellationToken);
    }
}
