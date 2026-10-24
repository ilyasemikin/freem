using Freem.Api.Public.UseCases.Context;
using Utilities.UseCases.Abstractions;

namespace Freem.Api.Public.UseCases.Implementations.Handlers;

internal abstract class BaseAnonymousRequestHandler<TRequest> : BaseRequestHandler<ApiAnonymousHandlerContext, TRequest>
    where TRequest : IRequest
{
}
