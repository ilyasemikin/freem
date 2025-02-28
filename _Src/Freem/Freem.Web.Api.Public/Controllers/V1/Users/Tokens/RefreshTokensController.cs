using System.ComponentModel.DataAnnotations;
using Freem.Entities.UseCases;
using Freem.Entities.UseCases.Contracts.Users.Tokens.Refresh;
using Freem.UseCases.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;
using Freem.Web.Api.Public.Contracts.Users.Tokens;
using Freem.Web.Api.Public.Services.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace Freem.Web.Api.Public.Controllers.V1.Users.Tokens;

[Route("api/user/tokens/refresh")]
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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RefreshTokensResponse))]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RefreshAsync(
        [Required] [FromBody] RefreshTokensRequest body,
        CancellationToken cancellationToken = default)
    {
        var context = _contextProvider.Get();
        var request = Map(body);

        var response = await _executor.ExecuteAsync<RefreshUserAccessTokenRequest, RefreshUserAccessTokenResponse>(context, request, cancellationToken);

        return response.Success
            ? CreateSuccess(response.AccessToken, response.RefreshToken)
            : CreateFailure(response.Error);
    }

    private static RefreshUserAccessTokenRequest Map(RefreshTokensRequest request)
    {
        return new RefreshUserAccessTokenRequest(request.RefreshToken);
    }

    private static IActionResult CreateSuccess(string accessToken, string refreshToken)
    {
        var response = new RefreshTokensResponse(accessToken, refreshToken);
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