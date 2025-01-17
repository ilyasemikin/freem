using Freem.Entities.UseCases.Abstractions.Context;
using Freem.Entities.UseCases.Abstractions.Types;
using Freem.Entities.UseCases.DTO.Abstractions;

namespace Freem.Entities.UseCases.Abstractions;

public interface IUseCase<in TRequest, TResponse, TResponseErrorCode>
    where TResponse : IResponse<TResponseErrorCode>
    where TResponseErrorCode : struct, Enum
{
    Task<TResponse> ExecuteAsync(
        UseCaseExecutionContext context, TRequest request, 
        CancellationToken cancellationToken = default);
}