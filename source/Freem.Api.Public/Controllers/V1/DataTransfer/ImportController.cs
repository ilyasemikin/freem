using Freem.Api.Public.Contracts.V1.DataTransfer.Import.Dtos;
using System.ComponentModel.DataAnnotations;
using Freem.Api.Public.Contracts;
using Freem.Api.Public.Contracts.V1;
using Freem.Api.Public.Contracts.V1.DataTransfer.Import;
using Freem.Api.Public.Extensions;
using Freem.Api.Public.Forms.DataTransfer;
using Freem.Api.Public.UseCases.Context;
using Freem.Api.Public.UseCases.Contracts.DataTransfer.Import;
using Freem.Domain.DataTransfer.Import;
using Microsoft.AspNetCore.Mvc;
using Utilities.UseCases.Abstractions;

namespace Freem.Api.Public.Controllers.V1.DataTransfer;

/// <summary>Manages imports: uploaded files parsed into rows that can be reviewed and applied as records.</summary>
[Route("api/v1/data-transfer/imports")]
public sealed class ImportController : BaseController
{
    /// <summary>Largest file accepted for import, in bytes.</summary>
    private const long MaxFileSize = 32 * 1024 * 1024;

    public ImportController(IRequestMediator<ApiAuthorizedHandlerContext> mediator)
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
    [ProducesResponseType<ImportUploadResponse>(StatusCodes.Status201Created)]
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

    /// <summary>Updates the mapping of an import activity to an existing activity of the user.</summary>
    /// <param name="importId">Import id.</param>
    /// <param name="importActivityId">Key of the activity in the imported file.</param>
    /// <param name="body">Existing activity id; null removes the mapping.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpPut("{importId:required}/activities/{importActivityId:required}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status409Conflict)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateActivityMappingAsync(
        [Required] [FromRoute] ImportDocumentId importId,
        [Required] [FromRoute] ImportActivityId importActivityId,
        [Required] [FromBody] ImportActivityMappingUpdateRequest body,
        CancellationToken cancellationToken = default)
    {
        var context = HttpContext.ToAuthorizedHandlerContext();
        var request = new ApiUpdateImportActivityMappingRequest(importId, importActivityId, body);
        return await Mediator.ExecuteAsync<ApiUpdateImportActivityMappingRequest, IActionResult>(context, request, cancellationToken);
    }

    /// <summary>Updates the mapping of an import tag to an existing tag of the user.</summary>
    /// <param name="importId">Import id.</param>
    /// <param name="importTagId">Key of the tag in the imported file.</param>
    /// <param name="body">Existing tag id; null removes the mapping.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpPut("{importId:required}/tags/{importTagId:required}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status409Conflict)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateTagMappingAsync(
        [Required] [FromRoute] ImportDocumentId importId,
        [Required] [FromRoute] ImportTagId importTagId,
        [Required] [FromBody] ImportTagMappingUpdateRequest body,
        CancellationToken cancellationToken = default)
    {
        var context = HttpContext.ToAuthorizedHandlerContext();
        var request = new ApiUpdateImportTagMappingRequest(importId, importTagId, body);
        return await Mediator.ExecuteAsync<ApiUpdateImportTagMappingRequest, IActionResult>(context, request, cancellationToken);
    }

    /// <summary>Creates records from all or the selected rows of the import.</summary>
    /// <param name="importId">Import id.</param>
    /// <param name="body">Exactly one of <c>all</c> and <c>rowIds</c>.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <remarks>
    /// Every import activity and tag used by the selected rows must be mapped to an existing one, otherwise nothing is
    /// written (422 <c>import:unmapped_entities</c>). All records are written in one transaction. Rows from which a record
    /// cannot be created are skipped and listed in the response. Rows are not marked as applied: applying them again
    /// creates duplicates.
    /// </remarks>
    [HttpPost("{importId:required}/apply")]
    [ProducesResponseType<ImportApplyResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status409Conflict)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ApplyAsync(
        [Required] [FromRoute] ImportDocumentId importId,
        [Required] [FromBody] ImportApplyRequest body,
        CancellationToken cancellationToken = default)
    {
        var context = HttpContext.ToAuthorizedHandlerContext();
        var request = new ApiApplyImportRequest(importId, body);
        return await Mediator.ExecuteAsync<ApiApplyImportRequest, IActionResult>(context, request, cancellationToken);
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
    
    /// <summary>Returns a page of the user's imports, newest first.</summary>
    /// <param name="query">Paging: offset and limit.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    ///
    [HttpGet]
    [ProducesResponseType<ImportListResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ListAsync(
        [Required] [FromQuery] ImportListRequest query,
        CancellationToken cancellationToken = default)
    {
        var context = HttpContext.ToAuthorizedHandlerContext();
        var request = new ApiListImportRequest(query);
        return await Mediator.ExecuteAsync<ApiListImportRequest, IActionResult>(context, request, cancellationToken);
    }

    /// <summary>Returns a single import.</summary>
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

    /// <summary>Returns a page of the activities found in the file of an import, with their mappings.</summary>
    /// <param name="importId">Import id.</param>
    /// <param name="query">Paging: offset and limit.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    ///
    [HttpGet("{importId:required}/activities")]
    [ProducesResponseType<ImportActivityListResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ListActivitiesAsync(
        [Required] [FromRoute] ImportDocumentId importId,
        [Required] [FromQuery] ImportActivityListRequest query,
        CancellationToken cancellationToken = default)
    {
        var context = HttpContext.ToAuthorizedHandlerContext();
        var request = new ApiListImportActivityRequest(importId, query);
        return await Mediator.ExecuteAsync<ApiListImportActivityRequest, IActionResult>(context, request, cancellationToken);
    }

    /// <summary>Returns a page of the tags found in the file of an import, with their mappings.</summary>
    /// <param name="importId">Import id.</param>
    /// <param name="query">Paging: offset and limit.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    ///
    [HttpGet("{importId:required}/tags")]
    [ProducesResponseType<ImportTagListResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ListTagsAsync(
        [Required] [FromRoute] ImportDocumentId importId,
        [Required] [FromQuery] ImportTagListRequest query,
        CancellationToken cancellationToken = default)
    {
        var context = HttpContext.ToAuthorizedHandlerContext();
        var request = new ApiListImportTagRequest(importId, query);
        return await Mediator.ExecuteAsync<ApiListImportTagRequest, IActionResult>(context, request, cancellationToken);
    }

    /// <summary>Returns a page of the rows of an import in ascending order of start time.</summary>
    /// <param name="importId">Import id.</param>
    /// <param name="query">Paging: offset and limit.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    ///
    [HttpGet("{importId:required}/rows")]
    [ProducesResponseType<ImportRowListResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ListRowsAsync(
        [Required] [FromRoute] ImportDocumentId importId,
        [Required] [FromQuery] ImportRowListRequest query,
        CancellationToken cancellationToken = default)
    {
        var context = HttpContext.ToAuthorizedHandlerContext();
        var request = new ApiListImportRowRequest(importId, query);
        return await Mediator.ExecuteAsync<ApiListImportRowRequest, IActionResult>(context, request, cancellationToken);
    }
}
