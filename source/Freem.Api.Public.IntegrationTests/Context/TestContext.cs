using System.Net.Http.Headers;
using Freem.Api.Public.Client;
using Freem.Api.Public.Client.DependencyInjection;
using Freem.Api.Public.Client.Implementations;
using Freem.Api.Public.IntegrationTests.Configuration;
using Freem.Api.Public.IntegrationTests.Context.Preparers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Freem.Api.Public.IntegrationTests.Context;

// One context per test: its own client and session against a running API, so tests run in parallel.
public sealed class TestContext : IDisposable
{
    private static readonly Lazy<ConfigurationProperties> Configuration = new(BuildConfiguration);

    private readonly ServiceProvider _services;

    public ClientSession ClientSession { get; }
    
    public CompositeClient Client { get; }
    
    public CombinedPreparer Preparer { get; }
    
    public TestContext()
    {
        _services = new ServiceCollection()
            .AddWebClient(Configuration.Value)
            .BuildServiceProvider();
        
        ClientSession = _services.GetRequiredService<ClientSession>();
        
        Client = _services.GetRequiredService<CompositeClient>();

        Preparer = new CombinedPreparer(this);
    }

    // A raw client for requests the typed client cannot express (e.g. malformed route values).
    public HttpClient CreateHttpClient()
    {
        var client = new HttpClient
        {
            BaseAddress = Configuration.Value.FreemClient.Address
        };
        if (ClientSession.TryGet(out var accessToken))
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        return client;
    }

    public void Dispose()
    {
        _services.Dispose();
    }

    private static ConfigurationProperties BuildConfiguration()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile(ConfigurationProperties.Filename)
            .AddEnvironmentVariables()
            .Build();

        return configuration.Get<ConfigurationProperties>()
            ?? throw new InvalidOperationException("configuration is null");
    }
}
