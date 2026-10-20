using EgoHub.Api.Public.Client.Abstractions;
using EgoHub.Api.Public.Contracts.V1.Sessions;
using Freem.Domain;
using Freem.Api.Public.UseCases.Context;
using Freem.Api.Public.UseCases.Contracts.Auth;
using Freem.Api.Public.UseCases.Implementations.Mappers.Responses.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Freem.Api.Public.UseCases.Implementations.Handlers.Auth;

internal sealed class ApiRefreshRequestHandler : BaseAnonymousRequestHandler<ApiRefreshRequest>
{
    private const string MissingRefreshTokenMessage = "Refresh token cookie is missing";

    private readonly ISessionTokenClient _client;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ApiRefreshRequestHandler(ISessionTokenClient client, IHttpContextAccessor httpContextAccessor)
    {
        ArgumentNullException.ThrowIfNull(client);
        ArgumentNullException.ThrowIfNull(httpContextAccessor);

        _client = client;
        _httpContextAccessor = httpContextAccessor;
    }

    public override async Task<IActionResult> HandleAsync(
        ApiAnonymousHandlerContext context, ApiRefreshRequest request,
        CancellationToken cancellationToken = default)
    {
        var httpContext = _httpContextAccessor.HttpContext!;

        httpContext.Request.Cookies.TryGetValue(EgoHubAuthSupport.RefreshCookieName, out var refreshToken);
        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            var error = DomainErrorCatalog.Factories.CreateAuthenticationRejected(MissingRefreshTokenMessage);
            return error.MapError();
        }

        var egoHubRequest = new RefreshSessionTokensRequest { RefreshToken = refreshToken };
        var result = await _client.RefreshAsync(egoHubRequest, cancellationToken);
        if (!result.Success)
        {
            if (EgoHubAuthSupport.IsRejection(result.Error))
                EgoHubAuthSupport.DeleteRefreshCookie(httpContext);

            return EgoHubAuthSupport.MapError(result.Error);
        }

        var tokens = result.Value.Tokens;
        EgoHubAuthSupport.AppendRefreshCookie(httpContext, tokens);

        var response = EgoHubAuthSupport.ToSessionResponse(tokens);
        return new OkObjectResult(response);
    }
}
