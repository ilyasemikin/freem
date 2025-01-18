using System.Diagnostics.CodeAnalysis;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.Tokens.JWT.Implementations.RefreshTokens.Models;

public sealed class RefreshTokenValidationResult
{
    [MemberNotNullWhen(true, nameof(UserId))]
    public bool IsValid { get; }
    
    public UserIdentifier? UserId { get; }

    private RefreshTokenValidationResult(bool isValid, UserIdentifier? userId = null)
    {
        IsValid = isValid;
        UserId = userId;
    }

    public static RefreshTokenValidationResult Valid(UserIdentifier? userId)
    {
        return new RefreshTokenValidationResult(true, userId);
    }

    public static RefreshTokenValidationResult Invalid()
    {
        return new RefreshTokenValidationResult(false);
    }
}