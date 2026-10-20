using System.ComponentModel.DataAnnotations;
using Freem.Domain;
using Freem.Domain.Events;
using Freem.Domain.Users;
using Freem.Entities.UseCases.Context;
using Freem.Entities.UseCases.Contracts.Events.List;
using Freem.Entities.UseCases.Contracts.Filter;
using Freem.Web.Api.Public.Constants;
using Freem.Web.Api.Public.Contracts;
using Freem.Web.Api.Public.Services.Implementations;
using Microsoft.AspNetCore.Mvc;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;
using ApiListEventRequest = Freem.Web.Api.Public.Contracts.DTO.Events.ListEventRequest;
using UseCaseListEventRequest = Freem.Entities.UseCases.Contracts.Events.List.ListEventRequest;

namespace Freem.Web.Api.Public.Controllers.V1.Events;

[Route("api/v1/events")]
[Tags(ControllerTags.Events)]
[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IAsyncEnumerable<IEntityEvent<Id, UserId>>))]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public sealed class ListEventController : BaseController
{
    private readonly UseCaseContextProvider _contextProvider;
    private readonly IRequestMediator<HandlerContext> _mediator;

    public ListEventController(
        UseCaseContextProvider contextProvider, 
        IRequestMediator<HandlerContext> mediator)
    {
        ArgumentNullException.ThrowIfNull(contextProvider);
        ArgumentNullException.ThrowIfNull(mediator);
        
        _contextProvider = contextProvider;
        _mediator = mediator;
    }

    [HttpGet]
    [EndpointSummary("List events")]
    public async Task<IActionResult> ListAsync(
        [Required] [FromQuery] ApiListEventRequest query,
        CancellationToken cancellationToken = default)
    {
        if (!_contextProvider.TryGet(out var context))
            return new UnauthorizedResult();

        var request = Map(query);

        var response = await _mediator.ExecuteAsync<UseCaseListEventRequest, Result<ListEventResponse, Error>>(context, request, cancellationToken);

        return response.Success
            ? CreateSuccess(Response, response.Value.Events, response.Value.TotalCount)
            : CreateFailure(response.Error);
    }

    private static UseCaseListEventRequest Map(ApiListEventRequest request)
    {
        var limit = new Limit(request.Limit);
        return new UseCaseListEventRequest(limit)
        {
            After = request.After
        };
    }

    private static IActionResult CreateSuccess(
        HttpResponse response, IReadOnlyList<IEntityEvent<Id, UserId>> events, int totalCount)
    {
        response.Headers.Append(HeaderNames.ItemsCount, events.Count.ToString());
        response.Headers.Append(HeaderNames.TotalItemsCount, totalCount.ToString());

        return new OkObjectResult(events);
    }

    private static IActionResult CreateFailure(Error error)
    {
        return new StatusCodeResult(StatusCodes.Status500InternalServerError);
    }
}