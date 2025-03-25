using Microsoft.Extensions.DependencyInjection;

namespace Freem.DependencyInjection.Microsoft.Extensions;

public static class ServiceCollectionBuildExtensions
{
    public static IServiceProvider BuildAndValidateServiceProvider(this IServiceCollection services)
    {
        var options = new ServiceProviderOptions { ValidateOnBuild = true };
        return services.BuildServiceProvider(options);
    }
}