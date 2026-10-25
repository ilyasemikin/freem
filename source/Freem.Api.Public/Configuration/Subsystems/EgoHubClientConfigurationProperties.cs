namespace Freem.Api.Public.Configuration.Subsystems;

internal sealed class EgoHubClientConfigurationProperties
{
    public Uri Address { get; }

    public EgoHubClientConfigurationProperties(Uri address)
    {
        ArgumentNullException.ThrowIfNull(address);
        
        Address = address;
    }
}
