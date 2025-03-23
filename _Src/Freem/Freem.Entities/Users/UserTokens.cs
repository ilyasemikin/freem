namespace Freem.Entities.Users;

public sealed class UserTokens
{
    public string AccessToken { get; }
    public string RefreshToken { get; }

    public UserTokens(string accessToken, string refreshToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(accessToken);
        ArgumentException.ThrowIfNullOrWhiteSpace(refreshToken);
        
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }
}