using Freem.Domain;
using Freem.Domain.Entities.Users;
using Freem.Entities.UseCases.Context;
using Freem.Entities.UseCases.Contracts.Users.Get;
using Freem.Web.Api.Public.Constants;
using Freem.Web.Api.Public.Contracts.DTO.Users;
using Freem.Web.Api.Public.Services.Implementations;
using Microsoft.AspNetCore.Mvc;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;

namespace Freem.Web.Api.Public.Controllers.V1.Users;

[Route("api/v1/user/me")]
[Tags(ControllerTags.User, ControllerTags.Settings)]
[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MeResponse))]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public sealed class MeController : BaseController
{
    private readonly UseCaseContextProvider _contextProvider;
    private readonly IRequestMediator<HandlerContext> _mediator;

    public MeController(
        UseCaseContextProvider contextProvider, 
        IRequestMediator<HandlerContext> mediator)
    {
        ArgumentNullException.ThrowIfNull(contextProvider);
        ArgumentNullException.ThrowIfNull(mediator);
        
        _contextProvider = contextProvider;
        _mediator = mediator;
    }

    [HttpGet]
    [EndpointSummary("Get information about me")]
    public async Task<IActionResult> GetAsync(CancellationToken cancellationToken = default)
    {
        if (!_contextProvider.TryGet(out var context))
            return new UnauthorizedResult();

        var request = GetUserRequest.Instance;

        var response = await _mediator.ExecuteAsync<GetUserRequest, Result<GetUserResponse, Error>>(context, request, cancellationToken);

        return response.Success
            ? CreateSuccess(response.Value.User)
            : CreateFailure(response.Error);
    }
    
    private static IActionResult CreateSuccess(User user)
    {
        var response = new MeResponse(user.Id, user.Nickname);
        return new OkObjectResult(response);
    }

    private static IActionResult CreateFailure(Error error)
    {
        if (error.Type == DomainErrorCatalog.Types.EntityNotFound)
            return new UnauthorizedResult();

        return new StatusCodeResult(StatusCodes.Status500InternalServerError);
    }
}