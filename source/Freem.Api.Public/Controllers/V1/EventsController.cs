using Freem.Api.Public.Contracts.V1.Events.Dtos;
using System.ComponentModel.DataAnnotations;
using EventId = Freem.Domain.Events.EventId;
using Freem.Api.Public.Contracts;
using Freem.Api.Public.Contracts.V1;
using Freem.Api.Public.Contracts.V1.Events;
using Freem.Api.Public.Extensions;
using Freem.Api.Public.UseCases.Context;
using Freem.Api.Public.UseCases.Contracts.Events;
using Microsoft.AspNetCore.Mvc;
using Utilities.UseCases.Abstractions;

namespace Freem.Api.Public.Controllers.V1;

/// <summary>Read access to the user's event log: what changed and when.</summary>
[Route("api/v1/events")]
public sealed class EventsController : BaseController
{
    public EventsController(IRequestMediator<ApiAuthorizedHandlerContext> mediator)
        : base(mediator)
    {
    }

    /// <summary>Returns a page of events, optionally filtered by entity and time range.</summary>
    /// <param name="query">Paging and filters.</param>
    ///
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpGet]
    [ProducesResponseType<EventListResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ListAsync(
        [Required] [FromQuery] EventListRequest query,
        CancellationToken cancellationToken = default)
    {
        var context = HttpContext.ToAuthorizedHandlerContext();
        var request = new ApiListEventRequest(query);
        return await Mediator.ExecuteAsync<ApiListEventRequest, IActionResult>(context, request, cancellationToken);
    }

    /// <summary>Returns a single event.</summary>
    /// <param name="eventId">Event id.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    [HttpGet("{eventId:required}")]
    [ProducesResponseType<EventResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ErrorResponse>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAsync(
        [Required] [FromRoute] EventId eventId,
        CancellationToken cancellationToken = default)
    {
        var context = HttpContext.ToAuthorizedHandlerContext();
        var request = new ApiGetEventRequest(eventId);
        return await Mediator.ExecuteAsync<ApiGetEventRequest, IActionResult>(context, request, cancellationToken);
    }
}
