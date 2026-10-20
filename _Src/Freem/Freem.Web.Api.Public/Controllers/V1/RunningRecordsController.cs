using System.ComponentModel.DataAnnotations;
using Freem.Web.Api.Public.Constants;
using Freem.Web.Api.Public.Contracts.DTO;
using Freem.Web.Api.Public.Contracts.DTO.Records.Running;
using Freem.Web.Api.Public.Extensions;
using Freem.Web.Api.Public.UseCases.Context;
using Freem.Web.Api.Public.UseCases.Contracts.RunningRecords;
using Microsoft.AspNetCore.Mvc;
using Utilities.UseCases.Abstractions;

namespace Freem.Web.Api.Public.Controllers.V1;

[Route("api/v1/records/running")]
[Tags(ControllerTags.RunningRecords)]
public sealed class RunningRecordsController : BaseController
{
    public RunningRecordsController(IRequestMediator<ApiAuthorizedHandlerContext> mediator)
        : base(mediator)
    {
    }

    [HttpGet]
    [EndpointSummary("Get running record")]
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

    [HttpDelete]
    [EndpointSummary("Remove running record")]
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

    [HttpPut]
    [EndpointSummary("Update running record")]
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

    [HttpPost("start")]
    [EndpointSummary("Start running record")]
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

    [HttpPost("stop")]
    [EndpointSummary("Stop running record if exists")]
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
