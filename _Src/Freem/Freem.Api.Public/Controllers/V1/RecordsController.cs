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

/// <summary>Manages finished records: completed time intervals spent on activities.</summary>
[Route("api/v1/records")]
[Tags(ControllerTags.Records)]
public sealed class RecordsController : BaseController
{
    public RecordsController(IRequestMediator<ApiAuthorizedHandlerContext> mediator)
        : base(mediator)
    {
    }

    /// <summary>Creates a finished record for an explicit period.</summary>
    /// <param name="body">Period, activities, and optional name, description and tags.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The id of the created record.</returns>
    [HttpPost]
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

    /// <summary>Returns a page of the user's records.</summary>
    /// <param name="query">Paging: offset and limit.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <remarks>The page size and the total count are returned in the <c>X-ITEMS-COUNT</c> and <c>X-TOTAL-ITEMS-COUNT</c> headers.</remarks>
    [HttpGet]
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

    /// <summary>Returns records whose start or end falls within the period (start inclusive, end exclusive), up to the limit.</summary>
    /// <param name="query">Period and the maximum number of records.</param>
    /// <remarks>The page size and the total count are returned in the <c>X-ITEMS-COUNT</c> and <c>X-TOTAL-ITEMS-COUNT</c> headers.</remarks>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpGet("by-period")]
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

    /// <summary>Returns a single record.</summary>
    /// <param name="recordId">Record id.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpGet("{recordId:required}")]
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

    /// <summary>Permanently removes a record.</summary>
    /// <param name="recordId">Record id.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpDelete("{recordId:required}")]
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

    /// <summary>Partially updates a record: only the fields present in the body are changed.</summary>
    /// <param name="recordId">Record id.</param>
    /// <param name="body">Fields to change.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpPut("{recordId:required}")]
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
