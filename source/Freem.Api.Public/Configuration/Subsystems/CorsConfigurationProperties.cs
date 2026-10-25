namespace Freem.Api.Public.Configuration.Subsystems;

internal sealed class CorsConfigurationProperties
{
    public IReadOnlyList<string> Origins { get; }

    public CorsConfigurationProperties(IReadOnlyList<string> origins)
    {
        ArgumentNullException.ThrowIfNull(origins);
        
        Origins = origins;
    }
}
