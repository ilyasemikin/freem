namespace Freem.Entities.UseCases.Users.Tokens.Refresh.Models;

public sealed class RefreshUserAccessTokenRequest
{
    public required string RefreshToken { get; init; }
}