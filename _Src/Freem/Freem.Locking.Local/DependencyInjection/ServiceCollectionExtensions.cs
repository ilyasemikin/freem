using Freem.Locking.Abstractions;
using Freem.Locking.Local.Implementations.Empty;
using Microsoft.Extensions.DependencyInjection;

namespace Freem.Locking.Local.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEmptyLocking(this IServiceCollection services)
    {
        services.AddSingleton<IDistributedLocker, EmptyDistributedLocker>();
        
        return services;
    }
}