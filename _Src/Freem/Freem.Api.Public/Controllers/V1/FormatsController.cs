using Freem.Api.Public.Constants;
using Freem.Api.Public.Contracts.DTO;
using Freem.Api.Public.Contracts.DTO.DataTransfer;
using Freem.Api.Public.Extensions;
using Freem.Api.Public.UseCases.Context;
using Freem.Api.Public.UseCases.Contracts.DataTransfer;
using Microsoft.AspNetCore.Mvc;
using Utilities.UseCases.Abstractions;

namespace Freem.Api.Public.Controllers.V1;

/// <summary>Lists data formats supported for export and import.</summary>
[Route("api/v1/formats")]
[Tags(ControllerTags.DataTransfer)]
public sealed class FormatsController : BaseController
{
    public FormatsController(IRequestMediator<ApiAuthorizedHandlerContext> mediator)
        : base(mediator)
    {
    }

    /// <summary>Returns the formats the user's data can be exported to and imported from.</summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpGet]
    [ProducesResponseType<ListFormatsResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ListAsync(CancellationToken cancellationToken = default)
    {
        var context = HttpContext.ToAuthorizedHandlerContext();
        var request = new ApiListFormatsRequest();
        return await Mediator.ExecuteAsync<ApiListFormatsRequest, IActionResult>(context, request, cancellationToken);
    }
}
