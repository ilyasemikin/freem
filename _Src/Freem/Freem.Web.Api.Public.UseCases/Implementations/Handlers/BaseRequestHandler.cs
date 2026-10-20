using Microsoft.AspNetCore.Mvc;
using Utilities.UseCases.Abstractions;

namespace Freem.Web.Api.Public.UseCases.Implementations.Handlers;

internal abstract class BaseRequestHandler<TContext, TRequest> : IRequestHandler<TContext, TRequest, IActionResult>
    where TContext : IHandlerContext
    where TRequest : IRequest
{
    public abstract Task<IActionResult> HandleAsync(
        TContext context, TRequest request,
        CancellationToken cancellationToken = default);
}
