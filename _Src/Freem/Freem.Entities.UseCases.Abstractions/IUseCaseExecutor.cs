using Freem.Entities.UseCases.Abstractions.Context;

namespace Freem.Entities.UseCases.Abstractions;

public interface IUseCaseExecutor
{
    Task ExecuteAsync<TRequest>(
        UseCaseExecutionContext context, TRequest request, 
        CancellationToken cancellationToken = default);

    Task<TResponse> ExecuteAsync<TRequest, TResponse>(
        UseCaseExecutionContext context, TRequest request,
        CancellationToken cancellationToken = default);
}