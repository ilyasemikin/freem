using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;
using Freem.Entities.Tokens.JWT.Implementations.AccessTokens;
using Freem.Web.Api.Public.Contracts;
using Microsoft.Extensions.Options;

namespace Freem.Web.Api.Public.Authentication;

public sealed class JwtCookieAuthenticationHandler : JwtAuthenticationHandlerBase<JwtCookieAuthenticationOptions>
{
    private const string AuthenticationType = "JWT Cookie";
    
    public JwtCookieAuthenticationHandler(
        IOptionsMonitor<JwtCookieAuthenticationOptions> options, 
        ILoggerFactory logger, 
        UrlEncoder encoder, 
        AccessTokenValidator validator) 
        : base(options, logger, encoder, validator, AuthenticationType)
    {
    }

    protected override bool TryGetToken([NotNullWhen(true)] out string? token)
    {
        return Request.Cookies.TryGetValue(CookieNames.AccessToken, out token);
    }
}