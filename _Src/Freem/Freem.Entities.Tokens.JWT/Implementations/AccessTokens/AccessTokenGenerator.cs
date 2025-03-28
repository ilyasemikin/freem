﻿using Freem.Entities.Tokens.JWT.Implementations.AccessTokens.Models;
using Freem.Time.Abstractions;
using Freem.Tokens.Abstractions;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Freem.Entities.Tokens.JWT.Implementations.AccessTokens;

public sealed class AccessTokenGenerator
{
    private readonly JsonWebTokenHandler _handler;
    private readonly AccessTokenSettings _settings;
    private readonly ISecurityKeyGetter _keyGetter;
    private readonly ICurrentTimeGetter _currentTimeGetter;

    public AccessTokenGenerator(
        AccessTokenSettings settings,
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

    public string Generate(AccessTokenProperties properties)
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
            Expires = expires.DateTime,
            Claims = properties.ToClaims()
        };

        return _handler.CreateToken(descriptor);
    }
}
