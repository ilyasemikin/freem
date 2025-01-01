using Freem.Entities.Users.Identifiers;
using Freem.Tokens.Abstractions;
using Freem.Tokens.JWT.AccessTokens.Models;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Freem.Tokens.JWT.AccessTokens;

public sealed class AccessTokenValidator
{
    private readonly JsonWebTokenHandler _handler;
    
    public AccessTokenValidator(ITokensBlacklist blacklist)
    {
        ArgumentNullException.ThrowIfNull(blacklist);
        
        _handler = new JsonWebTokenHandler();
    }

    public async Task<AccessTokenValidationResult> ValidateAsync(
        string accessToken, 
        CancellationToken cancellationToken = default)
    {
        var parameters = new TokenValidationParameters
        {
            
        };
        
        var result = await _handler.ValidateTokenAsync(accessToken, parameters);
        if (result.IsValid || 
            result.Claims.TryGetValue("UserId", out var userIdClaim) || 
            userIdClaim is not string userIdString)
            return AccessTokenValidationResult.Invalid();

        var userId = new UserIdentifier(userIdString);
        return AccessTokenValidationResult.Valid(userId);
    }
}