using Freem.Entities.Storage.PostgreSQL.Database;
using Freem.Entities.Storage.PostgreSQL.Database.Extensions;
using Freem.Web.Api.Public.Client;
using Freem.Web.Api.Public.Client.DependencyInjection.Microsoft.Extensions;
using Freem.Web.Api.Public.Client.Implementations;
using Freem.Web.Api.Public.FunctionalTests.Context.Preparers;
using Freem.Web.Api.Public.FunctionalTests.Infrastructure;
using Freem.Web.Api.Public.SyncClient.DependencyInjection.Microsoft.Extensions;
using Freem.Web.Api.Public.SyncClient.Implementations;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit.Abstractions;

namespace Freem.Web.Api.Public.FunctionalTests.Context;

public sealed class TestContext
{
    private const string ConfigurationFileName = "configuration.json";

    private readonly WebApplicationFactory<Program> _application;
    
    public ITestOutputHelper? Output { get; set; }
    
    public TokenLoader TokenLoader { get; }
    
    public CompositeClient Client { get; }
    public CompositeSyncClient SyncClient { get; }
    
    public CombinedPreparer Preparer { get; }
    
    public TestContext()
    {
        var configuration = BuildConfiguration();
        
        _application = BuildApplication(configuration);
        var services = BuildServices(_application);
        
        TokenLoader = services.GetRequiredService<TokenLoader>();
        
        Client = services.GetRequiredService<CompositeClient>();
        SyncClient = services.GetRequiredService<CompositeSyncClient>();

        Preparer = new CombinedPreparer(this);
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
        configuration = configuration.GetSection(nameof(TestsConfiguration.WebApiApplication));
        return new WebApiApplication(configuration, () => Output);
    }
    
    private static IConfiguration BuildConfiguration()
    {
        return new ConfigurationBuilder()
            .AddJsonFile(ConfigurationFileName)
            .Build();
    }
    
    private static IServiceProvider BuildServices(WebApplicationFactory<Program> application)
    {
        var services = new ServiceCollection();

        var client = application.CreateClient();
        
        services
            .AddWebClient(client)
            .AddSyncWebClient(client);
        
        return services.BuildServiceProvider();
    }
}