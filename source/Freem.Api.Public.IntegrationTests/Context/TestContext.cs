using System.Net.Http.Headers;
using Freem.Domain.Storage.PostgreSQL.Database;
using Freem.Domain.Storage.PostgreSQL.Database.Extensions;
using Freem.Api.Public.Client;
using Freem.Api.Public.Client.DependencyInjection;
using Freem.Api.Public.Client.Implementations;
using Freem.Api.Public.IntegrationTests.Context.Preparers;
using Freem.Api.Public.IntegrationTests.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit.Abstractions;

namespace Freem.Api.Public.IntegrationTests.Context;

public sealed class TestContext
{
    private const string ConfigurationFileName = "configuration.json";

    private readonly WebApplicationFactory<Program> _application;
    
    public ITestOutputHelper? Output { get; set; }
    
    public ClientSession ClientSession { get; }
    
    public CompositeClient Client { get; }
    
    public CombinedPreparer Preparer { get; }
    
    public TestContext()
    {
        var configuration = BuildConfiguration();
        
        _application = BuildApplication(configuration);
        var services = BuildServices(_application);
        
        ClientSession = services.GetRequiredService<ClientSession>();
        
        Client = services.GetRequiredService<CompositeClient>();

        Preparer = new CombinedPreparer(this);
    }

    // A raw client for requests the typed client cannot express (e.g. malformed route values).
    public HttpClient CreateHttpClient()
    {
        var client = _application.CreateClient();
        if (ClientSession.TryGet(out var accessToken))
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        return client;
    }

    public void CleanDatabase()
    {
        using var scope = _application.Services.CreateScope();
        var services = scope.ServiceProvider;

        var context = services.GetRequiredService<DatabaseContext>();
        
        context.TruncateTables();
    }

    private WebApplicationFactory<Program> BuildApplication(IConfiguration configuration)
    {
        return new WebApiApplication(configuration, () => Output);
    }
    
    private static IConfiguration BuildConfiguration()
    {
        return new ConfigurationBuilder()
            .AddJsonFile(ConfigurationFileName)
            .AddEnvironmentVariables()
            .Build();
    }
    
    private static IServiceProvider BuildServices(WebApplicationFactory<Program> application)
    {
        var services = new ServiceCollection();

        var client = application.CreateClient();
        
        services.AddWebClient(client);
        
        return services.BuildServiceProvider();
    }
}