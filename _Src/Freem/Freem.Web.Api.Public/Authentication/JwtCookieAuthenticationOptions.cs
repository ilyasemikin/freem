using Microsoft.AspNetCore.Authentication;

namespace Freem.Web.Api.Public.Authentication;

public sealed class JwtCookieAuthenticationOptions : AuthenticationSchemeOptions
{
    public const string SchemeName = "JwtCookieAuthentication";
}