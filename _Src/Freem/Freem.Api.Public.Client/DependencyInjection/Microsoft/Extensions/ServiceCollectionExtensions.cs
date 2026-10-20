using System.Text.Json;
using Freem.Domain.Serialization.Json;
using Freem.Api.Public.Client.Configuration;
using Freem.Api.Public.Client.Implementations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Utilities.Errors.Serialization.Json.Extensions;
using Utilities.Http.SystemNet.DependencyInjection.Microsoft.Extensions;
using NetHttpClient = System.Net.Http.HttpClient;

namespace Freem.Api.Public.Client.DependencyInjection.Microsoft.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWebClient(this IServiceCollection services, ClientConfiguration configuration)
    {
        return services
            .AddSystemNetHttpClient(configuration.BaseAddress)
            .AddWebClientSpecific();
    }

    internal static IServiceCollection AddWebClient(this IServiceCollection services, NetHttpClient client)
    {
        return services
            .AddSystemNetHttpClient(client)
            .AddWebClientSpecific();
    }

    private static IServiceCollection AddWebClientSpecific(this IServiceCollection services)
    {
        var jso = EntitiesJsonSerialization.CreateSerializerOptions();
        jso.PopulateErrorsConverters();
        
        // The API host overrides the naming policy with camelCase (Program.cs), the client must match it.
        jso.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;

        services.TryAddSingleton<ClientSession>();
        
        services.TryAddSingleton(provider => ActivatorUtilities.CreateInstance<ActivitiesClient>(provider, jso));
        services.TryAddSingleton(provider => ActivatorUtilities.CreateInstance<EventsClient>(provider, jso));
        services.TryAddSingleton(provider => ActivatorUtilities.CreateInstance<RecordsClient>(provider, jso));
        services.TryAddSingleton(provider => ActivatorUtilities.CreateInstance<StatisticsClient>(provider, jso));
        services.TryAddSingleton(provider => ActivatorUtilities.CreateInstance<TagsClient>(provider, jso));
        services.TryAddSingleton(provider => ActivatorUtilities.CreateInstance<UsersClient>(provider, jso));
        services.TryAddSingleton(provider => ActivatorUtilities.CreateInstance<AuthClient>(provider, jso));
        
        services.TryAddSingleton<CompositeClient>();

        return services;
    }
}