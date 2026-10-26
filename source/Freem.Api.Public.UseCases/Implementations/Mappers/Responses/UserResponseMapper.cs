using Freem.Api.Public.Contracts.V1.Users;
using Freem.Api.Public.Contracts.V1.Users.Settings;
using Freem.Domain.UseCases.Contracts.Users.Get;
using Freem.Domain.UseCases.Contracts.Users.Settings.Get;
using Freem.Api.Public.UseCases.Implementations.Mappers.Responses.Common;
using Microsoft.AspNetCore.Mvc;
using Utilities.Errors;
using Utilities.Primitives;
using ApiUserRegisterResponse = Freem.Api.Public.Contracts.V1.Users.UserRegisterResponse;
using RegisterUserResponse = Freem.Domain.UseCases.Contracts.Users.Register.RegisterUserResponse;

namespace Freem.Api.Public.UseCases.Implementations.Mappers.Responses;

internal static class UserResponseMapper
{
    public static IActionResult Map(this Result<GetUserResponse, Error> result)
    {
        if (!result.Success)
            return result.Error.MapError();

        var user = result.Value.User;
        var response = new UserResponse(user.Id, user.Nickname);
        return new OkObjectResult(response);
    }

    public static IActionResult Map(this Result<RegisterUserResponse, Error> result)
    {
        if (!result.Success)
            return result.Error.MapError();

        var response = new ApiUserRegisterResponse(result.Value.UserId);
        return new OkObjectResult(response);
    }

    public static IActionResult Map(this Result<GetUserSettingsResponse, Error> result)
    {
        if (!result.Success)
            return result.Error.MapError();

        var settings = result.Value.Settings;
        var response = new UserSettingsResponse(settings.TimeZoneOffset, settings.WeekStart);
        return new OkObjectResult(response);
    }
}
