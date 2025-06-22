using Freem.Entities.UseCases;
using Freem.Entities.UseCases.Contracts.Users.Tokens.Refresh;
using Freem.Entities.Users;
using Freem.UseCases.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;
using Freem.Web.Api.Public.Constants;
using Freem.Web.Api.Public.Contracts;
using Freem.Web.Api.Public.Contracts.DTO.Users.Tokens;
using Freem.Web.Api.Public.Services.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace Freem.Web.Api.Public.Controllers.V1.Users.Tokens;

[Route("api/v1/user/cookie-tokens/refresh")]
[Tags(ControllerTags.User, ControllerTags.AuthTokens)]
[ProducesResponseType(StatusCodes.Status200OK)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status403Forbidden)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public sealed class RefreshTokensCookieController : BaseController
{
    private readonly UseCaseContextProvider _contextProvider;
    private readonly IUseCaseExecutor<UseCaseExecutionContext> _executor;

    public RefreshTokensCookieController(
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
        CancellationToken cancellationToken = default)
    {
        if (!Request.Cookies.TryGetValue(CookieNames.RefreshToken, out var refreshToken))
            return BadRequest();
        
        var context = _contextProvider.Get();
        var request = Map(refreshToken);

        var response = await _executor.ExecuteAsync<RefreshUserAccessTokenRequest, RefreshUserAccessTokenResponse>(context, request, cancellationToken);

        return response.Success
            ? CreateSuccess(Response, response.Tokens)
            : CreateFailure(response.Error);
    }

    private static RefreshUserAccessTokenRequest Map(string refreshToken)
    {
        return new RefreshUserAccessTokenRequest(refreshToken);
    }

    private static IActionResult CreateSuccess(HttpResponse response, UserTokens tokens)
    {
        response.Cookies.Append(CookieNames.AccessToken, tokens.AccessToken, new CookieOptions
        {
            //HttpOnly = true,
            //Secure = true,
            Expires = DateTimeOffset.MaxValue
        });
        
        response.Cookies.Append(CookieNames.RefreshToken, tokens.RefreshToken, new CookieOptions
        {
            //HttpOnly = true,
            //Secure = true,
            Path = "/api/v1/user/tokens/refresh",
            Expires = DateTimeOffset.MaxValue
        });
        
        return new OkResult();
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