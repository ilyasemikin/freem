using System.Diagnostics.CodeAnalysis;
using Freem.UseCases.Contracts.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;

namespace Freem.Entities.UseCases.Contracts.Users.Password.Update;

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
    
    public static UpdateLoginCredentialsResponse CreateFailure(UpdateLoginCredentialsErrorCode code)
    {
        var error = new Error<UpdateLoginCredentialsErrorCode>(code);
        return new UpdateLoginCredentialsResponse(error);
    }
}