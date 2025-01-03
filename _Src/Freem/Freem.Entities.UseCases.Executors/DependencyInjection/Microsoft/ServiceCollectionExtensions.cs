using Freem.Entities.UseCases.Abstractions;
using Freem.Entities.UseCases.Executors.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace Freem.Entities.UseCases.Executors.DependencyInjection.Microsoft;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUseCaseExecutor(this IServiceCollection services)
    {
        services.AddTransient<IUseCaseExecutor>(provider => new UseCaseExecutor(provider));
        
        return services;
    }
}