using Freem.Entities.Tokens.JWT.Implementations.RefreshTokens.Models;
using Freem.Time.Abstractions;
using Freem.Tokens.Abstractions;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Freem.Entities.Tokens.JWT.Implementations.RefreshTokens;

public sealed class RefreshTokenGenerator
{
    private readonly JsonWebTokenHandler _handler;
    private readonly RefreshTokenSettings _settings;
    private readonly ISecurityKeyGetter _keyGetter;
    private readonly ICurrentTimeGetter _currentTimeGetter;
    
    public RefreshTokenGenerator(
        RefreshTokenSettings settings,
        ISecurityKeyGetter keyGetter, 
        ICurrentTimeGetter currentTimeGetter)
    {
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentNullException.ThrowIfNull(keyGetter);
        ArgumentNullException.ThrowIfNull(currentTimeGetter);

        _handler = new JsonWebTokenHandler();

        _settings = settings;
        _keyGetter = keyGetter;
        _currentTimeGetter = currentTimeGetter;
    }

    public string Generate(RefreshTokenProperties properties)
    {
        var now = _currentTimeGetter.Get();
        
        var securityKey = _keyGetter.Get();
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var expires = now.Add(_settings.ExpirationPeriod);

        var descriptor = new SecurityTokenDescriptor
        {
            SigningCredentials = credentials,
            Issuer = _settings.Issuer,
            Audience = _settings.Audience,
            NotBefore = now.UtcDateTime,
            Expires = expires.UtcDateTime,
            Claims = properties.ToClaims()
        };

        return _handler.CreateToken(descriptor);
    }
}