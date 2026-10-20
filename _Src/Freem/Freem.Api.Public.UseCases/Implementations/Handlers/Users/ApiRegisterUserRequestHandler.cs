using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Users.Register;
using Freem.Api.Public.UseCases.Context;
using Freem.Api.Public.UseCases.Contracts.Users;
using Freem.Api.Public.UseCases.Implementations.Mappers;
using Freem.Api.Public.UseCases.Implementations.Mappers.Requests;
using Freem.Api.Public.UseCases.Implementations.Mappers.Responses;
using Microsoft.AspNetCore.Mvc;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Api.Public.UseCases.Implementations.Handlers.Users;

internal sealed class ApiRegisterUserRequestHandler : BaseAuthorizedRequestHandler<ApiRegisterUserRequest>
{
    public ApiRegisterUserRequestHandler(IRequestMediator<HandlerContext> mediator)
        : base(mediator)
    {
    }

    public override async Task<IActionResult> HandleAsync(
        ApiAuthorizedHandlerContext context, ApiRegisterUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var domainContext = context.Map();
        var domainRequest = request.Map();
        var response = await Mediator.ExecuteAsync<RegisterUserRequest, Result<RegisterUserResponse, Error>>(domainContext, domainRequest, cancellationToken);
        return response.Map();
    }
}
