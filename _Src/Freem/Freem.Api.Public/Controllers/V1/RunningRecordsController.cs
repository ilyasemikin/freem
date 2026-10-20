using System.ComponentModel.DataAnnotations;
using Freem.Api.Public.Constants;
using Freem.Api.Public.Contracts.DTO;
using Freem.Api.Public.Contracts.DTO.Records.Running;
using Freem.Api.Public.Extensions;
using Freem.Api.Public.UseCases.Context;
using Freem.Api.Public.UseCases.Contracts.RunningRecords;
using Microsoft.AspNetCore.Mvc;
using Utilities.UseCases.Abstractions;

namespace Freem.Api.Public.Controllers.V1;

/// <summary>Manages the user's running record: the activity that is being tracked right now. A user has at most one.</summary>
[Route("api/v1/records/running")]
[Tags(ControllerTags.RunningRecords)]
public sealed class RunningRecordsController : BaseController
{
    public RunningRecordsController(IRequestMediator<ApiAuthorizedHandlerContext> mediator)
        : base(mediator)
    {
    }

    /// <summary>Returns the running record.</summary>
    [HttpGet]
    [ProducesResponseType<RunningRecordResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAsync(CancellationToken cancellationToken = default)
    {
        var context = HttpContext.ToAuthorizedHandlerContext();
        var request = ApiGetRunningRecordRequest.Instance;
        return await Mediator.ExecuteAsync<ApiGetRunningRecordRequest, IActionResult>(context, request, cancellationToken);
    }

    /// <summary>Discards the running record without saving it as a finished record.</summary>
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemoveAsync(CancellationToken cancellationToken = default)
    {
        var context = HttpContext.ToAuthorizedHandlerContext();
        var request = ApiRemoveRunningRecordRequest.Instance;
        return await Mediator.ExecuteAsync<ApiRemoveRunningRecordRequest, IActionResult>(context, request, cancellationToken);
    }

    /// <summary>Partially updates the running record: only the fields present in the body are changed.</summary>
    /// <param name="body">Fields to change.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateAsync(
        [Required] [FromBody] UpdateRunningRecordRequest body,
        CancellationToken cancellationToken = default)
    {
        var context = HttpContext.ToAuthorizedHandlerContext();
        var request = new ApiUpdateRunningRecordRequest(body);
        return await Mediator.ExecuteAsync<ApiUpdateRunningRecordRequest, IActionResult>(context, request, cancellationToken);
    }

    /// <summary>Starts tracking an activity.</summary>
    /// <remarks>If a running record already exists, it is stopped at the new start time and stored as a finished record.</remarks>
    /// <param name="body">Optional start time (defaults to now), activities and optional name, description and tags.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpPost("start")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> StartAsync(
        [Required] [FromBody] StartRunningRecordRequest body,
        CancellationToken cancellationToken = default)
    {
        var context = HttpContext.ToAuthorizedHandlerContext();
        var request = new ApiStartRunningRecordRequest(body);
        return await Mediator.ExecuteAsync<ApiStartRunningRecordRequest, IActionResult>(context, request, cancellationToken);
    }

    /// <summary>Stops the running record and stores it as a finished record. Responds with 404 if there is no running record.</summary>
    /// <param name="body">Optional end time (defaults to now). It must not be earlier than the start time.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpPost("stop")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> StopAsync(
        [Required] [FromBody] StopRunningRecordRequest body,
        CancellationToken cancellationToken = default)
    {
        var context = HttpContext.ToAuthorizedHandlerContext();
        var request = new ApiStopRunningRecordRequest(body);
        return await Mediator.ExecuteAsync<ApiStopRunningRecordRequest, IActionResult>(context, request, cancellationToken);
    }
}
