using System.ComponentModel.DataAnnotations;
using Freem.Domain;
using Freem.Entities.UseCases.Context;
using Freem.Web.Api.Public.Constants;
using Freem.Web.Api.Public.Services.Implementations;
using Microsoft.AspNetCore.Mvc;
using Utilities.Errors;
using Utilities.Primitives;
using Utilities.UseCases.Abstractions;
using ApiRegisterUserResponse = Freem.Web.Api.Public.Contracts.DTO.Users.RegisterUserResponse;
using ApiRegisterUserRequest = Freem.Web.Api.Public.Contracts.DTO.Users.RegisterUserRequest;
using RegisterUserRequest = Freem.Entities.UseCases.Contracts.Users.Register.RegisterUserRequest;
using RegisterUserResponse = Freem.Entities.UseCases.Contracts.Users.Register.RegisterUserResponse;

namespace Freem.Web.Api.Public.Controllers.V1.Users;

[Route("api/v1/user/register")]
[Tags(ControllerTags.User)]
[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiRegisterUserResponse))]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status409Conflict)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public sealed class RegisterUserController : BaseController
{
    private readonly UseCaseContextProvider _contextProvider;
    private readonly IRequestMediator<HandlerContext> _mediator;

    public RegisterUserController(
        UseCaseContextProvider contextProvider, 
        IRequestMediator<HandlerContext> mediator)
    {
        ArgumentNullException.ThrowIfNull(contextProvider);
        ArgumentNullException.ThrowIfNull(mediator);
        
        _contextProvider = contextProvider;
        _mediator = mediator;
    }

    [HttpPost]
    [EndpointSummary("Register user")]
    public async Task<IActionResult> RegisterAsync(
        [Required] [FromBody] ApiRegisterUserRequest body,
        CancellationToken cancellationToken = default)
    {
        if (!_contextProvider.TryGet(out var context))
            return new UnauthorizedResult();

        var request = new RegisterUserRequest(body.Nickname);

        var response = await _mediator.ExecuteAsync<RegisterUserRequest, Result<RegisterUserResponse, Error>>(context, request, cancellationToken);

        if (!response.Success)
            return CreateFailure(response.Error);

        var result = new ApiRegisterUserResponse(response.Value.UserId);
        return Ok(result);
    }

    private static IActionResult CreateFailure(Error error)
    {
        if (error.Type == DomainErrorCatalog.Types.EntityAlreadyExists)
            return CreateResult(StatusCodes.Status409Conflict);

        return CreateResult(StatusCodes.Status500InternalServerError);
    }
}
