using System.ComponentModel.DataAnnotations;
using Freem.Api.Public.Constants;
using Freem.Api.Public.Contracts.DTO;
using Freem.Api.Public.Extensions;
using Freem.Api.Public.UseCases.Context;
using Freem.Api.Public.UseCases.Contracts.DataTransfer;
using Freem.Domain.Formats;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Utilities.UseCases.Abstractions;

namespace Freem.Api.Public.Controllers.V1;

/// <summary>Exports all of the current user's data to a file.</summary>
[Route("api/v1/export")]
[Tags(ControllerTags.DataTransfer)]
public sealed class ExportController : BaseController
{
    public ExportController(IRequestMediator<ApiAuthorizedHandlerContext> mediator)
        : base(mediator)
    {
    }

    /// <summary>Returns a file with all of the user's tags, activities and records in the given format.</summary>
    /// <param name="format">Format key, see the list of formats.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <remarks>The file is streamed. If reading fails after the response has started, the connection is aborted instead of returning an error.</remarks>
    [HttpGet("{format:required}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ExportAsync(
        [Required] [FromRoute] FormatKey format,
        CancellationToken cancellationToken = default)
    {
        var context = HttpContext.ToAuthorizedHandlerContext();
        var request = new ApiExportDataRequest(format, OpenOutput);
        var result = await Mediator.ExecuteAsync<ApiExportDataRequest, IActionResult>(context, request, cancellationToken);
        if (Response.HasStarted && result is not EmptyResult)
        {
            HttpContext.Abort();
            return new EmptyResult();
        }

        return result;
    }

    private Stream OpenOutput(FormatDescription format)
    {
        var timestamp = DateTimeOffset.UtcNow.ToString("yyyyMMdd-HHmmss");
        var disposition = new ContentDispositionHeaderValue("attachment")
        {
            FileName = $"freem-export-{timestamp}.{format.FileExtension}"
        };

        Response.StatusCode = StatusCodes.Status200OK;
        Response.ContentType = format.MediaType.ToString();
        Response.Headers[HeaderNames.ContentDisposition] = disposition.ToString();
        return Response.Body;
    }
}
