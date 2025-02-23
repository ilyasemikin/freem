using System.ComponentModel.DataAnnotations;
using Freem.Entities.Abstractions.Events;
using Freem.Entities.Abstractions.Identifiers;
using Freem.Entities.Identifiers;
using Freem.Entities.UseCases;
using Freem.Entities.UseCases.Contracts.Events.List;
using Freem.Entities.UseCases.Contracts.Filter;
using Freem.UseCases.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;
using Freem.Web.Api.Public.Contracts;
using Freem.Web.Api.Public.Contracts.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApiListEventRequest = Freem.Web.Api.Public.Contracts.Events.ListEventRequest;
using UseCaseListEventRequest = Freem.Entities.UseCases.Contracts.Events.List.ListEventRequest;

namespace Freem.Web.Api.Public.Controllers.V1.Events;

[Authorize]
[Route("api/v1/events")]
public sealed class ListEventController : BaseController
{
    private readonly UseCaseContextProvider _contextProvider;
    private readonly IUseCaseExecutor<UseCaseExecutionContext> _executor;

    public ListEventController(
        UseCaseContextProvider contextProvider, 
        IUseCaseExecutor<UseCaseExecutionContext> executor)
    {
        ArgumentNullException.ThrowIfNull(contextProvider);
        ArgumentNullException.ThrowIfNull(executor);
        
        _contextProvider = contextProvider;
        _executor = executor;
    }

    [HttpGet]
    public async Task<ActionResult<IAsyncEnumerable<EventResponse>>> ListAsync(
        [Required] [FromQuery] ApiListEventRequest query,
        CancellationToken cancellationToken = default)
    {
        var context = _contextProvider.Get();
        var request = Map(query);

        var response = await _executor.ExecuteAsync<UseCaseListEventRequest, ListEventResponse>(context, request, cancellationToken);

        return response.Success
            ? Ok(CreateSuccess(Response, response.Events, response.TotalCount))
            : CreateFailure(response.Error);
    }

    private static UseCaseListEventRequest Map(ApiListEventRequest request)
    {
        var limit = new Limit(request.Limit);
        return new UseCaseListEventRequest(limit);
    }

    private static async IAsyncEnumerable<EventResponse> CreateSuccess(
        HttpResponse response, IReadOnlyList<IEntityEvent<IEntityIdentifier, UserIdentifier>> events, int totalCount)
    {
        response.Headers.Append(HeaderNames.ItemsCount, events.Count.ToString());
        response.Headers.Append(HeaderNames.TotalItemsCount, totalCount.ToString());
        
        foreach (var @event in events)
            yield return new EventResponse(@event.Id, @event.EntityId, @event.Action);

        await Task.CompletedTask;
    }

    private static ActionResult<IAsyncEnumerable<EventResponse>> CreateFailure(Error<ListEventErrorCode> error)
    {
        throw new NotImplementedException();
    }
}