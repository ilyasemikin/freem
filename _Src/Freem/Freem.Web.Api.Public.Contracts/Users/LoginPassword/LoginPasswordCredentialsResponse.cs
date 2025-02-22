namespace Freem.Web.Api.Public.Contracts.Users.LoginPassword;

public sealed class LoginPasswordCredentialsResponse
{
    public string AccessToken { get; }
    public string RefreshToken { get; }
    
    public LoginPasswordCredentialsResponse(string accessToken, string refreshToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(accessToken);
        ArgumentException.ThrowIfNullOrWhiteSpace(refreshToken);
        
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }
}