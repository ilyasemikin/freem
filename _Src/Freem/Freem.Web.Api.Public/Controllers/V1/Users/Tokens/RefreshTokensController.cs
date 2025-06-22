using System.ComponentModel.DataAnnotations;
using Freem.Entities.UseCases;
using Freem.Entities.UseCases.Contracts.Users.Tokens.Refresh;
using Freem.Entities.Users;
using Freem.UseCases.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;
using Freem.Web.Api.Public.Constants;
using Freem.Web.Api.Public.Contracts.DTO.Users.Tokens;
using Freem.Web.Api.Public.Services.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace Freem.Web.Api.Public.Controllers.V1.Users.Tokens;

[Route("api/v1/user/tokens/refresh")]
[Tags(ControllerTags.User, ControllerTags.AuthTokens)]
[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RefreshTokensResponse))]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status403Forbidden)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public sealed class RefreshTokensController : BaseController
{
    private readonly UseCaseContextProvider _contextProvider;
    private readonly IUseCaseExecutor<UseCaseExecutionContext> _executor;

    public RefreshTokensController(
        UseCaseContextProvider contextProvider, 
        IUseCaseExecutor<UseCaseExecutionContext> executor)
    {
        ArgumentNullException.ThrowIfNull(contextProvider);
        ArgumentNullException.ThrowIfNull(executor);
        
        _contextProvider = contextProvider;
        _executor = executor;
    }

    [HttpPost]
    [EndpointSummary("Refresh authentication tokens using refresh token")]
    public async Task<IActionResult> RefreshAsync(
        [Required] [FromBody] RefreshTokensRequest body,
        CancellationToken cancellationToken = default)
    {
        var context = _contextProvider.Get();
        var request = Map(body);

        var response = await _executor.ExecuteAsync<RefreshUserAccessTokenRequest, RefreshUserAccessTokenResponse>(context, request, cancellationToken);

        return response.Success
            ? CreateSuccess(response.Tokens)
            : CreateFailure(response.Error);
    }

    private static RefreshUserAccessTokenRequest Map(RefreshTokensRequest request)
    {
        return new RefreshUserAccessTokenRequest(request.RefreshToken);
    }

    private static IActionResult CreateSuccess(UserTokens tokens)
    {
        var response = new RefreshTokensResponse(tokens);
        return new OkObjectResult(response);
    }

    private static IActionResult CreateFailure(Error<RefreshUserAccessTokenErrorCode> error)
    {
        return error.Code switch
        {
            RefreshUserAccessTokenErrorCode.TokenInvalid => new ForbidResult(),
            RefreshUserAccessTokenErrorCode.UserNotFound => new ForbidResult(),
            _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
        };
    }
}