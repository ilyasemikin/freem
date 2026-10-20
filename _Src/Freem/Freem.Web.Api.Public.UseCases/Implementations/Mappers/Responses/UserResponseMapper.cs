using Freem.Entities.UseCases.Contracts.Users.Get;
using Freem.Entities.UseCases.Contracts.Users.Settings.Get;
using Freem.Web.Api.Public.Contracts.DTO.Users;
using Freem.Web.Api.Public.Contracts.DTO.Users.Settings;
using Freem.Web.Api.Public.UseCases.Implementations.Mappers.Responses.Common;
using Microsoft.AspNetCore.Mvc;
using Utilities.Errors;
using Utilities.Primitives;
using ApiRegisterUserResponse = Freem.Web.Api.Public.Contracts.DTO.Users.RegisterUserResponse;
using RegisterUserResponse = Freem.Entities.UseCases.Contracts.Users.Register.RegisterUserResponse;

namespace Freem.Web.Api.Public.UseCases.Implementations.Mappers.Responses;

internal static class UserResponseMapper
{
    public static IActionResult Map(this Result<GetUserResponse, Error> result)
    {
        if (!result.Success)
            return result.Error.MapError();

        var user = result.Value.User;
        var response = new MeResponse(user.Id, user.Nickname);
        return new OkObjectResult(response);
    }

    public static IActionResult Map(this Result<RegisterUserResponse, Error> result)
    {
        if (!result.Success)
            return result.Error.MapError();

        var response = new ApiRegisterUserResponse(result.Value.UserId);
        return new OkObjectResult(response);
    }

    public static IActionResult Map(this Result<GetUserSettingsResponse, Error> result)
    {
        if (!result.Success)
            return result.Error.MapError();

        var response = new UserSettingsResponse(result.Value.Settings.DayUtcOffset);
        return new OkObjectResult(response);
    }
}
