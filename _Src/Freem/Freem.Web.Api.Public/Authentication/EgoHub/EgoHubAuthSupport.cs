using Freem.Web.Api.Public.Contracts.DTO.Auth;
using Microsoft.AspNetCore.Mvc;
using EgoHub.Api.Public.Contracts.V1.Sessions;
using Utilities.Http.Errors;
using Utilities.Http.Errors.Abstractions;

namespace Freem.Web.Api.Public.Authentication.EgoHub;

internal static class EgoHubAuthSupport
{
    public const string RefreshCookieName = "eh_refresh_token";
    public const string RefreshCookiePath = "/api/v1/auth";

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
        var mapped = PassThroughStatusCodes.Contains(statusCode) ? statusCode : StatusCodes.Status502BadGateway;
        return new StatusCodeResult(mapped);
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
            Path = RefreshCookiePath,
            Expires = tokens.RefreshTokenExpiresAt
        };
        httpContext.Response.Cookies.Append(RefreshCookieName, tokens.RefreshToken, options);
    }

    public static void DeleteRefreshCookie(HttpContext httpContext)
    {
        var options = new CookieOptions { Path = RefreshCookiePath };
        httpContext.Response.Cookies.Delete(RefreshCookieName, options);
    }
}
