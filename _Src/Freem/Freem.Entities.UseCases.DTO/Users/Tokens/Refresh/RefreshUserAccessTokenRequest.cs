namespace Freem.Entities.UseCases.DTO.Users.Tokens.Refresh;

public sealed class RefreshUserAccessTokenRequest
{
    public string RefreshToken { get; }
    
    public RefreshUserAccessTokenRequest(string refreshToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(refreshToken);
        
        RefreshToken = refreshToken;
    }
}