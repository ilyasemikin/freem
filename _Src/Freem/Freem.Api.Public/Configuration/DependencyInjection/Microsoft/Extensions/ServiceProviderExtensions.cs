using Microsoft.Extensions.Options;

namespace Freem.Api.Public.Configuration.DependencyInjection.Microsoft.Extensions;

public static class ServiceProviderExtensions
{
    public static CompositeConfiguration GetConfiguration(this IServiceProvider provider)
    {
        var options = provider.GetRequiredService<IOptions<CompositeConfiguration>>();

        return options.Value;
    }
}