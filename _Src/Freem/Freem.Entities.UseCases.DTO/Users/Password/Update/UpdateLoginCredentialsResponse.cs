using System.Diagnostics.CodeAnalysis;
using Freem.Entities.UseCases.DTO.Abstractions;
using Freem.Entities.UseCases.DTO.Abstractions.Models.Errors;

namespace Freem.Entities.UseCases.DTO.Users.Password.Update;

public sealed class UpdateLoginCredentialsResponse : IResponse<UpdateLoginCredentialsErrorCode>
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