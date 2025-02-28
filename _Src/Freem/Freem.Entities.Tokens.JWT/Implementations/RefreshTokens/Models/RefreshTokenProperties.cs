using System.Diagnostics.CodeAnalysis;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.Tokens.JWT.Implementations.RefreshTokens.Models;

public sealed class RefreshTokenProperties
{
    private const string UserIdClaim = "UserId";
    
    public UserIdentifier UserId { get; }

    public RefreshTokenProperties(UserIdentifier userId)
    {
        ArgumentNullException.ThrowIfNull(userId);
        
        UserId = userId;
    }

    internal IDictionary<string, object> ToClaims()
    {
        return new Dictionary<string, object>
        {
            [UserIdClaim] = UserId
        };
    }

    internal static bool TryCreate(
        IDictionary<string, object> claims,
        [NotNullWhen(true)] out RefreshTokenProperties? properties)
    {
        properties = null;

        if (!claims.TryGet<string>(UserIdClaim, out var userId))
            return false;
        
        properties = new RefreshTokenProperties(userId);
        return true;
    }
}