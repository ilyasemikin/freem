namespace Freem.Entities.Tokens.JWT.Implementations.RefreshTokens.Models;

public sealed class RefreshTokenSettings
{
    public string Issuer { get; }
    public string Audience { get; }
    
    public TimeSpan ExpirationPeriod { get; }
    
    public RefreshTokenSettings(string issuer, string audience, TimeSpan expirationPeriod)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(issuer);
        ArgumentException.ThrowIfNullOrWhiteSpace(audience);
        
        Issuer = issuer;
        Audience = audience;
        ExpirationPeriod = expirationPeriod;
    }
}