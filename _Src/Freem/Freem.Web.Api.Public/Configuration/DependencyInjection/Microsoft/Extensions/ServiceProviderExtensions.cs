using Microsoft.Extensions.Options;

namespace Freem.Web.Api.Public.Configuration.DependencyInjection.Microsoft.Extensions;

public static class ServiceProviderExtensions
{
    public static CompositeConfiguration GetConfiguration(this IServiceProvider provider)
    {
        var options = provider.GetRequiredService<IOptions<CompositeConfiguration>>();

        return options.Value;
    }
}