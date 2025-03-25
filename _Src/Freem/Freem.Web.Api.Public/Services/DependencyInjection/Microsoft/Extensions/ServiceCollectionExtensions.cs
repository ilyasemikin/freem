using Freem.Web.Api.Public.Services.Implementations;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Freem.Web.Api.Public.Services.DependencyInjection.Microsoft.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUseCaseContextProvider(this IServiceCollection services)
    {
        services.TryAddScoped<UseCaseContextProvider>();
        return services;
    }
}