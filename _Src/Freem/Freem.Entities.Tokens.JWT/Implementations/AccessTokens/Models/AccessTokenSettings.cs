namespace Freem.Entities.Tokens.JWT.Implementations.AccessTokens.Models;

public sealed class AccessTokenSettings
{
    public string Issuer { get; }
    public string Audience { get; }
    
    public TimeSpan ExpirationPeriod { get; }

    public AccessTokenSettings(string issuer, string audience, TimeSpan expirationPeriod)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(issuer);
        ArgumentException.ThrowIfNullOrWhiteSpace(audience);
        
        Issuer = issuer;
        Audience = audience;
        ExpirationPeriod = expirationPeriod;
    }
}