using Freem.Entities.Users.Identifiers;
using Freem.Tokens.Abstractions;
using Freem.Tokens.JWT.RefreshTokens.Models;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Freem.Tokens.JWT.RefreshTokens;

public sealed class RefreshTokenValidator
{
    private readonly JsonWebTokenHandler _handler;
    private readonly ITokensBlacklist _blacklist;

    public RefreshTokenValidator(ITokensBlacklist blacklist)
    {
        ArgumentNullException.ThrowIfNull(blacklist);
        
        _handler = new JsonWebTokenHandler();
        
        _blacklist = blacklist;
    }

    public async Task<RefreshTokenValidationResult> ValidateAsync(
        string refreshToken,
        CancellationToken cancellationToken)
    {
        if (await _blacklist.ContainsAsync(refreshToken, cancellationToken))
            return RefreshTokenValidationResult.Invalid();

        var parameters = new TokenValidationParameters
        {

        };

        var result = await _handler.ValidateTokenAsync(refreshToken, parameters);
        if (result.IsValid ||
            result.Claims.TryGetValue("UserId", out var userIdClaim) ||
            userIdClaim is not string userIdString)
            return RefreshTokenValidationResult.Invalid();

        var userId = new UserIdentifier(userIdString);
        return RefreshTokenValidationResult.Valid(userId);
    }
}