using System.Diagnostics.CodeAnalysis;

namespace Freem.Web.Api.Public.Client;

public sealed class ClientSession
{
    public string? AccessToken { get; private set; }

    public bool TryGet([NotNullWhen(true)] out string? accessToken)
    {
        accessToken = AccessToken;
        return accessToken is not null;
    }

    public void Set(string accessToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(accessToken);
        
        AccessToken = accessToken;
    }

    public void Clean()
    {
        AccessToken = null;
    }
}
