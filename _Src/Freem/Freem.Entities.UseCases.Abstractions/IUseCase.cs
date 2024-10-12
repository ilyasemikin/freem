using Freem.Entities.UseCases.Abstractions.Types;
using Freem.Entities.UseCases.Context;

namespace Freem.Entities.UseCases.Abstractions;

public interface IUseCase<in TRequest, TResponse>
    
{
    Task<TResponse> ExecuteAsync(
        UseCaseExecutionContext context, TRequest request, 
        CancellationToken cancellationToken = default);
}

public interface IUseCase<in TRequest> : IUseCase<TRequest, Unit>
{
    new Task ExecuteAsync(
        UseCaseExecutionContext context, TRequest request, 
        CancellationToken cancellationToken = default);

    async Task<Unit> IUseCase<TRequest, Unit>.ExecuteAsync(
        UseCaseExecutionContext context, TRequest request, 
        CancellationToken cancellationToken)
    {
        return await Task.FromResult(Unit.Instance);
    }
}