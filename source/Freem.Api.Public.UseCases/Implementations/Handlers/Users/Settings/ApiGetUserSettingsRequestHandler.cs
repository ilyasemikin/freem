using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Users.Settings.Get;
using Freem.Api.Public.UseCases.Context;
using Freem.Api.Public.UseCases.Contracts.Users.Settings;
using Freem.Api.Public.UseCases.Implementations.Mappers;
using Freem.Api.Public.UseCases.Implementations.Mappers.Requests;
using Freem.Api.Public.UseCases.Implementations.Mappers.Responses;
using Microsoft.AspNetCore.Mvc;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Api.Public.UseCases.Implementations.Handlers.Users.Settings;

internal sealed class ApiGetUserSettingsRequestHandler : BaseAuthorizedRequestHandler<ApiGetUserSettingsRequest>
{
    public ApiGetUserSettingsRequestHandler(IRequestMediator<HandlerContext> mediator)
        : base(mediator)
    {
    }

    public override async Task<IActionResult> HandleAsync(
        ApiAuthorizedHandlerContext context, ApiGetUserSettingsRequest request,
        CancellationToken cancellationToken = default)
    {
        var domainContext = context.Map();
        var domainRequest = request.Map();
        var response = await Mediator.ExecuteAsync<GetUserSettingsRequest, Result<GetUserSettingsResponse, Error>>(domainContext, domainRequest, cancellationToken);
        return response.Map();
    }
}
