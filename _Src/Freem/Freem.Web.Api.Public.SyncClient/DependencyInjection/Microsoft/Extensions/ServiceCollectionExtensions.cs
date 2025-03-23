using Freem.Entities.Serialization.Json;
using Freem.Http.Requests.DependencyInjection.Microsoft.Extensions;
using Freem.Web.Api.Public.Client.Configuration;
using Freem.Web.Api.Public.SyncClient.Implementations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NetHttpClient = System.Net.Http.HttpClient;

namespace Freem.Web.Api.Public.SyncClient.DependencyInjection.Microsoft.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSyncWebClient(this IServiceCollection services, ClientConfiguration configuration)
    {
        return services
            .AddNetHttpClient(configuration.BaseAddress)
            .AddWebClientSpecific();
    }

    internal static IServiceCollection AddSyncWebClient(this IServiceCollection services, NetHttpClient client)
    {
        return services
            .AddNetHttpClient(client)
            .AddWebClientSpecific();
    }

    private static IServiceCollection AddWebClientSpecific(this IServiceCollection services)
    {
        var jso = EntitiesJsonSerialization.CreateSerializerOptions();
        
        services.TryAddSingleton(provider => ActivatorUtilities.CreateInstance<ActivitiesSyncClient>(provider, jso));
        services.TryAddSingleton(provider => ActivatorUtilities.CreateInstance<EventsSyncClient>(provider, jso));
        services.TryAddSingleton(provider => ActivatorUtilities.CreateInstance<RecordsSyncClient>(provider, jso));
        services.TryAddSingleton(provider => ActivatorUtilities.CreateInstance<StatisticsSyncClient>(provider, jso));
        services.TryAddSingleton(provider => ActivatorUtilities.CreateInstance<TagsSyncClient>(provider, jso));
        services.TryAddSingleton(provider => ActivatorUtilities.CreateInstance<UsersSyncClient>(provider, jso));
        
        services.TryAddSingleton<CompositeSyncClient>();
        
        return services;
    }
}