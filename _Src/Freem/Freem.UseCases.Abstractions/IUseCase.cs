using Freem.UseCases.Contracts.Abstractions;

namespace Freem.UseCases.Abstractions;

public interface IUseCase<in TContext, in TRequest, TResponse, TResponseErrorCode>
    where TResponse : IResponse<TResponseErrorCode>
    where TResponseErrorCode : struct, Enum
{
    Task<TResponse> ExecuteAsync(
        TContext context, TRequest request, 
        CancellationToken cancellationToken = default);
}