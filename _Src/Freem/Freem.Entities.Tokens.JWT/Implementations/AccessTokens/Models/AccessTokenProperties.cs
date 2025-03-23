using System.Diagnostics.CodeAnalysis;
using Freem.Entities.Users.Identifiers;

namespace Freem.Entities.Tokens.JWT.Implementations.AccessTokens.Models;

public sealed class AccessTokenProperties
{
    private const string EntropyClaim = "EntropyId";
    private const string UserIdClaim = "UserId";
    
    public UserIdentifier UserId { get; }

    public AccessTokenProperties(UserIdentifier userId)
    {
        ArgumentNullException.ThrowIfNull(userId);
        
        UserId = userId;
    }
    
    internal IDictionary<string, object> ToClaims()
    {
        return new Dictionary<string, object>
        {
            [EntropyClaim] = Guid.NewGuid().ToString("N"),
            [UserIdClaim] = UserId.ToString(),
        };
    }

    internal static bool TryCreate(
        IDictionary<string, object> claims, 
        [NotNullWhen(true)] out AccessTokenProperties? properties)
    {
        properties = null;
        
        if (!claims.TryGet<string>(UserIdClaim, out var userId))
            return false;

        properties = new AccessTokenProperties(userId);
        return true;
    }
}