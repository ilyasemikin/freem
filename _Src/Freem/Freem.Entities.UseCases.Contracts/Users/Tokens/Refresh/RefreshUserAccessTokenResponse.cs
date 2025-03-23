using System.Diagnostics.CodeAnalysis;
using Freem.Entities.Users;
using Freem.UseCases.Contracts.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;

namespace Freem.Entities.UseCases.Contracts.Users.Tokens.Refresh;

public class RefreshUserAccessTokenResponse : IResponse<RefreshUserAccessTokenErrorCode>
{
    [MemberNotNullWhen(true, nameof(Tokens))]
    [MemberNotNullWhen(false, nameof(Error))]
    public bool Success { get; }
    
    public UserTokens? Tokens { get; }
    
    public Error<RefreshUserAccessTokenErrorCode>? Error { get; }

    private RefreshUserAccessTokenResponse(
        UserTokens? tokens = null,
        Error<RefreshUserAccessTokenErrorCode>? error = null)
    {
        Success = error is null;
        Tokens = tokens;
        Error = error;
    }
    
    public static RefreshUserAccessTokenResponse CreateSuccess(UserTokens tokens)
    {
        ArgumentNullException.ThrowIfNull(tokens);
        
        return new RefreshUserAccessTokenResponse(tokens);
    }
    
    public static RefreshUserAccessTokenResponse CreateFailure(RefreshUserAccessTokenErrorCode code, string? message = null)
    {
        var error = new Error<RefreshUserAccessTokenErrorCode>(code, message);
        return new RefreshUserAccessTokenResponse(error: error);
    }
}