using Freem.Domain.UseCases.Contracts.Activities.Create;
using Freem.Domain.UseCases.Contracts.Activities.Find;
using Freem.Domain.UseCases.Contracts.Activities.Get;
using Freem.Domain.UseCases.Contracts.Activities.List;
using Freem.Api.Public.UseCases.Implementations.Mappers.Dtos;
using Freem.Api.Public.UseCases.Implementations.Mappers.Responses.Common;
using Freem.Api.Public.UseCases.Implementations.Results;
using Microsoft.AspNetCore.Mvc;
using Utilities.Errors;
using Utilities.Primitives;
using ActivityResponse = Freem.Api.Public.Contracts.DTO.Activities.ActivityResponse;
using ApiCreateActivityResponse = Freem.Api.Public.Contracts.DTO.Activities.CreateActivityResponse;

namespace Freem.Api.Public.UseCases.Implementations.Mappers.Responses;

internal static class ActivityResponseMapper
{
    public static IActionResult Map(this Result<CreateActivityResponse, Error> result)
    {
        if (!result.Success)
            return result.Error.MapError();

        var response = new ApiCreateActivityResponse(result.Value.Activity.Id);
        return new CreatedResult
        {
            Value = response
        };
    }

    public static IActionResult Map(this Result<FindActivityResponse, Error> result)
    {
        if (!result.Success)
            return result.Error.MapError();

        var activities = result.Value.Activities
            .Select(activity => activity.Map())
            .ToArray();
        return new ItemsListResult<ActivityResponse>(activities, result.Value.TotalCount);
    }

    public static IActionResult Map(this Result<GetActivityResponse, Error> result)
    {
        if (!result.Success)
            return result.Error.MapError();

        var response = result.Value.Activity.Map();
        return new OkObjectResult(response);
    }

    public static IActionResult Map(this Result<ListActivityResponse, Error> result)
    {
        if (!result.Success)
            return result.Error.MapError();

        var activities = result.Value.Activities
            .Select(activity => activity.Map())
            .ToArray();
        return new ItemsListResult<ActivityResponse>(activities, result.Value.TotalCount);
    }
}
