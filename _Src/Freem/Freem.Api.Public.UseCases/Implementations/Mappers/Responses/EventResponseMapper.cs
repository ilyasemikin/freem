using Freem.Domain;
using Freem.Domain.Events;
using Freem.Domain.Users;
using Freem.Domain.UseCases.Contracts.Events.List;
using Freem.Api.Public.UseCases.Implementations.Mappers.Responses.Common;
using Freem.Api.Public.UseCases.Implementations.Results;
using Microsoft.AspNetCore.Mvc;
using Utilities.Errors;
using Utilities.Primitives;

namespace Freem.Api.Public.UseCases.Implementations.Mappers.Responses;

internal static class EventResponseMapper
{
    public static IActionResult Map(this Result<ListEventResponse, Error> result)
    {
        if (!result.Success)
            return result.Error.MapError();

        return new ItemsListResult<IEntityEvent<Id, UserId>>(result.Value.Events, result.Value.TotalCount);
    }
}
