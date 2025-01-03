using Freem.Entities.UseCases.Abstractions;
using Freem.Entities.UseCases.Abstractions.Context;
using Microsoft.Extensions.DependencyInjection;

namespace Freem.Entities.UseCases.Executors.Implementations;

internal sealed class UseCaseExecutor : IUseCaseExecutor
{
    private readonly IServiceProvider _services;

    public UseCaseExecutor(IServiceProvider services)
    {
        ArgumentNullException.ThrowIfNull(services);
        
        _services = services;
    }

    public async Task ExecuteAsync<TRequest>(UseCaseExecutionContext context, TRequest request,
        CancellationToken cancellationToken = default)
    {
        var @case = _services.GetRequiredService<IUseCase<TRequest>>();
        
        await @case.ExecuteAsync(context, request, cancellationToken);
    }

    public async Task<TResponse> ExecuteAsync<TRequest, TResponse>(UseCaseExecutionContext context, TRequest request,
        CancellationToken cancellationToken = default)
    {
        var @case = _services.GetRequiredService<IUseCase<TRequest, TResponse>>();
        
        return await @case.ExecuteAsync(context, request, cancellationToken);
    }
}
