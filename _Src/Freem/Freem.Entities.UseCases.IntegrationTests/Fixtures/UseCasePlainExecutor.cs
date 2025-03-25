using Freem.Entities.UseCases.Plain.Implementations;
using Freem.UseCases.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Freem.Entities.UseCases.IntegrationTests.Fixtures;

public sealed class UseCasePlainExecutor : EntitiesPlainSyncExecutors, IDisposable
{
    private readonly IServiceScope _scope;
    
    private UseCasePlainExecutor(IServiceScope scope, IUseCaseExecutor<UseCaseExecutionContext> executor) 
        : base(executor)
    {
        _scope = scope;
    }

    public void Dispose()
    {
        _scope.Dispose();
    }
    
    public static UseCasePlainExecutor Create(IServiceProvider provider)
    {
        var scope = provider.CreateScope();

        return new UseCasePlainExecutor(scope, scope.ServiceProvider.GetRequiredService<IUseCaseExecutor<UseCaseExecutionContext>>());
    }
}