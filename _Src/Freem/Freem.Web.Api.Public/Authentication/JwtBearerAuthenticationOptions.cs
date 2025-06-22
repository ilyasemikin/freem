using Microsoft.AspNetCore.Authentication;

namespace Freem.Web.Api.Public.Authentication;

public sealed class JwtBearerAuthenticationOptions : AuthenticationSchemeOptions
{
    public const string SchemeName = "JwtBearerAuthentication";
}