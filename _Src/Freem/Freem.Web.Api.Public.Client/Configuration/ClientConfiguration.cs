namespace Freem.Web.Api.Public.Client.Configuration;

public sealed class ClientConfiguration
{
    public string BaseAddress { get; }

    public ClientConfiguration(string baseAddress)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(baseAddress);
        
        BaseAddress = baseAddress;
    }
}