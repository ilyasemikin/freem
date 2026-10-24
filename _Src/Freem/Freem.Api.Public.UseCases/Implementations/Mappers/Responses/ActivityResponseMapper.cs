using Freem.Api.Public.Contracts.V1.Activities;
using Freem.Api.Public.Contracts.V1.Activities.Dtos;
using Freem.Domain.UseCases.Contracts.Activities.Create;
using Freem.Domain.UseCases.Contracts.Activities.Search;
using Freem.Domain.UseCases.Contracts.Activities.Get;
using Freem.Domain.UseCases.Contracts.Activities.List;
using Freem.Api.Public.UseCases.Implementations.Mappers.Dtos;
using Freem.Api.Public.UseCases.Implementations.Mappers.Responses.Common;
using Microsoft.AspNetCore.Mvc;
using Utilities.Errors;
using Utilities.Primitives;
using ActivityDto = Freem.Api.Public.Contracts.V1.Activities.Dtos.ActivityDto;
using ApiActivityCreateResponse = Freem.Api.Public.Contracts.V1.Activities.ActivityCreateResponse;

namespace Freem.Api.Public.UseCases.Implementations.Mappers.Responses;

internal static class ActivityResponseMapper
{
    public static IActionResult Map(this Result<CreateActivityResponse, Error> result)
    {
        if (!result.Success)
            return result.Error.MapError();

        var response = new ApiActivityCreateResponse(result.Value.Activity.Id);
        return new CreatedResult
        {
            Value = response
        };
    }

    public static IActionResult Map(this Result<SearchActivityByNameResponse, Error> result)
    {
        if (!result.Success)
            return result.Error.MapError();

        var activities = result.Value.Activities
            .Select(activity => activity.Map())
            .ToArray();
        var response = new ActivityListResponse(result.Value.TotalCount, activities);
        return new OkObjectResult(response);
    }

    public static IActionResult Map(this Result<GetActivityResponse, Error> result)
    {
        if (!result.Success)
            return result.Error.MapError();

        var activity = result.Value.Activity.Map();
        var response = new ActivityResponse(activity);
        return new OkObjectResult(response);
    }

    public static IActionResult Map(this Result<ListActivityResponse, Error> result)
    {
        if (!result.Success)
            return result.Error.MapError();

        var activities = result.Value.Activities
            .Select(activity => activity.Map())
            .ToArray();
        var response = new ActivityListResponse(result.Value.TotalCount, activities);
        return new OkObjectResult(response);
    }
}
