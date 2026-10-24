using Freem.Domain.UseCases.Context;
using Freem.Domain.UseCases.Contracts.Users.Settings.Update;
using Freem.Api.Public.UseCases.Context;
using Freem.Api.Public.UseCases.Contracts.Users.Settings;
using Freem.Api.Public.UseCases.Implementations.Mappers;
using Freem.Api.Public.UseCases.Implementations.Mappers.Requests;
using Freem.Api.Public.UseCases.Implementations.Mappers.Responses.Common;
using Microsoft.AspNetCore.Mvc;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Api.Public.UseCases.Implementations.Handlers.Users.Settings;

internal sealed class ApiUpdateUserSettingsRequestHandler : BaseAuthorizedRequestHandler<ApiUpdateUserSettingsRequest>
{
    public ApiUpdateUserSettingsRequestHandler(IRequestMediator<HandlerContext> mediator)
        : base(mediator)
    {
    }

    public override async Task<IActionResult> HandleAsync(
        ApiAuthorizedHandlerContext context, ApiUpdateUserSettingsRequest request,
        CancellationToken cancellationToken = default)
    {
        var domainContext = context.Map();
        var domainRequest = request.Map();
        var response = await Mediator.ExecuteAsync<UpdateUserSettingsRequest, Result<Error>>(domainContext, domainRequest, cancellationToken);
        return response.Map();
    }
}
