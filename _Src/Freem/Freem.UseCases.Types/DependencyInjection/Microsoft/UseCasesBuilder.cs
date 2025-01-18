using Freem.UseCases.Types.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace Freem.UseCases.Types.DependencyInjection.Microsoft;

public sealed class UseCasesBuilder<TContext>
    where TContext : notnull
{
    private readonly IServiceCollection _services;
    private readonly UseCasesTypesCollectionMonoContextBuilder<TContext> _builder;
    
    public UseCasesBuilder(IServiceCollection services, UseCasesTypesCollectionMonoContextBuilder<TContext> builder)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(builder);
        
        _services = services;
        _builder = builder;
    }

    public UseCasesBuilder<TContext> Add<TUseCase>(ServiceLifetime lifetime = ServiceLifetime.Transient)
        where TUseCase : notnull
    {
        var useCaseType = typeof(TUseCase);
        
        _builder.Add<TUseCase>();
        if (!_builder.TryGetDescriptor(useCaseType, out var descriptor))
            throw new InvalidOperationException();
        
        var service = new ServiceDescriptor(descriptor.AbstractionType, useCaseType, lifetime);
        _services.Add(service);

        return this;
    }

    public UseCasesBuilder<TContext> Add<TUseCase>(
        Func<IServiceProvider, TUseCase> factory, 
        ServiceLifetime lifetime = ServiceLifetime.Transient)
        where TUseCase : notnull
    {
        var useCaseType = typeof(TUseCase);
        
        _builder.Add<TUseCase>();
        if (!_builder.TryGetDescriptor(useCaseType, out var descriptor))
            throw new InvalidOperationException();

        var service = new ServiceDescriptor(descriptor.AbstractionType, null, (provider, _) => factory(provider), lifetime);
        _services.Add(service);

        return this;
    }
}