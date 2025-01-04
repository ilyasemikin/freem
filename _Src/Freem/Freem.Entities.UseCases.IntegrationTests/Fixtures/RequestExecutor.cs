using Freem.Credentials.Password.Abstractions;
using Freem.Credentials.Password.Implementations;
using Freem.Crypto.Hashes.Abstractions.Models;
using Freem.Crypto.Hashes.DependencyInjection.Microsoft.Extensions;
using Freem.DependencyInjection.Microsoft.Extensions;
using Freem.Entities._Common.DependencyInjection;
using Freem.Entities.Storage.PostgreSQL.DependencyInjection;
using Freem.Entities.Storage.PostgreSQL.DependencyInjection.Microsoft.Extensions;
using Freem.Entities.UseCases.Abstractions;
using Freem.Entities.UseCases.Abstractions.Context;
using Freem.Entities.UseCases.DependencyInjection.Microsoft.Extensions;
using Freem.Entities.UseCases.Events.DependencyInjection.Microsoft;
using Freem.Entities.UseCases.Executors.DependencyInjection.Microsoft;
using Freem.Entities.UseCases.IntegrationTests.Infrastructure;
using Freem.Locking.Local.DependencyInjection;
using Freem.Time.DependencyInjection.Microsoft;
using Freem.Tokens.Blacklist.Redis.DependencyInjection;
using Freem.Tokens.Blacklist.Redis.DependencyInjection.Microsoft.Extensions;
using Freem.Tokens.JWT.DependencyInjection;
using Freem.Tokens.JWT.Implementations.AccessTokens.Models;
using Freem.Tokens.JWT.Implementations.RefreshTokens.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Freem.Entities.UseCases.IntegrationTests.Fixtures;

public sealed class RequestExecutor
{
    private readonly IServiceProvider _services;

    internal RequestExecutor(IServiceProvider services)
    {
        ArgumentNullException.ThrowIfNull(services);
        
        _services = services;
    }

    public void Execute<TRequest>(UseCaseExecutionContext context, TRequest request)
    {
        var task = ExecuteAsync(context, request);
        var awaiter = task.GetAwaiter();
        awaiter.GetResult();
    }

    public TResponse Execute<TRequest, TResponse>(UseCaseExecutionContext context, TRequest request)
    {
        var task = ExecuteAsync<TRequest, TResponse>(context, request);
        var awaiter = task.GetAwaiter();
        return awaiter.GetResult();
    }
    
    public async Task ExecuteAsync<TRequest>(UseCaseExecutionContext context, TRequest request)
    {
        using var scope = _services.CreateScope();
        var provider = scope.ServiceProvider;
        
        var executor = provider.GetRequiredService<IUseCaseExecutor>();

        await executor.ExecuteAsync(context, request);
    }
    
    public async Task<TResponse> ExecuteAsync<TRequest, TResponse>(UseCaseExecutionContext context, TRequest request)
    {
        using var scope = _services.CreateScope();
        var provider = scope.ServiceProvider;
        
        var executor = provider.GetRequiredService<IUseCaseExecutor>();
        
        return await executor.ExecuteAsync<TRequest, TResponse>(context, request);
    }
}