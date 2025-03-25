using Freem.UseCases.Types.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace Freem.UseCases.Types.DependencyInjection.Microsoft.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUseCases<TContext>(
        this IServiceCollection services, Action<UseCasesBuilder<TContext>> builderAction)
        where TContext : notnull
    {
        var typesCollectionBuilder = new UseCasesTypesCollectionMonoContextBuilder<TContext>();
        var builder = new UseCasesBuilder<TContext>(services, typesCollectionBuilder);

        builderAction(builder);
        
        var typesCollection = typesCollectionBuilder.Build();
        services.AddSingleton(typesCollection);
        
        return services;
    }
}