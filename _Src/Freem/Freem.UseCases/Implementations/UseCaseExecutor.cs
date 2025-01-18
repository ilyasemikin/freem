using Freem.UseCases.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Freem.UseCases.Implementations;

public sealed class UseCaseExecutor<TContext> : IUseCaseExecutor<TContext>
{
    private readonly IServiceProvider _services;

    public UseCaseExecutor(IServiceProvider services)
    {
        ArgumentNullException.ThrowIfNull(services);
        
        _services = services;
    }

    public async Task<TResponse> ExecuteAsync<TRequest, TResponse>(TContext context, TRequest request,
        CancellationToken cancellationToken = default)
    {
        var interfaces = typeof(TResponse).GetInterfaces();
        var responseType = interfaces.FirstOrDefault(e => e.FullName?.Contains("Freem.Entities.UseCases.DTO.Abstractions.IResponse") == true);
        var errorType = responseType.GetGenericArguments()[0];
        var caseType = typeof(IUseCase<,,,>).MakeGenericType(typeof(TContext), typeof(TRequest), typeof(TResponse), errorType);
        var method = caseType.GetMethod("ExecuteAsync");

        var @case = _services.GetRequiredService(caseType);
        
        return await (Task<TResponse>)method.Invoke(@case, [context, request, cancellationToken]);
    }
}
