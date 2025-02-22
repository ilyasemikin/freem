namespace Freem.Web.Api.Public.Contracts.Users.Tokens;

public sealed class RefreshTokensResponse
{
    public string AccessToken { get; }
    public string RefreshToken { get; }
    
    public RefreshTokensResponse(string accessToken, string refreshToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(accessToken);
        ArgumentException.ThrowIfNullOrWhiteSpace(refreshToken);
        
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }
}