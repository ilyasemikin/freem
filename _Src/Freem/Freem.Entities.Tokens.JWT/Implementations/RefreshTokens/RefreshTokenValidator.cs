using Freem.Entities.Identifiers;
using Freem.Entities.Tokens.JWT.Implementations.RefreshTokens.Models;
using Freem.Tokens.Abstractions;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Freem.Entities.Tokens.JWT.Implementations.RefreshTokens;

public sealed class RefreshTokenValidator
{
    private readonly JsonWebTokenHandler _handler;
    private readonly RefreshTokenSettings _settings;
    private readonly ITokensBlacklist _blacklist;
    private readonly ISecurityKeyGetter _securityKeyGetter;

    public RefreshTokenValidator(
        RefreshTokenSettings settings,
        ITokensBlacklist blacklist, 
        ISecurityKeyGetter securityKeyGetter)
    {
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentNullException.ThrowIfNull(blacklist);
        ArgumentNullException.ThrowIfNull(securityKeyGetter);
        
        _handler = new JsonWebTokenHandler();
        
        _settings = settings;
        _blacklist = blacklist;
        _securityKeyGetter = securityKeyGetter;
    }

    public async Task<RefreshTokenValidationResult> ValidateAsync(
        string refreshToken,
        CancellationToken cancellationToken)
    {
        if (await _blacklist.ContainsAsync(refreshToken, cancellationToken))
            return RefreshTokenValidationResult.Invalid();

        var securityKey = _securityKeyGetter.Get();
        var parameters = new TokenValidationParameters
        {
            ValidIssuer = _settings.Issuer,
            ValidAudience = _settings.Audience,
            IssuerSigningKey = securityKey
        };

        var result = await _handler.ValidateTokenAsync(refreshToken, parameters);
        if (!result.IsValid ||
            !result.Claims.TryGetValue("UserId", out var userIdClaim) ||
            userIdClaim is not string userIdString)
            return RefreshTokenValidationResult.Invalid();

        var userId = new UserIdentifier(userIdString);
        return RefreshTokenValidationResult.Valid(userId);
    }
}