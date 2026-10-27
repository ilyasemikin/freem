using EgoHub.Api.Public.Contracts.V1.Sessions;
using Freem.Api.Public.Contracts.V1.Auth;
using Freem.Domain;
using Freem.Api.Public.UseCases.Implementations.Mappers.Responses.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Utilities.Http.Errors;
using Utilities.Http.Errors.Abstractions;

namespace Freem.Api.Public.UseCases.Implementations.Handlers.Auth;

internal static class EgoHubAuthSupport
{
    public const string RefreshCookieName = "eh_refresh_token";

    // Relative to Request.PathBase, which is the prefix a reverse proxy stripped (X-Forwarded-Prefix): the browser
    // sees the prefixed path and sends the cookie only there.
    private static readonly PathString RefreshCookiePath = new("/api/v1/auth");

    // Only errors caused by the caller's input are passed through; a rejected service token, EgoHub failures
    // and network errors are not the caller's fault and become 502.
    private static readonly HashSet<int> PassThroughStatusCodes =
    [
        StatusCodes.Status400BadRequest,
        StatusCodes.Status401Unauthorized,
        StatusCodes.Status409Conflict,
        StatusCodes.Status422UnprocessableEntity
    ];

    public static IActionResult MapError(IClientError error)
    {
        var statusCode = GetStatusCode(error);
        if (!PassThroughStatusCodes.Contains(statusCode))
        {
            var dependencyError = DomainErrorCatalog.Factories.CreateFailedDependency(error.Message);
            return dependencyError.MapError(StatusCodes.Status502BadGateway);
        }

        var rejectedError = DomainErrorCatalog.Factories.CreateAuthenticationRejected(error.Message);
        return rejectedError.MapError(statusCode);
    }

    // The refresh cookie is dropped only when EgoHub has rejected the token itself, not on temporary failures.
    public static bool IsRejection(IClientError error)
    {
        var statusCode = GetStatusCode(error);
        return statusCode is >= 400 and < 500;
    }

    private static int GetStatusCode(IClientError error)
    {
        return error is HttpClientError httpError ? (int)httpError.Response.StatusCode : 0;
    }

    public static SessionResponse ToSessionResponse(SessionTokensInfo tokens)
    {
        return new SessionResponse(tokens.AccessToken, tokens.AccessTokenExpiresAt);
    }

    public static void AppendRefreshCookie(HttpContext httpContext, SessionTokensInfo tokens)
    {
        var options = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Path = GetRefreshCookiePath(httpContext),
            Expires = tokens.RefreshTokenExpiresAt
        };
        httpContext.Response.Cookies.Append(RefreshCookieName, tokens.RefreshToken, options);
    }

    public static void DeleteRefreshCookie(HttpContext httpContext)
    {
        var options = new CookieOptions { Path = GetRefreshCookiePath(httpContext) };
        httpContext.Response.Cookies.Delete(RefreshCookieName, options);
    }

    private static string GetRefreshCookiePath(HttpContext httpContext)
    {
        var path = httpContext.Request.PathBase.Add(RefreshCookiePath);
        return path.Value!;
    }
}
