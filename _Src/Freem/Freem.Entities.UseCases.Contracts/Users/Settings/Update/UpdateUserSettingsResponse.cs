using System.Diagnostics.CodeAnalysis;
using Freem.UseCases.Contracts.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;

namespace Freem.Entities.UseCases.Contracts.Users.Settings.Update;

public sealed class UpdateUserSettingsResponse : IResponse<UpdateUserSettingsErrorCode>
{
    [MemberNotNullWhen(false, nameof(Error))]
    public bool Success { get; }
    
    public Error<UpdateUserSettingsErrorCode>? Error { get; }

    private UpdateUserSettingsResponse(Error<UpdateUserSettingsErrorCode>? error = null)
    {
        Success = error is null;
        Error = error;
    }

    public static UpdateUserSettingsResponse CreateSuccess()
    {
        return new UpdateUserSettingsResponse();
    }

    public static UpdateUserSettingsResponse CreateFailure(UpdateUserSettingsErrorCode code, string? message = null)
    {
        var error = new Error<UpdateUserSettingsErrorCode>(code, message);
        return new UpdateUserSettingsResponse(error);
    }
}