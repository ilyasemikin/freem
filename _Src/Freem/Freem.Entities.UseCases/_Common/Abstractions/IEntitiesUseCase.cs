using Freem.UseCases.Abstractions;
using Freem.UseCases.Contracts.Abstractions;

namespace Freem.Entities.UseCases.Abstractions;

public interface IEntitiesUseCase<TRequest, TResponse, TResponseErrorCode> 
    : IUseCase<UseCaseExecutionContext, TRequest, TResponse, TResponseErrorCode>
    where TResponse : IResponse<TResponseErrorCode>
    where TResponseErrorCode : struct, Enum
{
    
}