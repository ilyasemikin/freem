namespace Freem.Web.Api.Public.Contracts.DTO.Auth;

public sealed class SessionResponse
{
    public string AccessToken { get; }
    public DateTimeOffset AccessTokenExpiresAt { get; }

    public SessionResponse(string accessToken, DateTimeOffset accessTokenExpiresAt)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(accessToken);

        AccessToken = accessToken;
        AccessTokenExpiresAt = accessTokenExpiresAt;
    }
}
