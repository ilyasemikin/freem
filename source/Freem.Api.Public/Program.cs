using System.Security.Claims;
using System.Text.Json;
using EgoHub.Api.Public.Client.Abstractions;
using EgoHub.Api.Public.Client.DependencyInjection.Microsoft.Extensions;
using EgoHub.Api.Public.Client.Implementations;
using EgoHub.Authentication.AspNetCore;
using EgoHub.Authentication.AspNetCore.DependencyInjection;
using EgoHub.Domain.Services;
using Freem.Domain.Serialization.Json;
using Freem.Domain.Storage.PostgreSQL.DependencyInjection.Microsoft.Extensions;
using Freem.Api.Public.Authentication.EgoHub;
using Freem.Api.Public.Configuration;
using Freem.Api.Public.Configuration.Subsystems;
using Freem.Api.Public.Contracts;
using Freem.Api.Public.Contracts.V1;
using Freem.Api.Public.ErrorHandling;
using Freem.Api.Public.Formatters;
using Freem.Api.Public.ModelBinders.Providers;
using Freem.Api.Public.OpenApi;
using Freem.Api.Public.OpenApi.Errors;
using Freem.Api.Public.UseCases.DependencyInjection.Microsoft.Extensions;
using Freem.Domain.UseCases.DependencyInjection;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Scalar.AspNetCore;
using Utilities.App.DependencyInjection;
using Utilities.Clock.DependencyInjection.Microsoft;
using Utilities.Errors.Serialization.Json.Extensions;
using Utilities.Locking.Redis.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseDefaultServiceProvider(options =>
{
    options.ValidateScopes = true;
    options.ValidateOnBuild = true;
});

var configuration = builder.Configuration.Get<ConfigurationProperties>();
if (configuration is null)
    throw new InvalidOperationException("configuration is null");

builder.Services.AddHttpLogging(o => {});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        var origins = configuration.Cors.Origins.ToArray();

        policy
            .WithOrigins(origins)
            .AllowAnyHeader()
            .AllowCredentials()
            .AllowAnyMethod()
            .WithExposedHeaders([HeaderNames.ContentDisposition]);
    });
});

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    // Only the prefix stripped by a reverse proxy is taken, so the proxy address is not checked: a forged header
    // changes PathBase (and the refresh cookie path) of the sender's own request only.
    options.ForwardedHeaders = ForwardedHeaders.XForwardedPrefix;
    options.KnownIPNetworks.Clear();
    options.KnownProxies.Clear();
});

builder.Services.AddBuildInfo();

builder.Services
    .AddUtcTruncatedToSecondsClock()
    .AddHttpContextAccessor()
    .AddLogging();

builder.Services
    .AddPostgreSqlStorage(configuration);

builder.Services.AddRedisLocking(configuration.Redis.ConnectionString);

builder.Services
    .AddEntitiesUseCases()
    .AddApiUseCases();

var egoHubApiKey = TokenCredential.Parse(configuration.Integrations.EgoHubApiKey, null);
builder.Services.AddTransient<ITokenCredentialLoader>(_ => new StaticTokenCredentialLoader(egoHubApiKey));
var egoHubAddress = configuration.Integrations.EgoHubClient.Address.ToString();
builder.Services.AddPublicClient(egoHubAddress);

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

builder.Services.AddApiErrorHandling();

builder.Services
    .AddControllers(options =>
    {
        options.ModelBinderProviders.Insert(0, new QueryListModelBinderProvider());
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PopulateErrorsConverters();
        EntitiesJsonSerialization.Populate(options.JsonSerializerOptions);
        
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    })
    .ConfigureApiBehaviorOptions(options =>
    {
        // Configured on the MVC builder: a plain Configure<ApiBehaviorOptions> registered before AddControllers
        // is overwritten by the default ProblemDetails factory.
        options.InvalidModelStateResponseFactory = InvalidModelStateResponseFactory.Create;
    });

builder.Services.AddTransient<IPostConfigureOptions<MvcOptions>, ArgumentValidationMvcOptionsSetup>();

builder.Services.AddSingleton(configuration.OpenApi ?? new OpenApiConfigurationProperties());
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<OpenApiDocumentTransformer>();
    options.AddSchemaTransformer<ErrorsSchemaTransformer>();
});

var application = builder.Build();

application.UseForwardedHeaders();

application.UseApiErrorHandling();

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