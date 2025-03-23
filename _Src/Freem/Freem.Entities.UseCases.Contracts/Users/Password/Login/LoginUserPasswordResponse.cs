using System.Diagnostics.CodeAnalysis;
using Freem.Entities.Users;
using Freem.UseCases.Contracts.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;

namespace Freem.Entities.UseCases.Contracts.Users.Password.Login;

public sealed class LoginUserPasswordResponse : IResponse<LoginUserPasswordErrorCode>
{
    [MemberNotNullWhen(true, nameof(Tokens))]
    [MemberNotNullWhen(false, nameof(Error))]
    public bool Success { get; }
    
    public UserTokens? Tokens { get; }
    
    public Error<LoginUserPasswordErrorCode>? Error { get; }

    private LoginUserPasswordResponse(UserTokens? tokens = null, Error<LoginUserPasswordErrorCode>? error = null)
    {
        Success = error is null;
        Tokens = tokens;
        Error = error;
    }
    
    public static LoginUserPasswordResponse CreateSuccess(UserTokens tokens)
    {
        ArgumentNullException.ThrowIfNull(tokens);
        
        return new LoginUserPasswordResponse(tokens);
    }
    
    public static LoginUserPasswordResponse CreateFailure(LoginUserPasswordErrorCode code, string? message = null)
    {
        var error = new Error<LoginUserPasswordErrorCode>(code, message);
        return new LoginUserPasswordResponse(error: error);
    }
}