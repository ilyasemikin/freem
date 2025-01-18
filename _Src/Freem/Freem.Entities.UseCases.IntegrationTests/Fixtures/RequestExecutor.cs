using Freem.UseCases.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Freem.Entities.UseCases.IntegrationTests.Fixtures;

public sealed class RequestExecutor
{
    private readonly IServiceProvider _services;

    internal RequestExecutor(IServiceProvider services)
    {
        ArgumentNullException.ThrowIfNull(services);
        
        _services = services;
    }

    public TResponse Execute<TRequest, TResponse>(UseCaseExecutionContext context, TRequest request)
        where TRequest : notnull
    {
        var task = ExecuteAsync<TRequest, TResponse>(context, request);
        var awaiter = task.GetAwaiter();
        return awaiter.GetResult();
    }
    
    public async Task<TResponse> ExecuteAsync<TRequest, TResponse>(UseCaseExecutionContext context, TRequest request)
        where TRequest : notnull
    {
        using var scope = _services.CreateScope();
        var provider = scope.ServiceProvider;
        
        var executor = provider.GetRequiredService<IUseCaseExecutor<UseCaseExecutionContext>>();
        
        return await executor.ExecuteAsync<TRequest, TResponse>(context, request);
    }
}