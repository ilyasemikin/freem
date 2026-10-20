using System.ComponentModel.DataAnnotations;
using Freem.Domain;
using Freem.Domain.Events;
using Freem.Domain.Users;
using Freem.Web.Api.Public.Constants;
using Freem.Web.Api.Public.Contracts;
using Freem.Web.Api.Public.Contracts.DTO;
using Freem.Web.Api.Public.Contracts.DTO.Events;
using Freem.Web.Api.Public.Extensions;
using Freem.Web.Api.Public.OpenApi.Headers;
using Freem.Web.Api.Public.UseCases.Context;
using Freem.Web.Api.Public.UseCases.Contracts.Events;
using Microsoft.AspNetCore.Mvc;
using Utilities.UseCases.Abstractions;

namespace Freem.Web.Api.Public.Controllers.V1;

[Route("api/v1/events")]
[Tags(ControllerTags.Events)]
public sealed class EventsController : BaseController
{
    public EventsController(IRequestMediator<ApiAuthorizedHandlerContext> mediator)
        : base(mediator)
    {
    }

    [HttpGet]
    [EndpointSummary("List events")]
    [ProducesResponseType<IReadOnlyList<IEntityEvent<Id, UserId>>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status500InternalServerError)]
    [ProducesHeader(HeaderNames.ItemsCount, StatusCode = StatusCodes.Status200OK)]
    [ProducesHeader(HeaderNames.TotalItemsCount, StatusCode = StatusCodes.Status200OK)]
    public async Task<IActionResult> ListAsync(
        [Required] [FromQuery] ListEventRequest query,
        CancellationToken cancellationToken = default)
    {
        var context = HttpContext.ToAuthorizedHandlerContext();
        var request = new ApiListEventRequest(query);
        return await Mediator.ExecuteAsync<ApiListEventRequest, IActionResult>(context, request, cancellationToken);
    }
}
