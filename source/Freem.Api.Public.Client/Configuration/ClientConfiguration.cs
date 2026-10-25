namespace Freem.Api.Public.Client.Configuration;

public sealed class ClientConfiguration
{
    public Uri Address { get; }

    public ClientConfiguration(Uri address)
    {
        ArgumentNullException.ThrowIfNull(address);
        
        Address = address;
    }
}
