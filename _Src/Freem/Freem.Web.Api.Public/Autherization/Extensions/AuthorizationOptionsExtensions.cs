using Freem.Web.Api.Public.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace Freem.Web.Api.Public.Autherization.Extensions;

public static class AuthorizationOptionsExtensions
{
    public static void AddJwtPolicy(this AuthorizationOptions options)
    {
        options.AddPolicy(JwtAuthorizationPolicy.Name, policy =>
        {
            policy.AddAuthenticationSchemes(
                JwtBearerAuthenticationOptions.SchemeName,
                JwtCookieAuthenticationOptions.SchemeName);
            policy.RequireAuthenticatedUser();
        });
    }
}