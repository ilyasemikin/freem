using System.ComponentModel.DataAnnotations;
using Freem.Entities.UseCases;
using Freem.Entities.UseCases.Contracts.Users.Tokens.Refresh;
using Freem.UseCases.Abstractions;
using Freem.UseCases.Contracts.Abstractions.Errors;
using Freem.Web.Api.Public.Contracts.Users.Tokens;
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
    public async Task<ActionResult<RefreshTokensResponse>> RefreshAsync(
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

    private static RefreshTokensResponse CreateSuccess(string accessToken, string refreshToken)
    {
        return new RefreshTokensResponse(accessToken, refreshToken);
    }

    private static ActionResult<RefreshTokensResponse> CreateFailure(Error<RefreshUserAccessTokenErrorCode> error)
    {
        throw new NotImplementedException();
    }
}