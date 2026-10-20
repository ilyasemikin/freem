using Freem.Entities.UseCases.Context;
using Freem.Web.Api.Public.UseCases.Context;
using Utilities.UseCases.Abstractions;

namespace Freem.Web.Api.Public.UseCases.Implementations.Handlers;

internal abstract class BaseAuthorizedRequestHandler<TRequest> : BaseRequestHandler<ApiAuthorizedHandlerContext, TRequest>
    where TRequest : IRequest
{
    protected IRequestMediator<HandlerContext> Mediator { get; }

    protected BaseAuthorizedRequestHandler(IRequestMediator<HandlerContext> mediator)
    {
        ArgumentNullException.ThrowIfNull(mediator);

        Mediator = mediator;
    }
}
