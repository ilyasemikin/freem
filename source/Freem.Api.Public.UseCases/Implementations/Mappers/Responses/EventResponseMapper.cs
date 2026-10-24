using Freem.Api.Public.Contracts.V1.Events;
using Freem.Api.Public.Contracts.V1.Events.Dtos;
using Freem.Domain.UseCases.Contracts.Events.Get;
using Freem.Domain.UseCases.Contracts.Events.List;
using Freem.Api.Public.UseCases.Implementations.Mappers.Dtos;
using Freem.Api.Public.UseCases.Implementations.Mappers.Responses.Common;
using Microsoft.AspNetCore.Mvc;
using Utilities.Errors;
using Utilities.Primitives;
using EventDto = Freem.Api.Public.Contracts.V1.Events.Dtos.EventDto;

namespace Freem.Api.Public.UseCases.Implementations.Mappers.Responses;

internal static class EventResponseMapper
{
    public static IActionResult Map(this Result<GetEventResponse, Error> result)
    {
        if (!result.Success)
            return result.Error.MapError();

        var @event = result.Value.Event.Map();
        var response = new EventResponse(@event);
        return new OkObjectResult(response);
    }

    public static IActionResult Map(this Result<ListEventResponse, Error> result)
    {
        if (!result.Success)
            return result.Error.MapError();

        var events = result.Value.Events
            .Select(@event => @event.Map())
            .ToArray();
        var response = new EventListResponse(result.Value.TotalCount, events);
        return new OkObjectResult(response);
    }
}
