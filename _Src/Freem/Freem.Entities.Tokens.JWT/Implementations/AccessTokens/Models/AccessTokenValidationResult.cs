using System.Diagnostics.CodeAnalysis;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.Tokens.JWT.Implementations.AccessTokens.Models;

public sealed class AccessTokenValidationResult
{
    [MemberNotNullWhen(true, nameof(UserId))]
    public bool IsValid { get; }
    
    public UserIdentifier? UserId { get; }

    private AccessTokenValidationResult(bool isValid, UserIdentifier? userId = null)
    {
        IsValid = isValid;
        UserId = userId;
    }
    
    public static AccessTokenValidationResult Valid(UserIdentifier userId)
    {
        return new AccessTokenValidationResult(true, userId);
    }

    public static AccessTokenValidationResult Invalid()
    {
        return new AccessTokenValidationResult(false);
    }
}