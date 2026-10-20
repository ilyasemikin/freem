using Freem.Entities.UseCases.Context;
using Freem.Entities.UseCases.Contracts.Users.Register;
using Freem.Web.Api.Public.UseCases.Context;
using Freem.Web.Api.Public.UseCases.Contracts.Users;
using Freem.Web.Api.Public.UseCases.Implementations.Mappers;
using Freem.Web.Api.Public.UseCases.Implementations.Mappers.Requests;
using Freem.Web.Api.Public.UseCases.Implementations.Mappers.Responses;
using Microsoft.AspNetCore.Mvc;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Web.Api.Public.UseCases.Implementations.Handlers.Users;

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
