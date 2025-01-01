using System.Diagnostics.CodeAnalysis;

namespace Freem.Entities.UseCases.Users.Password.Login.Models;

public sealed class LoginUserPasswordResponse
{
    [MemberNotNullWhen(true, nameof(AccessToken))]
    [MemberNotNullWhen(true, nameof(RefreshToken))]
    public required bool Success { get; init; }
    
    public string? AccessToken { get; init; }
    public string? RefreshToken { get; init; }

    public static LoginUserPasswordResponse Failure()
    {
        return new LoginUserPasswordResponse
        {
            Success = false
        };
    }

    public static LoginUserPasswordResponse Authorize(string accessToken, string refreshToken)
    {
        return new LoginUserPasswordResponse
        {
            Success = true,
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }
}