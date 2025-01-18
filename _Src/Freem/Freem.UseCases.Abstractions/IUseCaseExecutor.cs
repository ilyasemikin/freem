namespace Freem.UseCases.Abstractions;

public interface IUseCaseExecutor<in TContext>
{
    Task<TResponse> ExecuteAsync<TRequest, TResponse>(
        TContext context, TRequest request,
        CancellationToken cancellationToken = default);
}