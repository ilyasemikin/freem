using System.ComponentModel.DataAnnotations;
using Freem.Domain.Activities;
using Freem.Web.Api.Public.Constants;
using Freem.Web.Api.Public.Contracts;
using Freem.Web.Api.Public.Contracts.DTO;
using Freem.Web.Api.Public.Contracts.DTO.Activities;
using Freem.Web.Api.Public.Extensions;
using Freem.Web.Api.Public.OpenApi.Headers;
using Freem.Web.Api.Public.UseCases.Context;
using Freem.Web.Api.Public.UseCases.Contracts.Activities;
using Microsoft.AspNetCore.Mvc;
using Utilities.UseCases.Abstractions;

namespace Freem.Web.Api.Public.Controllers.V1;

[Route("api/v1/activities")]
[Tags(ControllerTags.Activities)]
public sealed class ActivitiesController : BaseController
{
    public ActivitiesController(IRequestMediator<ApiAuthorizedHandlerContext> mediator)
        : base(mediator)
    {
    }

    [HttpPost]
    [EndpointSummary("Create activity")]
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

    [HttpGet]
    [EndpointSummary("List all activities")]
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

    [HttpGet("by-name")]
    [EndpointSummary("Find activities by name")]
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

    [HttpGet("{activityId:required}")]
    [EndpointSummary("Get activity by id")]
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

    [HttpDelete("{activityId:required}")]
    [EndpointSummary("Remove activity by id")]
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

    [HttpPut("{activityId:required}")]
    [EndpointSummary("Update activity by id")]
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

    [HttpPost("{activityId:required}/archive")]
    [EndpointSummary("Move activity to archive")]
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

    [HttpPost("{activityId:required}/unarchive")]
    [EndpointSummary("Restore activity from archive")]
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
