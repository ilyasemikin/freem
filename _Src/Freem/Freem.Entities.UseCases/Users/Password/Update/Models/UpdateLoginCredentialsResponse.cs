using System.Diagnostics.CodeAnalysis;
using Freem.Entities.UseCases.Abstractions.Models.Errors;

namespace Freem.Entities.UseCases.Users.Password.Update.Models;

public sealed class UpdateLoginCredentialsResponse
{
    [MemberNotNullWhen(false, nameof(Error))]
    public bool Success { get; }
    
    public Error<UpdateLoginCredentialsErrorCode>? Error { get; }

    private UpdateLoginCredentialsResponse(Error<UpdateLoginCredentialsErrorCode>? error = null)
    {
        Success = error is null;
        Error = error;
    }
    
    public static UpdateLoginCredentialsResponse CreateSuccess()
    {
        return new UpdateLoginCredentialsResponse();
    }
    
    public static UpdateLoginCredentialsResponse CreateFailure(UpdateLoginCredentialsErrorCode code, string? message = null)
    {
        var error = new Error<UpdateLoginCredentialsErrorCode>(code, message);
        return new UpdateLoginCredentialsResponse(error);
    }
}