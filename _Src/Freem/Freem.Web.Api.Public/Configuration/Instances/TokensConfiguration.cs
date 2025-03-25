namespace Freem.Web.Api.Public.Configuration.Instances;

public sealed class CompositeTokensConfiguration
{
    public required string SecretKey { get; init; }
    
    public required TokensConfiguration Access { get; init; }
    public required TokensConfiguration Refresh { get; init; }
}

public sealed class TokensConfiguration
{
    public required string Issuer { get; init; }
    public required string Audience { get; init; }
    
    public required TimeSpan ExpirationPeriod { get; init; }
}