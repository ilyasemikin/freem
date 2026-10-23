using System.ComponentModel.DataAnnotations;
using Freem.Api.Public.Constants;
using Freem.Api.Public.Contracts;
using Freem.Api.Public.Contracts.DTO;
using Freem.Api.Public.Contracts.DTO.Imports;
using Freem.Api.Public.Extensions;
using Freem.Api.Public.Forms;
using Freem.Api.Public.OpenApi.Headers;
using Freem.Api.Public.UseCases.Context;
using Freem.Api.Public.UseCases.Contracts.Imports;
using Freem.Domain.Import;
using Microsoft.AspNetCore.Mvc;
using Utilities.UseCases.Abstractions;

namespace Freem.Api.Public.Controllers.V1;

/// <summary>Manages imports: uploaded files parsed into rows that can be reviewed.</summary>
[Route("api/v1/imports")]
[Tags(ControllerTags.Imports)]
public sealed class ImportsController : BaseController
{
    /// <summary>Largest file accepted for import, in bytes.</summary>
    public const long MaxFileSize = 32 * 1024 * 1024;

    public ImportsController(IRequestMediator<ApiAuthorizedHandlerContext> mediator)
        : base(mediator)
    {
    }

    /// <summary>Uploads a file, parses it and saves the import with its rows.</summary>
    /// <param name="form">Format key, optional time zone offset and the file.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <remarks>The file itself is not stored. Rows that were skipped or changed while parsing are listed in the warnings of the response and are not stored; the import keeps the number of rows in the file and the number of parsed rows.</remarks>
    [HttpPost]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(MaxFileSize)]
    [RequestFormLimits(MultipartBodyLengthLimit = MaxFileSize)]
    [ProducesResponseType<UploadImportResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UploadAsync(
        [Required] [FromForm] UploadImportForm form,
        CancellationToken cancellationToken = default)
    {
        var context = HttpContext.ToAuthorizedHandlerContext();
        var fileName = Path.GetFileName(form.File.FileName);
        await using var content = form.File.OpenReadStream();
        var request = new ApiUploadImportRequest(form.Format, fileName, form.TimeZoneOffset, content);
        return await Mediator.ExecuteAsync<ApiUploadImportRequest, IActionResult>(context, request, cancellationToken);
    }

    /// <summary>Returns a page of the user's imports, newest first.</summary>
    /// <param name="query">Paging: offset and limit.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <remarks>The page size and the total count are returned in the <c>X-ITEMS-COUNT</c> and <c>X-TOTAL-ITEMS-COUNT</c> headers.</remarks>
    [HttpGet]
    [ProducesResponseType<IReadOnlyList<ImportResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status500InternalServerError)]
    [ProducesHeader(HeaderNames.ItemsCount, StatusCode = StatusCodes.Status200OK)]
    [ProducesHeader(HeaderNames.TotalItemsCount, StatusCode = StatusCodes.Status200OK)]
    public async Task<IActionResult> ListAsync(
        [Required] [FromQuery] ListImportRequest query,
        CancellationToken cancellationToken = default)
    {
        var context = HttpContext.ToAuthorizedHandlerContext();
        var request = new ApiListImportRequest(query);
        return await Mediator.ExecuteAsync<ApiListImportRequest, IActionResult>(context, request, cancellationToken);
    }

    /// <summary>Returns a single import with the activities and tags found in the file.</summary>
    /// <param name="importId">Import id.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpGet("{importId:required}")]
    [ProducesResponseType<ImportResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAsync(
        [Required] [FromRoute] ImportDocumentId importId,
        CancellationToken cancellationToken = default)
    {
        var context = HttpContext.ToAuthorizedHandlerContext();
        var request = new ApiGetImportRequest(importId);
        return await Mediator.ExecuteAsync<ApiGetImportRequest, IActionResult>(context, request, cancellationToken);
    }

    /// <summary>Returns a page of the rows of an import in ascending order of start time.</summary>
    /// <param name="importId">Import id.</param>
    /// <param name="query">Paging: offset and limit.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <remarks>The page size and the total count are returned in the <c>X-ITEMS-COUNT</c> and <c>X-TOTAL-ITEMS-COUNT</c> headers.</remarks>
    [HttpGet("{importId:required}/rows")]
    [ProducesResponseType<IReadOnlyList<ImportRowResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status500InternalServerError)]
    [ProducesHeader(HeaderNames.ItemsCount, StatusCode = StatusCodes.Status200OK)]
    [ProducesHeader(HeaderNames.TotalItemsCount, StatusCode = StatusCodes.Status200OK)]
    public async Task<IActionResult> ListRowsAsync(
        [Required] [FromRoute] ImportDocumentId importId,
        [Required] [FromQuery] ListImportRowRequest query,
        CancellationToken cancellationToken = default)
    {
        var context = HttpContext.ToAuthorizedHandlerContext();
        var request = new ApiListImportRowRequest(importId, query);
        return await Mediator.ExecuteAsync<ApiListImportRowRequest, IActionResult>(context, request, cancellationToken);
    }

    /// <summary>Permanently removes an import together with its rows.</summary>
    /// <param name="importId">Import id.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpDelete("{importId:required}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemoveAsync(
        [Required] [FromRoute] ImportDocumentId importId,
        CancellationToken cancellationToken = default)
    {
        var context = HttpContext.ToAuthorizedHandlerContext();
        var request = new ApiRemoveImportRequest(importId);
        return await Mediator.ExecuteAsync<ApiRemoveImportRequest, IActionResult>(context, request, cancellationToken);
    }
}
