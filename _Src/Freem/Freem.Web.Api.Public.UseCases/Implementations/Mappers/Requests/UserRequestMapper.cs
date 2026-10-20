using Freem.Entities.UseCases.Contracts.Users.Get;
using Freem.Entities.UseCases.Contracts.Users.Register;
using Freem.Entities.UseCases.Contracts.Users.Settings.Get;
using Freem.Entities.UseCases.Contracts.Users.Settings.Update;
using Freem.Web.Api.Public.UseCases.Contracts.Users;
using Freem.Web.Api.Public.UseCases.Contracts.Users.Settings;

namespace Freem.Web.Api.Public.UseCases.Implementations.Mappers.Requests;

internal static class UserRequestMapper
{
    public static GetUserRequest Map(this ApiGetUserRequest request)
    {
        return GetUserRequest.Instance;
    }

    public static RegisterUserRequest Map(this ApiRegisterUserRequest request)
    {
        return new RegisterUserRequest(request.Body.Nickname);
    }

    public static GetUserSettingsRequest Map(this ApiGetUserSettingsRequest request)
    {
        return GetUserSettingsRequest.Instance;
    }

    public static UpdateUserSettingsRequest Map(this ApiUpdateUserSettingsRequest request)
    {
        return new UpdateUserSettingsRequest
        {
            UtcOffset = request.Body.DayUtcOffset?.Map()
        };
    }
}
