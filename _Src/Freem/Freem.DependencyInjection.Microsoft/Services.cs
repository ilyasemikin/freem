using Microsoft.Extensions.DependencyInjection;

namespace Freem.DependencyInjection.Microsoft;

public static class Services
{
    public static TService Resolve<TService>(Action<IServiceCollection> initializer)
        where TService : notnull
    {
        var provider = BuildServiceProvider(initializer);
        return provider.GetRequiredService<TService>();
    }

    public static TService Resolve<TService>(Action<IServiceCollection> initializer, object? key)
        where TService : notnull
    {
        var provider = BuildServiceProvider(initializer);
        return provider.GetRequiredKeyedService<TService>(key);
    }

    private static ServiceProvider BuildServiceProvider(Action<IServiceCollection> initializer)
    {
        var services = new ServiceCollection();

        initializer(services);

        return services.BuildServiceProvider();
    }
}