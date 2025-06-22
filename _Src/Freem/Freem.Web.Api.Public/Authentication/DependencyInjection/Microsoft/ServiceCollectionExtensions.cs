using Microsoft.AspNetCore.Authentication;

namespace Freem.Web.Api.Public.Authentication.DependencyInjection.Microsoft;

public static class ServiceCollectionExtensions
{
    public static AuthenticationBuilder AddJwtBearerAuthentication(
        this AuthenticationBuilder builder, Action<JwtBearerAuthenticationOptions>? configureOptions = null)
    {
        return builder.AddScheme<JwtBearerAuthenticationOptions, JwtBearerAuthenticationHandler>(
            JwtBearerAuthenticationOptions.SchemeName, 
            configureOptions);
    }

    public static AuthenticationBuilder AddJwtCookieAuthentication(
        this AuthenticationBuilder builder, Action<JwtCookieAuthenticationOptions>? configureOptions = null)
    {
        return builder.AddScheme<JwtCookieAuthenticationOptions, JwtCookieAuthenticationHandler>(
            JwtCookieAuthenticationOptions.SchemeName,
            configureOptions);
    }
}