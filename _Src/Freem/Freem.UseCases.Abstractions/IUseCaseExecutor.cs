namespace Freem.UseCases.Abstractions;

public interface IUseCaseExecutor<in TContext>
    where TContext : notnull
{
    Task<TResponse> ExecuteAsync<TRequest, TResponse>(
        TContext context, TRequest request,
        CancellationToken cancellationToken = default)
        where TRequest : notnull;
}