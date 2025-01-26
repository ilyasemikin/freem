using System.Diagnostics.CodeAnalysis;
using Freem.Entities.Users;
using Freem.UseCases.Contracts.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;

namespace Freem.Entities.UseCases.Contracts.Users.Settings.Get;

public sealed class GetUserSettingsResponse : IResponse<GetUserSettingsErrorCode>
{
    [MemberNotNullWhen(true, nameof(Settings))]
    [MemberNotNullWhen(false, nameof(Error))]
    public bool Success { get; }
    
    public UserSettings? Settings { get; }
    public Error<GetUserSettingsErrorCode>? Error { get; }

    private GetUserSettingsResponse(UserSettings? settings = null, Error<GetUserSettingsErrorCode>? error = null)
    {
        Success = settings is not null;
        Settings = settings;
        Error = error;
    }

    public static GetUserSettingsResponse CreateSuccess(UserSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);
        
        return new GetUserSettingsResponse(settings);
    }

    public static GetUserSettingsResponse CreateFailure(GetUserSettingsErrorCode code, string? message = null)
    {
        var error = new Error<GetUserSettingsErrorCode>(code, message);
        return new GetUserSettingsResponse(error: error);
    }
}