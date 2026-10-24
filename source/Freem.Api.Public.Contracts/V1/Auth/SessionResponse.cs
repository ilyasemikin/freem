namespace Freem.Api.Public.Contracts.V1.Auth;

/// <summary>A started or rotated session. The refresh token is delivered separately, as an HTTP-only cookie.</summary>
public sealed class SessionResponse
{
    /// <summary>Bearer access token.</summary>
    public string AccessToken { get; }
    /// <summary>When the access token expires.</summary>
    public DateTimeOffset AccessTokenExpiresAt { get; }

    public SessionResponse(string accessToken, DateTimeOffset accessTokenExpiresAt)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(accessToken);

        AccessToken = accessToken;
        AccessTokenExpiresAt = accessTokenExpiresAt;
    }
}
