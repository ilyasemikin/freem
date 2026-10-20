using Freem.Domain;
using Freem.Domain.Entities.Users;
using Freem.Entities.UseCases.Context;
using Freem.Entities.UseCases.Contracts.Users.Settings.Get;
using Freem.Web.Api.Public.Constants;
using Freem.Web.Api.Public.Contracts.DTO.Users.Settings;
using Freem.Web.Api.Public.Services.Implementations;
using Microsoft.AspNetCore.Mvc;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Web.Api.Public.Controllers.V1.Users.Settings;

[Route("api/v1/user/settings")]
[Tags(ControllerTags.User, ControllerTags.Settings)]
[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserSettingsResponse))]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public sealed class GetUserSettingsController : BaseController
{
    private readonly UseCaseContextProvider _contextProvider;
    private readonly IRequestMediator<HandlerContext> _mediator;

    public GetUserSettingsController(
        UseCaseContextProvider contextProvider, 
        IRequestMediator<HandlerContext> mediator)
    {
        ArgumentNullException.ThrowIfNull(contextProvider);
        ArgumentNullException.ThrowIfNull(mediator);
        
        _contextProvider = contextProvider;
        _mediator = mediator;
    }

    [HttpGet]
    [EndpointSummary("Get user settings")]
    public async Task<IActionResult> GetAsync(CancellationToken cancellationToken = default)
    {
        if (!_contextProvider.TryGet(out var context))
            return new UnauthorizedResult();

        var request = GetUserSettingsRequest.Instance;

        var response = await _mediator.ExecuteAsync<GetUserSettingsRequest, Result<GetUserSettingsResponse, Error>>(context, request, cancellationToken);

        return response.Success
            ? CreateSuccess(response.Value.Settings)
            : CreateFailure(response.Error);
    }

    private static IActionResult CreateSuccess(UserSettings settings)
    {
        var response = new UserSettingsResponse(settings.DayUtcOffset);
        return new OkObjectResult(response);
    }

    private static IActionResult CreateFailure(Error error)
    {
        if (error.Type == DomainErrorCatalog.Types.EntityNotFound)
            return new UnauthorizedResult();

        return new StatusCodeResult(StatusCodes.Status500InternalServerError);
    }
}