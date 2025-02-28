using Microsoft.AspNetCore.Authentication;

namespace Freem.Web.Api.Public.Authentication.DependencyInjection.Microsoft;

public static class ServiceCollectionExtensions
{
    public static AuthenticationBuilder AddJwtBearerAuthentication(
        this AuthenticationBuilder builder, Action<JwtBearerAuthenticationOptions>? configureOptions = null)
    {
        return builder.AddScheme<JwtBearerAuthenticationOptions, JwtBearerAuthenticationHandler>(
            JwtBearerAuthenticationHandler.AuthenticationScheme,
            configureOptions);
    }
}