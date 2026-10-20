using Freem.Domain.UseCases.Contracts.Events.Get;
using Freem.Domain.UseCases.Contracts.Events.List;
using Freem.Api.Public.UseCases.Implementations.Mappers.Dtos;
using Freem.Api.Public.UseCases.Implementations.Mappers.Responses.Common;
using Freem.Api.Public.UseCases.Implementations.Results;
using Microsoft.AspNetCore.Mvc;
using Utilities.Errors;
using Utilities.Primitives;
using EventResponse = Freem.Api.Public.Contracts.DTO.Events.EventResponse;

namespace Freem.Api.Public.UseCases.Implementations.Mappers.Responses;

internal static class EventResponseMapper
{
    public static IActionResult Map(this Result<GetEventResponse, Error> result)
    {
        if (!result.Success)
            return result.Error.MapError();

        var response = result.Value.Event.Map();
        return new OkObjectResult(response);
    }

    public static IActionResult Map(this Result<ListEventResponse, Error> result)
    {
        if (!result.Success)
            return result.Error.MapError();

        var events = result.Value.Events
            .Select(@event => @event.Map())
            .ToArray();
        return new ItemsListResult<EventResponse>(events, result.Value.TotalCount);
    }
}
