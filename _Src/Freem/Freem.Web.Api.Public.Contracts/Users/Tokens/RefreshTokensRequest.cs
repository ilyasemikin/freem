namespace Freem.Web.Api.Public.Contracts.Users.Tokens;

public sealed class RefreshTokensRequest
{
    public string RefreshToken { get; }
    
    public RefreshTokensRequest(string refreshToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(refreshToken);
        
        RefreshToken = refreshToken;
    }
}