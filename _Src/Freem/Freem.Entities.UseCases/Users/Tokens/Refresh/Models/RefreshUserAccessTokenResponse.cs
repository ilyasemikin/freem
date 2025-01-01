namespace Freem.Entities.UseCases.Users.Tokens.Refresh.Models;

public class RefreshUserAccessTokenResponse
{
    public required bool Success { get; init; }
    
    public string? AccessToken { get; init; }
    public string? RefreshToken { get; init; }

    public static RefreshUserAccessTokenResponse Failure()
    {
        return new RefreshUserAccessTokenResponse
        {
            Success = false
        };
    }

    public static RefreshUserAccessTokenResponse Updated(string accessToken, string refreshToken)
    {
        return new RefreshUserAccessTokenResponse
        {
            Success = true,
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }
}