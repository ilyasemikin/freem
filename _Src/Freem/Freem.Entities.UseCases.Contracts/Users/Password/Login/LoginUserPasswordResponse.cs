using System.Diagnostics.CodeAnalysis;
using Freem.UseCases.Contracts.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;

namespace Freem.Entities.UseCases.Contracts.Users.Password.Login;

public sealed class LoginUserPasswordResponse : IResponse<LoginUserPasswordErrorCode>
{
    [MemberNotNullWhen(true, nameof(AccessToken))]
    [MemberNotNullWhen(true, nameof(RefreshToken))]
    [MemberNotNullWhen(false, nameof(Error))]
    public bool Success { get; }
    
    public string? AccessToken { get; }
    public string? RefreshToken { get; }
    
    public Error<LoginUserPasswordErrorCode>? Error { get; }

    private LoginUserPasswordResponse(string? accessToken = null, string? refreshToken = null, Error<LoginUserPasswordErrorCode>? error = null)
    {
        Success = error is null;
        AccessToken = accessToken;
        RefreshToken = refreshToken;
        Error = error;
    }
    
    public static LoginUserPasswordResponse CreateSuccess(string accessToken, string refreshToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(accessToken);
        ArgumentException.ThrowIfNullOrWhiteSpace(refreshToken);
        
        return new LoginUserPasswordResponse(accessToken, refreshToken);
    }
    
    public static LoginUserPasswordResponse CreateFailure(LoginUserPasswordErrorCode code, string? message = null)
    {
        var error = new Error<LoginUserPasswordErrorCode>(code, message);
        return new LoginUserPasswordResponse(error: error);
    }
}