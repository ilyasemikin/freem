namespace Freem.Web.Api.Public.Configuration.DependencyInjection.Microsoft.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        return services.Configure<CompositeConfiguration>(configuration);
    }
}