using Freem.Http.Requests.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using HttpClient = Freem.Http.Requests.Implementations.HttpClient;
using NetHttpClient = System.Net.Http.HttpClient;

namespace Freem.Http.Requests.DependencyInjection.Microsoft.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddNetHttpClient(this IServiceCollection services, string? baseUri = null)
    {
        var handler = new SocketsHttpHandler();
        var client = new NetHttpClient(handler);
        
        services.TryAddSingleton<IHttpClient>(_ => new HttpClient(client, baseUri));
        return services;
    }

    public static IServiceCollection AddNetHttpClient(this IServiceCollection services, NetHttpClient client)
    {
        services.TryAddSingleton<IHttpClient>(_ => new HttpClient(client));
        return services;
    }
}