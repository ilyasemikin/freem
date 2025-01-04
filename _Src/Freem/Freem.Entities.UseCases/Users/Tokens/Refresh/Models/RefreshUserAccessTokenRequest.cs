namespace Freem.Entities.UseCases.Users.Tokens.Refresh.Models;

public sealed class RefreshUserAccessTokenRequest
{
    public string RefreshToken { get; }
    
    public RefreshUserAccessTokenRequest(string refreshToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(refreshToken);
        
        RefreshToken = refreshToken;
    }
}