using System.Net;
using System.Security.Claims;
using System.Text.Json;
using EgoHub.Api.Public.Client.Abstractions;
using EgoHub.Api.Public.Client.DependencyInjection.Microsoft.Extensions;
using EgoHub.Api.Public.Client.Implementations;
using EgoHub.Authentication.AspNetCore;
using EgoHub.Authentication.AspNetCore.DependencyInjection;
using EgoHub.Domain.Services;
using Freem.Entities.DependencyInjection;
using Freem.Entities.Events.Producer.DependencyInjection.Microsoft;
using Freem.Entities.Serialization.Json;
using Freem.Entities.Storage.PostgreSQL.DependencyInjection.Microsoft.Extensions;
using Freem.Entities.UseCases.DependencyInjection.Microsoft.Extensions;
using Freem.Locking.Redis.DependencyInjection.Microsoft.Extensions;
using Freem.Web.Api.Public.Authentication.EgoHub;
using Freem.Web.Api.Public.Configuration.DependencyInjection.Microsoft.Extensions;
using Freem.Web.Api.Public.Configuration.Extensions;
using Freem.Web.Api.Public.Contracts;
using Freem.Web.Api.Public.ModelBinders.Providers;
using Freem.Web.Api.Public.OpenApi;
using Freem.Web.Api.Public.OpenApi.Headers;
using Freem.Web.Api.Public.Services.DependencyInjection.Microsoft.Extensions;
using Scalar.AspNetCore;
using Utilities.Clock.DependencyInjection.Microsoft;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseDefaultServiceProvider(options =>
{
    options.ValidateScopes = true;
    options.ValidateOnBuild = true;
});

builder.WebHost.ConfigureKestrel(options =>
{
    var configuration = builder.Configuration.GetApiConfiguration();

    if (configuration.Ports is null || configuration.Ports.Count == 0)
        return;

    foreach (var port in configuration.Ports)
        options.Listen(IPAddress.Any, port);
});

builder.Services.AddConfiguration(builder.Configuration);

builder.Services.AddHttpLogging(o => {});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        var configuration = builder.Configuration.GetApiConfiguration();

        if (configuration.CorsOrigins is not null)
        {
            policy
                .WithOrigins(configuration.CorsOrigins.ToArray())
                .AllowAnyHeader()
                .AllowCredentials()
                .AllowAnyMethod()
                .WithExposedHeaders([HeaderNames.ItemsCount, HeaderNames.TotalItemsCount]);
        }
    });
});

builder.Services
    .AddUtcTruncatedToSecondsClock()
    .AddHttpContextAccessor()
    .AddLogging();

builder.Services
    .AddPostgreSqlStorage(builder.Configuration.GetStorageConfiguration())
    .AddSimpleRedisDistributedLocks(builder.Configuration.GetRedisLockingConfiguration())
    .AddNullEventPublishing();

builder.Services
    .AddUseCaseContextProvider()
    .AddIdentifiersGenerators()
    .AddEntitiesIdentifiersNameConverters()
    .AddEntitiesUseCases();

var egoHubConfiguration = builder.Configuration.GetEgoHubIntegrationConfiguration();
var egoHubToken = TokenCredential.Parse(egoHubConfiguration.Token, null);
builder.Services.AddTransient<ITokenCredentialLoader>(_ => new StaticTokenCredentialLoader(egoHubToken));
builder.Services.AddPublicClient(egoHubConfiguration.Url);

builder.Services
    .AddAuthentication(EgoHubBearerAuthenticationDefaults.SchemeName)
    .AddEgoHubBearerAuthentication(options =>
    {
        options.OnTokenValidated = context =>
        {
            if (!EgoHubClaimsMapper.TryMapUserIdClaim(context.Principal!, out var claim))
            {
                context.Fail("EgoHub access token is missing a valid subject claim.");
                return Task.CompletedTask;
            }

            var identity = (ClaimsIdentity)context.Principal!.Identity!;
            identity.AddClaim(claim);
            return Task.CompletedTask;
        };
    });
builder.Services.AddAuthorization();

builder.Services
    .AddControllers(options =>
    {
        options.ModelBinderProviders.Insert(0, new QueryListModelBinderProvider());
    })
    .AddJsonOptions(options =>
    {
        EntitiesJsonSerialization.Populate(options.JsonSerializerOptions);
        
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<OpenApiDocumentTransformer>();
    options.AddOperationTransformer<HeadersOpenApiOperationTransformer>();
});

var application = builder.Build();

application
    .MapOpenApi()
    .CacheOutput();
application.MapScalarApiReference();

application.UseCors();

application.UseAuthentication();
application.UseAuthorization();

application.MapControllers();

application.Run();

public partial class Program {}