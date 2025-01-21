using System.Diagnostics.CodeAnalysis;
using Freem.UseCases.Contracts.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;

namespace Freem.Entities.UseCases.Contracts.Users.Tokens.Refresh;

public class RefreshUserAccessTokenResponse : IResponse<RefreshUserAccessTokenErrorCode>
{
    [MemberNotNullWhen(true, nameof(AccessToken))]
    [MemberNotNullWhen(true, nameof(RefreshToken))]
    [MemberNotNullWhen(false, nameof(Error))]
    public bool Success { get; }
    
    public string? AccessToken { get; }
    public string? RefreshToken { get; }
    
    public Error<RefreshUserAccessTokenErrorCode>? Error { get; }

    private RefreshUserAccessTokenResponse(
        string? accessToken = null, 
        string? refreshToken = null, 
        Error<RefreshUserAccessTokenErrorCode>? error = null)
    {
        Success = error is null;
        AccessToken = accessToken;
        RefreshToken = refreshToken;
        Error = error;
    }
    
    public static RefreshUserAccessTokenResponse CreateSuccess(string accessToken, string refreshToken)
    {
        ArgumentException.ThrowIfNullOrEmpty(accessToken);
        ArgumentException.ThrowIfNullOrEmpty(refreshToken);
        
        return new RefreshUserAccessTokenResponse(accessToken, refreshToken);
    }
    
    public static RefreshUserAccessTokenResponse CreateFailure(RefreshUserAccessTokenErrorCode code, string? message = null)
    {
        var error = new Error<RefreshUserAccessTokenErrorCode>(code, message);
        return new RefreshUserAccessTokenResponse(error: error);
    }
}