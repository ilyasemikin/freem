using Freem.Entities.Tokens.JWT.Implementations.AccessTokens.Models;
using Freem.Tokens.Abstractions;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Freem.Entities.Tokens.JWT.Implementations.AccessTokens;

public sealed class AccessTokenValidator
{
    private readonly JsonWebTokenHandler _handler;
    private readonly AccessTokenSettings _settings;
    private readonly ISecurityKeyGetter _keyGetter;
    
    public AccessTokenValidator(AccessTokenSettings settings, ISecurityKeyGetter keyGetter)
    {
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentNullException.ThrowIfNull(keyGetter);

        _handler = new JsonWebTokenHandler();
        _settings = settings;
        _keyGetter = keyGetter;
    }

    public async Task<AccessTokenValidationResult> ValidateAsync(
        string accessToken, 
        CancellationToken cancellationToken = default)
    {
        var parameters = new TokenValidationParameters
        {
            IssuerSigningKey = _keyGetter.Get(),
            ValidIssuer = _settings.Issuer,
            ValidAudience = _settings.Audience,
        };

        var result = await _handler.ValidateTokenAsync(accessToken, parameters);
        if (!result.IsValid || !AccessTokenProperties.TryCreate(result.Claims, out var properties))
            return AccessTokenValidationResult.Invalid(result.Exception);
        
        return AccessTokenValidationResult.Valid(properties);
    }
}