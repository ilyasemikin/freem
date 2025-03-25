using Freem.Entities.Serialization.Json;
using Freem.Http.Requests.DependencyInjection.Microsoft.Extensions;
using Freem.Web.Api.Public.Client.Configuration;
using Freem.Web.Api.Public.Client.Implementations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NetHttpClient = System.Net.Http.HttpClient;

namespace Freem.Web.Api.Public.Client.DependencyInjection.Microsoft.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWebClient(this IServiceCollection services, ClientConfiguration configuration)
    {
        return services
            .AddNetHttpClient(configuration.BaseAddress)
            .AddWebClientSpecific();
    }

    internal static IServiceCollection AddWebClient(this IServiceCollection services, NetHttpClient client)
    {
        return services
            .AddNetHttpClient(client)
            .AddWebClientSpecific();
    }

    private static IServiceCollection AddWebClientSpecific(this IServiceCollection services)
    {
        var jso = EntitiesJsonSerialization.CreateSerializerOptions();

        services.TryAddSingleton(provider => ActivatorUtilities.CreateInstance<TokenLoader>(provider, jso));
        
        services.TryAddSingleton(provider => ActivatorUtilities.CreateInstance<ActivitiesClient>(provider, jso));
        services.TryAddSingleton(provider => ActivatorUtilities.CreateInstance<EventsClient>(provider, jso));
        services.TryAddSingleton(provider => ActivatorUtilities.CreateInstance<RecordsClient>(provider, jso));
        services.TryAddSingleton(provider => ActivatorUtilities.CreateInstance<StatisticsClient>(provider, jso));
        services.TryAddSingleton(provider => ActivatorUtilities.CreateInstance<TagsClient>(provider, jso));
        services.TryAddSingleton(provider => ActivatorUtilities.CreateInstance<UsersClient>(provider, jso));
        
        services.TryAddSingleton<CompositeClient>();

        return services;
    }
}