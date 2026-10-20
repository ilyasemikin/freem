using System.ComponentModel.DataAnnotations;
using Freem.Domain.Records;
using Freem.Api.Public.Constants;
using Freem.Api.Public.Contracts;
using Freem.Api.Public.Contracts.DTO;
using Freem.Api.Public.Contracts.DTO.Records;
using Freem.Api.Public.Extensions;
using Freem.Api.Public.OpenApi.Headers;
using Freem.Api.Public.UseCases.Context;
using Freem.Api.Public.UseCases.Contracts.Records;
using Microsoft.AspNetCore.Mvc;
using Utilities.UseCases.Abstractions;

namespace Freem.Api.Public.Controllers.V1;

[Route("api/v1/records")]
[Tags(ControllerTags.Records)]
public sealed class RecordsController : BaseController
{
    public RecordsController(IRequestMediator<ApiAuthorizedHandlerContext> mediator)
        : base(mediator)
    {
    }

    [HttpPost]
    [EndpointSummary("Create record")]
    [ProducesResponseType<CreateRecordResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateAsync(
        [Required] [FromBody] CreateRecordRequest body,
        CancellationToken cancellationToken = default)
    {
        var context = HttpContext.ToAuthorizedHandlerContext();
        var request = new ApiCreateRecordRequest(body);
        return await Mediator.ExecuteAsync<ApiCreateRecordRequest, IActionResult>(context, request, cancellationToken);
    }

    [HttpGet]
    [EndpointSummary("List all records")]
    [ProducesResponseType<IReadOnlyList<RecordResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status500InternalServerError)]
    [ProducesHeader(HeaderNames.ItemsCount, StatusCode = StatusCodes.Status200OK)]
    [ProducesHeader(HeaderNames.TotalItemsCount, StatusCode = StatusCodes.Status200OK)]
    public async Task<IActionResult> ListAsync(
        [Required] [FromQuery] ListRecordRequest query,
        CancellationToken cancellationToken = default)
    {
        var context = HttpContext.ToAuthorizedHandlerContext();
        var request = new ApiListRecordRequest(query);
        return await Mediator.ExecuteAsync<ApiListRecordRequest, IActionResult>(context, request, cancellationToken);
    }

    [HttpGet("by-period")]
    [EndpointSummary("List records by period")]
    [ProducesResponseType<IReadOnlyList<RecordResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status500InternalServerError)]
    [ProducesHeader(HeaderNames.ItemsCount, StatusCode = StatusCodes.Status200OK)]
    [ProducesHeader(HeaderNames.TotalItemsCount, StatusCode = StatusCodes.Status200OK)]
    public async Task<IActionResult> ListByPeriodAsync(
        [Required] [FromQuery] ListRecordByPeriodRequest query,
        CancellationToken cancellationToken = default)
    {
        var context = HttpContext.ToAuthorizedHandlerContext();
        var request = new ApiListRecordByPeriodRequest(query);
        return await Mediator.ExecuteAsync<ApiListRecordByPeriodRequest, IActionResult>(context, request, cancellationToken);
    }

    [HttpGet("{recordId:required}")]
    [EndpointSummary("Get record by id")]
    [ProducesResponseType<RecordResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAsync(
        [Required] [FromRoute] RecordId recordId,
        CancellationToken cancellationToken = default)
    {
        var context = HttpContext.ToAuthorizedHandlerContext();
        var request = new ApiGetRecordRequest(recordId);
        return await Mediator.ExecuteAsync<ApiGetRecordRequest, IActionResult>(context, request, cancellationToken);
    }

    [HttpDelete("{recordId:required}")]
    [EndpointSummary("Remove record by id")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemoveAsync(
        [Required] [FromRoute] RecordId recordId,
        CancellationToken cancellationToken = default)
    {
        var context = HttpContext.ToAuthorizedHandlerContext();
        var request = new ApiRemoveRecordRequest(recordId);
        return await Mediator.ExecuteAsync<ApiRemoveRecordRequest, IActionResult>(context, request, cancellationToken);
    }

    [HttpPut("{recordId:required}")]
    [EndpointSummary("Update record by id")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateAsync(
        [Required] [FromRoute] RecordId recordId,
        [Required] [FromBody] UpdateRecordRequest body,
        CancellationToken cancellationToken = default)
    {
        var context = HttpContext.ToAuthorizedHandlerContext();
        var request = new ApiUpdateRecordRequest(recordId, body);
        return await Mediator.ExecuteAsync<ApiUpdateRecordRequest, IActionResult>(context, request, cancellationToken);
    }
}
