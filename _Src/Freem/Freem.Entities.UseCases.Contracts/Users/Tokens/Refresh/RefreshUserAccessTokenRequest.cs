namespace Freem.Entities.UseCases.Contracts.Users.Tokens.Refresh;

public sealed class RefreshUserAccessTokenRequest
{
    public string RefreshToken { get; }
    
    public RefreshUserAccessTokenRequest(string refreshToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(refreshToken);
        
        RefreshToken = refreshToken;
    }
}